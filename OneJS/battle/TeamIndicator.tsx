import { h } from "preact"

export const TeamIndicator = ({ team }: { team: number }) => {
  const baseClass = 'relative w-auto mx-auto px-4 text-white font-bold'

  return team === 0 ? (
    /* TODO: Use <label text='' /> instead */
    <div class={`${baseClass} bg-[#80DD83]`}>
      {'>> ALLY <<'}
    </div>
  ) : (
    <div class={`${baseClass} bg-[#EC4242]`}>
      {'<< ENEMY >>'}
    </div>
  )
}