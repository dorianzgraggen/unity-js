writeFile("ahahaha", "mewo")
// lol();
// console2.log(multiply(4, 9));
// console2.log("alles klar");

// addEventListener("lucky", (e) => {
//   console2.log("wow, it's " + JSON.stringify(e));
// })



let cubes = [];

for (let i = 0; i < 5; i++) {
  let top = new Cube(0.4);
  let bottom = new Cube(0.4);
  top.setHSV(i / 5, 1, 0.8);
  bottom.setHSV(i / 5, 1, 0.3);
  cubes.push([top, bottom]);
}

let player = new Cube(0.2);
// player.enableGravity(true);

addEventListener("keydown", e => {
  console2.log(e);
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
    let x = -(TIME.seconds * 0.5 + i) % 5;
    top.setPosition(x, 1, 0);
    bottom.setPosition(x, -1, 0);
  });

  // testPerformance();
}

const TIME = {
  seconds: 0,
  deltaSeconds: 0
}
let secondsBefore = 0;
const interval = 16;

function core_loop() {
  handle_events()
  // getGlobals();
  tick()
  TIME.seconds += 1 / interval;
  TIME.deltaSeconds = TIME.seconds - secondsBefore;
  secondsBefore = TIME.seconds;
  if (!shouldExit()) {
    setTimeout(core_loop, interval);
  } else {
    console2.log("exiting")
  }
}

core_loop()
