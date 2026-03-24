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

app.use(express.static(__dirname));

app.get("/", (req, res) => {
    res.sendFile(path.join(__dirname, "index.html"));
});

console.log("✅ Server running on port 3000");

wss.on("connection", (ws) => {

    ws.clientType = "unknown";
    ws.playerId = null;

    ws.on("message", (message) => {
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

                console.log("🎮 Player joined:", ws.playerId);

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
                console.log("🖥️ Unity connected");
            }

            return;
        }

        if (ws.clientType === "controller" && data.type === "input") {
            console.log(`📩 Input from ${ws.playerId}:`, data);

            broadcastToUnity({
                type: "input",
                playerId: ws.playerId,
                action: data.action
            });
        }
    });

    ws.on("close", () => {

        if (ws.clientType === "controller" && ws.playerId) {
            console.log("❌ Player disconnected:", ws.playerId);

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
            console.log("❌ Unity disconnected");
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
    console.log("🌐 HTTP server on http://localhost:3000");
    console.log("🔌 WebSocket on ws://localhost:3000");
});