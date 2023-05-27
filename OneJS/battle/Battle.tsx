import { h } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"

export const Battle = ({ battle }: { battle: any }) => {
  return (
    <div class="w-full h-full flex">
      <Menu />
      <Statusbar battle={battle} />
      <Result />
    </div>
  )
}