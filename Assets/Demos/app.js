writeFile("ahahaha", "mewo")
// lol();
console2.log(multiply(4, 9));
console2.log("alles klar");

addEventListener("lucky", (e) => {
  console2.log("wow, it's " + JSON.stringify(e));
})


function tick() {

}

function core_loop() {
  handle_events()
  tick()
  setTimeout(core_loop, 100);
}

core_loop()
