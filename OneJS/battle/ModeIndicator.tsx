import { h } from "preact"

export const ModeIndicator = ({ mode }: { mode: number }) => {
  return (
    <div class='relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50'>
      <div>MODE: </div>
      <div class='grow'></div>
      <div>{mode === 0 ? 'GRID' : 'MENU'}</div>
      <div class='grow'></div>
    </div>
  )
}