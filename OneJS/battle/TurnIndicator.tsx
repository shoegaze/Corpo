import { h } from "preact"

export const TurnIndicator = ({ turn }: { turn: number }) => {
  return (
    <span class='relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50'>
      <span>TURN: </span>
      <span class='grow'></span>
      <span>{turn.toString().padStart(4, '0')}</span>
      <span class='grow'></span>
    </span>
  )
}