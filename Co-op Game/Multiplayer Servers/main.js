const canvas  = document.createElement('canvas'), context = canvas.getContext('2d');
document.body.append(canvas);

const statusEl = document.getElementById('connectionStatus');
const socketUrl = `${window.location.protocol === 'https:' ? 'wss' : 'ws'}://${window.location.hostname}:3000`;
const reconnectDelayBase = 1000;
const reconnectDelayMax = 10000;
const sessionStorageKey = 'controllerSessionId';

let socket = null;
let reconnectTimer = null;
let reconnectAttempt = 0;
let playerId = null;
let lastJoystick = null;


let width = canvas.width = innerWidth
let height = canvas.height = innerHeight

const FPS = 120;

function backgroud(){
    context.fillStyle = '#000';
    context.fillRect(0, 0, width, height);
}

function joystickConfigForScreen() {
    const minSide = Math.min(width, height);
    const radius = Math.max(70, Math.min(140, minSide * 0.22));
    const handleRadius = Math.max(28, Math.min(64, radius * 0.48));
    const margin = Math.max(20, minSide * 0.05);

    return {
        x: Math.max(radius + margin, width * 0.28),
        y: Math.min(height - radius - margin, Math.max(radius + margin, height * 0.55)),
        radius,
        handleRadius,
    };
}

function createJoystick() {
    const cfg = joystickConfigForScreen();
    return new Joystick(cfg.x, cfg.y, cfg.radius, cfg.handleRadius);
}

function resizeCanvas() {
    width = canvas.width = innerWidth;
    height = canvas.height = innerHeight;
    joystick = createJoystick();
}

let joystick = createJoystick();

addEventListener('resize', resizeCanvas);

setInterval(() => {
    backgroud();

    joystick.update();

}, 1000 / FPS);

function getOrCreateSessionId() {
    try {
        const existingId = localStorage.getItem(sessionStorageKey);
        if (existingId) {
            return existingId;
        }

        const newId = (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function')
            ? crypto.randomUUID()
            : `session-${Date.now()}-${Math.random().toString(16).slice(2)}`;

        localStorage.setItem(sessionStorageKey, newId);
        return newId;
    }
    catch {
        if (!window.__controllerSessionId) {
            window.__controllerSessionId = `session-${Date.now()}-${Math.random().toString(16).slice(2)}`;
        }

        return window.__controllerSessionId;
    }
}

const controllerSessionId = getOrCreateSessionId();

function setStatus(text, state) {
    statusEl.textContent = text;
    statusEl.className = `status status--${state}`;
}

function clearReconnectTimer() {
    if (reconnectTimer) {
        clearTimeout(reconnectTimer);
        reconnectTimer = null;
    }
}

function connectSocket() {
    if (socket && (socket.readyState === WebSocket.OPEN || socket.readyState === WebSocket.CONNECTING)) {
        return;
    }

    setStatus('Verbinden...', 'connecting');
    socket = new WebSocket(socketUrl);

    socket.onopen = () => {
        reconnectAttempt = 0;
        clearReconnectTimer();
        playerId = null;
        lastJoystick = null;
        setStatus('Verbonden', 'connected');

        socket.send(JSON.stringify({
            type: 'register',
            clientType: 'controller',
            sessionId: controllerSessionId
        }));
    };

    socket.onmessage = (event) => {
        const data = JSON.parse(event.data);

        if (data.type === 'welcome') {
            playerId = data.playerId;
            console.log('🎮 My Player ID:', playerId);
        }
    };

    socket.onerror = (error) => {
        console.log('⚠️ Socket error:', error);
    };

    socket.onclose = (event) => {
        console.log('❌ Socket closed:', event);

        if (event.code === 4001) {
            setStatus('Deze sessie is overgenomen door een ander tabblad/apparaat.', 'disconnected');
            return;
        }

        if (event.code === 4003) {
            setStatus('Server is vol (max spelers bereikt).', 'disconnected');
            return;
        }

        if (event.wasClean) {
            setStatus('Verbinding gesloten', 'disconnected');
            return;
        }

        const delay = Math.min(reconnectDelayMax, reconnectDelayBase * (2 ** reconnectAttempt));
        reconnectAttempt += 1;

        setStatus(`Verbinding weg. Opnieuw proberen over ${Math.round(delay / 1000)}s...`, 'reconnecting');

        clearReconnectTimer();
        reconnectTimer = setTimeout(() => {
            connectSocket();
        }, delay);
    };
}

connectSocket();

function sendInput(action) {
    if (!playerId || !socket || socket.readyState !== WebSocket.OPEN) return;
    socket.send(JSON.stringify({
        type: 'input',
        action: action
    }));
}

function sendJoystickInput(x, y) {
    if (!playerId || !socket || socket.readyState !== WebSocket.OPEN) return;
    const quantizedX = Number(x.toFixed(2));
    const quantizedY = Number(y.toFixed(2));
    const sameAsLast = lastJoystick !== null && quantizedX === lastJoystick.x && quantizedY === lastJoystick.y;
    if (sameAsLast) {
        return;
    }
    lastJoystick = { x: quantizedX, y: quantizedY };
    socket.send(JSON.stringify({
        type: "input",
        action: "joystick",
        x: quantizedX,
        y: quantizedY
    }));
}

window.sendJoystickInput = sendJoystickInput;

window.addEventListener('online', () => {
    if (!socket || socket.readyState === WebSocket.CLOSED) {
        reconnectAttempt = 0;
        connectSocket();
    }
});