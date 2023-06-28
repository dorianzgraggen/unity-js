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
