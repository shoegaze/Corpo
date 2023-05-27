import { h } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"
import { useRef } from "preact/hooks"

export const Battle = () => {
  return (
    <div class="w-full h-full flex">
      <Menu />
      <Statusbar />
      <Result />
    </div>
  )
}