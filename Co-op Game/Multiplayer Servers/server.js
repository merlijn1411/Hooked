const express = require("express");
const http = require("http");
const path = require("path");
const WebSocket = require("ws");
const { v4: uuidv4 } = require("uuid");

const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

let players = {}; // playerId → websocket
const MAX_PLAYERS = 4;
const LOG_HISTORY_PER_PLAYER = 10;
const logHistory = [];

function getLogHistoryLimit() {
    const playerCount = Object.keys(players).length;
    return Math.max(1, playerCount) * LOG_HISTORY_PER_PLAYER;
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

    if (logHistory.length > getLogHistoryLimit()) {
        logHistory.shift();
    }

    console.clear();
    for (const entry of logHistory) {
        console.log(entry);
    }
}

app.use(express.static(__dirname));

app.get("/", (req, res) => {
    res.sendFile(path.join(__dirname, "index.html"));
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

                if (Object.keys(players).length >= MAX_PLAYERS) {
                    ws.send(JSON.stringify({ type: "full" }));
                    ws.close();
                    return;
                }

                ws.playerId = uuidv4();
                players[ws.playerId] = ws;

                logMessage("🎮 Player joined:", ws.playerId);

                ws.send(JSON.stringify({
                    type: "welcome",
                    playerId: ws.playerId
                }));

                broadcastToUnity({
                    type: "player_joined",
                    playerId: ws.playerId,
                    count: Object.keys(players).length
                });
            }

            if (ws.clientType === "unity") {
                logMessage("🖥️ Unity connected");
            }

            return;
            }

            if (ws.clientType === "controller" && data.type === "input") {
                logMessage(`📩 Input from ${ws.playerId}:`, data);

                const unityInput = {
                    type: "input",
                    playerId: ws.playerId,
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

        if (ws.clientType === "controller" && ws.playerId) {
            logMessage("❌ Player disconnected:", ws.playerId);

            delete players[ws.playerId];

            broadcastToUnity({
                type: "player_left",
                playerId: ws.playerId,
                count: Object.keys(players).length
            });

            broadcastToUnity({
                type: "disconnect",
                playerId: ws.playerId
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