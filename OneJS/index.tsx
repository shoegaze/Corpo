import { Battle } from "battle/Battle"
import { useEventfulState } from "onejs"
import { h, render } from "preact"

const App = () => {
  const game = require('game')
  const [mode, _] = useEventfulState(game, 'Mode')

  switch (mode) {
    // TODO: Compare with C# enum GameMode
    // world
    case 0:
      return (
        <div class='w-full h-full'>
          Hello, world!
        </div>
      )

    // battle
    case 1:
      return <Battle />

    default:
      return <div></div>
  }
}

render(<App />, document.body)