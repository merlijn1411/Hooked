const socket = new WebSocket(`ws://${window.location.hostname}:3000`);//school = 10.120.18.225

let playerId = null;

socket.onopen = () => {
    socket.send(JSON.stringify({
        type: "register",
        clientType: "controller"
    }));
};

socket.onmessage = (event) => {
    const data = JSON.parse(event.data);

    if (data.type === "welcome") {
        playerId = data.playerId;
        console.log("🎮 My Player ID:", playerId);
    }
};

socket.onclose = (event) => {
    console.log("❌ Socket closed:", event);
};

socket.onerror = (error) => {
    console.log("⚠️ Socket error:", error);
};

function sendInput(action) {
    if (!playerId) return;

    socket.send(JSON.stringify({
        type: "input",
        action: action
    }));
}