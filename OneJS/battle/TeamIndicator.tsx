import { h } from "preact"

export const TeamIndicator = ({ team }: { team: number }) => {
  const baseClass = 'w-auto mx-auto px-4 text-white font-bold'

  return team === 0 ? (
    <div class={`${baseClass} bg-lime-200`}>
      {'>> ALLY <<'}
    </div>
  ) : (
    <div class={`${baseClass} bg-rose-400`}>
      {'<< ENEMY >>'}
    </div>
  )
}