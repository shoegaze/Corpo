import { h } from "preact"

export const ModeIndicator = ({ panelState }: { panelState: number }) => {
  return (
    <div class='relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50'>
      <div>MODE: </div>
      <div class='grow'></div>
      <div>{panelState === 0 ? 'GRID' : 'MENU'}</div>
      <div class='grow'></div>
    </div>
  )
}