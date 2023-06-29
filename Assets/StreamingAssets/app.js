let cubes = [];

for (let i = 0; i < 6; i++) {
  let top = new Cube(0.4, 2, 0.4);
  let bottom = new Cube(0.4, 2, 0.4);

  top.setHSV(i / 6, 1, 0.8);
  bottom.setHSV(i / 6, 1, 0.3);

  cubes.push([top, bottom]);
}

let player = new Cube(0.2, 0.2, 0.2, "Bird");
// let player = new GLTF("bird.gltf", 0.2, 0.2, 0.2);
player.enableGravity(true);
// player.enableCollisions(true);

const camera = new Camera(25);
camera.setPosition(0, 0, -12);

setBackgroundColorHSV(0.65, 0.6, 0.1);

addEventListener("keydown", e => {
  if (e.key == "W") {
    player.setVelocity(0, 3, 0);
  }
})


addEventListener("keyup", e => {
  console2.log(e);
})

function testPerformance() {
  for (let i = 0; i < 100; i++) {
    // cube.setPosition(Math.cos(TIME.seconds * 2), Math.sin(TIME.seconds * 2), 0);
    multiply(4, i);
  }
}

function tick() {
  cubes.forEach(([top, bottom], i) => {
    let x = -(TIME.seconds * 0.4 + i) % cubes.length * 2;
    x += 6;
    let y = 2;
    top.setPosition(x, y, 0);
    bottom.setPosition(x, -y, 0);
  });

  // testPerformance();
}

