let cube = new Cube(1, 1, 1);

const camera = new Camera(25);
camera.setPosition(0, 0, -12);

setBackgroundColorHSV(0.65, 0.6, 0.1);

function tick() {
  let y = Math.cos(TIME.seconds);
  cube.setPosition(0, y, 0);
}

