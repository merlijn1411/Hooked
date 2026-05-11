const express = require("express");
const http = require("http");
const path = require("path");
const WebSocket = require("ws");
const { v4: uuidv4 } = require("uuid");

const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

let players = {}; 
let sessions = {}; 
let playerIndices = {};
const MAX_PLAYERS = 4;
const LOG_HISTORY_PER_PLAYER = 15;
const logHistory = [];

function getLogHistoryLimit() {
    const playerCount = Object.keys(players).length;
    return Math.max(1, playerCount) * LOG_HISTORY_PER_PLAYER;
}

function trimLogHistory() {
    while (logHistory.length > getLogHistoryLimit()) {
        logHistory.shift();
    }
}

function getAvailablePlayerIndex() {
    const usedIndices = new Set(Object.values(playerIndices));

    for (let index = 0; index < MAX_PLAYERS; index++) {
        if (!usedIndices.has(index)) {
            return index;
        }
    }

    return -1;
}

function formatLogPart(part) {
    if (typeof part === "string") {
        return part;
    }

    try {
        return JSON.stringify(part);
    }
    catch {
        return String(part);
    }
}

function logMessage(...parts) {
    const line = parts.map(formatLogPart).join(" ");
    logHistory.push(line);

    trimLogHistory();

    console.clear();
    for (const entry of logHistory) {
        console.log(entry);
    }
}


const basePath = process.pkg
  ? path.dirname(process.execPath)
  : __dirname;

app.use(express.static(path.join(basePath, "public")));

app.get("/", (req, res) => {
  res.sendFile(path.join(basePath, "public", "index.html"));
});

logMessage("✅ Server running on port 3000");

wss.on("connection", (ws) => {

    ws.clientType = "unknown";
    ws.playerId = null;

    ws.on("message", (message) => {
        try {
            const data = JSON.parse(message);

            if (data.type === "register") {
                ws.clientType = data.clientType;

                if (ws.clientType === "controller") {
                    const sessionId = typeof data.sessionId === "string" && data.sessionId.trim() !== ""
                        ? data.sessionId.trim()
                        : null;

                    if (sessionId && sessions[sessionId]) {
                        const existingPlayerId = sessions[sessionId];
                        const existingWs = players[existingPlayerId];
                        const existingPlayerIndex = typeof playerIndices[existingPlayerId] === "number"
                            ? playerIndices[existingPlayerId]
                            : getAvailablePlayerIndex();

                        if (existingWs && existingWs !== ws) {
                            try {
                                existingWs.close(4001, "Superseded by a newer controller connection");
                            }
                            catch {
                                existingWs.terminate();
                            }
                        }

                        ws.playerId = existingPlayerId;
                        ws.playerIndex = existingPlayerIndex;
                        ws.sessionId = sessionId;
                        players[existingPlayerId] = ws;
                        playerIndices[existingPlayerId] = existingPlayerIndex;

                        logMessage("🔄 Player reconnected:", ws.playerId);

                        ws.send(JSON.stringify({
                            type: "welcome",
                            playerId: ws.playerId,
                            playerIndex: ws.playerIndex
                        }));

                        return;
                    }

                    if (Object.keys(players).length >= MAX_PLAYERS) {
                        ws.send(JSON.stringify({ type: "full" }));
                        ws.close(4003, "Server full");
                        return;
                    }

                    ws.playerId = uuidv4();
                    ws.playerIndex = getAvailablePlayerIndex();
                    if (ws.playerIndex === -1) {
                        ws.send(JSON.stringify({ type: "full" }));
                        ws.close(4003, "Server full");
                        return;
                    }
                    ws.sessionId = sessionId;
                    players[ws.playerId] = ws;
                    playerIndices[ws.playerId] = ws.playerIndex;
                    if (sessionId) {
                        sessions[sessionId] = ws.playerId;
                    }

                    logMessage("🎮 Player joined:", ws.playerId, "index:", ws.playerIndex);

                    ws.send(JSON.stringify({
                        type: "welcome",
                        playerId: ws.playerId,
                        playerIndex: ws.playerIndex
                    }));

                    broadcastToUnity({
                        type: "player_joined",
                        playerId: ws.playerId,
                        playerIndex: ws.playerIndex,
                        count: Object.keys(players).length
                    });
                    return;
                }

                if (ws.clientType === "unity") {
                    logMessage("🖥️ Unity connected");
                }

                return;
            }

            if (ws.clientType === "controller" || data.type === "input") {
                logMessage(`📩 Input from ${ws.playerId}:`, data);

                const unityInput = {
                    type: "input",
                    playerId: ws.playerId,
                    playerIndex: ws.playerIndex,
                    action: data.action
                };

                if (typeof data.x === "number") {
                    unityInput.x = data.x;
                }

                if (typeof data.y === "number") {
                    unityInput.y = data.y;
                }

                broadcastToUnity(unityInput);
            }
        }
        catch (err) {
            logMessage("❌ JSON error:", message.toString());
        }

    });

    ws.on("close", () => {

        if (ws.clientType === "controller" && ws.playerId && players[ws.playerId] === ws) {
            delete players[ws.playerId];
            delete playerIndices[ws.playerId];

            logMessage("❌ Player disconnected:", ws.playerId, "index:", ws.playerIndex);
            trimLogHistory();

            broadcastToUnity({
                type: "player_left",
                playerId: ws.playerId,
                playerIndex: ws.playerIndex,
                count: Object.keys(players).length
            });

            broadcastToUnity({
                type: "disconnect",
                playerId: ws.playerId,
                playerIndex: ws.playerIndex
            });
        }

        if (ws.clientType === "unity") {
            logMessage("❌ Unity disconnected");
        }
    });
});

function broadcastToUnity(data) {
    const msg = JSON.stringify(data);

    wss.clients.forEach(client => {
        if (client.readyState === WebSocket.OPEN) {
            client.send(msg);
        }
    });
}

server.listen(3000, () => {
    logMessage("🌐 HTTP server on http://localhost:3000");
    logMessage("🔌 WebSocket on ws://localhost:3000");
});