import { h } from "preact"

export const TurnIndicator = ({ turn }: { turn: number }) => {
  return (
    <div class='w-auto px-4 text-3xl bg-slate-500 font-bold text-slate-50'>
      {/* TURN: {turn} */}
      TURN: {0}
    </div>
  )
}