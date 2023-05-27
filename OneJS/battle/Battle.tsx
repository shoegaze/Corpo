import { h } from "preact"
import { Menu } from "./Menu"
import { Statusbar } from "./Statusbar"
import { Result } from "./Result"
import { useRef } from "preact/hooks"

export const Battle = ({ battle }: { battle: any }) => {
  const ui = battle.UI

  return (
    <div class="w-full h-full flex">
      <Menu />
      <Statusbar ui={ui} />
      <Result />
    </div>
  )
}