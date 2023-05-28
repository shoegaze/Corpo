import { h } from "preact"

export const ModeIndicator = ({ mode }: { mode: number }) => {
  return (
    <span class='relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50'>
      <span>MODE: </span>
      <span class='grow'></span>
      <span>{mode === 0 ? 'GRID' : 'MENU'}</span>
      <span class='grow'></span>
    </span>
  )
}