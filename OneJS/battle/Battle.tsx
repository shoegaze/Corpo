import { h } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"

export const Battle = () => {
  return (
    <div class="w-full h-full" style={{ backgroundColor: 'red' }}>
      <Menu />
      <Statusbar />
      <Result />
    </div>
  )
}