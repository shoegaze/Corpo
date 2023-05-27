import { h } from "preact"

export const TeamIndicator = ({ team }: { team: number }) => {
  const baseClass = 'relative w-auto mx-auto px-4 text-white font-bold'

  return team === 0 ? (
    <span class={`${baseClass} bg-lime-200`}>
      {'>> ALLY <<'}
    </span>
  ) : (
    <span class={`${baseClass} bg-rose-400`}>
      {'<< ENEMY >>'}
    </span>
  )
}