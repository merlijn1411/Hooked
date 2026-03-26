function circle(pos, radius, color) {
    context.beginPath();
    context.fillStyle = color;
    context.arc(pos.x, pos.y, radius, 0, Math.PI * 2);
    context.fill();
    context.closePath();
}

class Joystick {
    constructor(x, y, radius, handleRadius) {
        this.pos = new vector2(x, y);
        this.origin = new vector2(x, y);
        this.radius = radius;
        this.handleRadius = handleRadius;
        this.handleFriction = 0.5;
        this.ondrag = false;
        this.touchPos = new vector2(0, 0);
        this.activeTouchId = null;
        this.listener();
    }
    isInsideStartArea(point) {
        return point.sub(this.origin).mag() <= this.radius * 1.6;
    }
    listener() {
        addEventListener('touchstart', e => {
            if (this.ondrag) {
                return;
            }

            for (const touch of e.changedTouches) {
                const target = touch.target;
                if (target && typeof target.closest === 'function' && target.closest('.buttons')) {
                    continue;
                }

                const point = new vector2(touch.pageX, touch.pageY);
                if (!this.isInsideStartArea(point)) {
                    continue;
                }

                this.touchPos = point;
                this.activeTouchId = touch.identifier;
                this.ondrag = true;
                break;
            }
        });
        addEventListener('touchend', e => {
            if (!this.ondrag) {
                return;
            }

            for (const touch of e.changedTouches) {
                if (touch.identifier === this.activeTouchId) {
                    this.ondrag = false;
                    this.activeTouchId = null;
                    break;
                }
            }
        });
        addEventListener('touchmove', e => {
            if (!this.ondrag) {
                return;
            }

            for (const touch of e.touches) {
                if (touch.identifier === this.activeTouchId) {
                    this.touchPos = new vector2(touch.pageX, touch.pageY);
                    break;
                }
            }
        });
        addEventListener('touchcancel', e => {
            if (!this.ondrag) {
                return;
            }

            for (const touch of e.changedTouches) {
                if (touch.identifier === this.activeTouchId) {
                    this.ondrag = false;
                    this.activeTouchId = null;
                    break;
                }
            }
        });
    }
    reposition() {
        if (this.ondrag == false) {
            this.pos = this.pos.add(this.origin.sub(this.pos).mul(this.handleFriction));
        } else {
            const diff = this.touchPos.sub(this.origin);
            const maxDist = Math.min(diff.mag(), this.radius);
            this.pos = this.origin.add(diff.normalize().mul(maxDist));
        }
    }
    getAxis() {
        const delta = this.pos.sub(this.origin);
        const rawX = delta.x / this.radius;
        const rawY = delta.y / this.radius;

        const clamp = (value) => Math.max(-1, Math.min(1, value));
        const deadzone = 0.08;

        let x = clamp(rawX);
        let y = clamp(rawY);

        if (Math.abs(x) < deadzone) x = 0;
        if (Math.abs(y) < deadzone) y = 0;

        return { x, y };
    }
    draw(){
        // draw joystick base
        circle(this.origin, this.radius, '#707070');
        //draw handle
        circle(this.pos, this.handleRadius, '#3d3d3d');
    }
    update() {
        this.reposition();
        if (typeof window.sendJoystickInput === 'function') {
            const axis = this.getAxis();
            window.sendJoystickInput(axis.x, axis.y);
        }
        this.draw();
    }
}