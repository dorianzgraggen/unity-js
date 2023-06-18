writeFile("ahahaha", "mewo")
// lol();
console2.log(multiply(4, 9));
console2.log("alles klar");

addEventListener("lucky", (e) => {
  console2.log("wow, it's " + JSON.stringify(e));
})


let cube = new Cube(20, "red");
console2.log(cube)
console2.log("based")

function tick() {
  console2.log(TIME.seconds)
  cube.setPosition(Math.cos(TIME.seconds * 5), Math.sin(TIME.seconds * 5), 0);
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
  setTimeout(core_loop, interval);
}

core_loop()
