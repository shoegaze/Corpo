import { useEventfulState } from "onejs"
import { h } from "preact"

export const Result = ({ battleView }: { battleView: any }) => {
  const [doAlliesWin, _0] = useEventfulState(battleView, 'DoAlliesWin')
  const [doEnemiesWin, _1] = useEventfulState(battleView, 'DoEnemiesWin')

  const baseClass = 'w-full py-[40px] text-[120px] text-center font-bold italic bg-slate-400'

  return (
    <div class='absolute flex flex-col w-full h-full'>
      <div class='grow'></div>
      {/* TODO: Use <label text='' /> instead */}
      <div class={baseClass} style={{
        visibility: doAlliesWin || doEnemiesWin ? 'Visible' : 'Hidden'
      }}>
        {doAlliesWin ? 'WIN' : 'GAME OVER'}
      </div>
      <div class='grow'></div>
    </div>
  )
}