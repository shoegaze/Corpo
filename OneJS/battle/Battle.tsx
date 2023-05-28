import { h, render } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"

const Battle = () => {
  const battleUI = require('battle_ui')

  return (
    <div class="absolute flex w-full h-full">
      <Menu battleUI={battleUI} />
      <Statusbar battleUI={battleUI} />
      <Result battleUI={battleUI}/>
    </div>
  )
}

render(<Battle />, document.body)