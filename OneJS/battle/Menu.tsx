import { useEventfulState } from "onejs"
import { h } from "preact"
import { font } from "preload"

export const Menu = ({ battleUI }: { battleUI: any }) => {
  // const [activeActor, _] = useEventfulState(battleUI, 'ActiveActor')

  return (
    <div class='absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px]' style={{
      unityFontDefinition: font,
      backgroundColor: 'cyan'
    }}>
      <div class='flex flex-row px-6 py-2 text-5xl bg-teal-400'>
        <span>* djimpp</span>
        <span class='grow'></span>
        <span>4 / 4</span>
      </div>

      <div class='flex flex-row px-6 text-4xl' style={{
        backgroundColor: 'skyblue'
      }}>
        <span>Icon</span>
        <span class='grow'></span>
        <span>Name</span>
        <span class='grow'></span>
        <span>Cost</span>
      </div>

      <div class='text-4xl'>
        <div class='flex flex-row px-6 py-3' style={{
          backgroundColor: 'salmon'
        }}>
          <span class=''>%</span>
          <span class='grow'></span>
          <span class=''>Tackle</span>
          <span class='grow'></span>
          <span class=''>20</span>
        </div>
        <div class='flex flex-row px-6 py-3' style={{
          backgroundColor: 'violet'
        }}>
          <span class=''>!</span>
          <span class='grow'></span>
          <span class=''>Snap</span>
          <span class='grow'></span>
          <span class=''>30</span>
        </div>
        <div class='flex flex-row px-6 py-3' style={{
          backgroundColor: 'salmon'
        }}>
          <span class=''>#</span>
          <span class='grow'></span>
          <span class=''>Censor</span>
          <span class='grow'></span>
          <span class=''>50</span>
        </div>
      </div>
    </div>
  )
}