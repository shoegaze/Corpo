import { h } from "preact"

export const TurnIndicator = ({ turn }: { turn: number }) => {
  return (
    <div class='relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50'>
      {/* TODO: Use <label text='' /> instead */}
      <div>TURN: </div>
      <div class='grow'></div>
      <div>{turn.toString().padStart(4, '0')}</div>
      <div class='grow'></div>
    </div>
  )
}