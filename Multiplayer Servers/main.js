const canvas  = document.createElement('canvas'), context = canvas.getContext('2d');
document.body.append(canvas);

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