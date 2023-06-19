writeFile("ahahaha", "mewo")
// lol();
// console2.log(multiply(4, 9));
// console2.log("alles klar");

// addEventListener("lucky", (e) => {
//   console2.log("wow, it's " + JSON.stringify(e));
// })


let cube = new Cube(0.4, "red");
let cube2 = new Cube(1, "red");
console2.log(cube)
console2.log("based")


function testPerformance() {
  for (let i = 0; i < 100; i++) {
    // cube.setPosition(Math.cos(TIME.seconds * 2), Math.sin(TIME.seconds * 2), 0);
    multiply(4, i);
  }
}

function tick() {
  cube.setPosition(Math.cos(TIME.seconds * 2), Math.sin(TIME.seconds * 2), 0);
  cube2.setPosition(Math.sin(TIME.seconds * 2), Math.cos(TIME.seconds * 2), 0);

  testPerformance();
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
