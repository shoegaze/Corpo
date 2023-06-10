import { h, render } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"

const Battle = () => {
  const battleView = require('battle_view')

  return (
    <div class="absolute flex w-full h-full">
      <Menu battleView={battleView} />
      <Statusbar battleView={battleView} />
      <Result battleView={battleView} />
    </div>
  )
}

render(<Battle />, document.body)