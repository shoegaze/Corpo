import { h } from "preact"

export const TeamIndicator = ({ team }: { team: number }) => {
  const baseClass = 'relative w-auto mx-auto px-4 text-white font-bold'

  return team === 0 ? (
    <span class={`${baseClass} bg-[#80DD83]`}>
      {'>> ALLY <<'}
    </span>
  ) : (
    <span class={`${baseClass} bg-[#EC4242]`}>
      {'<< ENEMY >>'}
    </span>
  )
}