import { useEventfulState } from "onejs"
import { h } from "preact"
import { font } from "preload"

export const Menu = ({ battleUI }: { battleUI: any }) => {
  const [activeActor, _setActiveActor] = useEventfulState(battleUI, 'ActiveActor')

  return (
    <div class='absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px]' style={{
      unityFontDefinition: font,
      backgroundColor: 'cyan'
    }}>
      <div class='flex flex-row px-6 py-2 text-5xl bg-teal-400'>
        <label text={`* ${activeActor?.Name}`} />
        <div class='grow'></div>
        <label text={`${activeActor?.Health} / ${activeActor?.MaxHealth}`} />
      </div>

      <div class='flex flex-row px-6 text-4xl' style={{
        backgroundColor: 'skyblue'
      }}>
        <label text="Icon" />
        <div class='grow'></div>
        <label text="Name" />
        <div class='grow'></div>
        <label text="Cost" />
      </div>

      <div class='text-4xl'>
        {/* TODO: Change ability type to Ability def */}
        {activeActor?.Abilities.map((ability: any, i: number) => (
          <div class='flex flex-row px-6 py-3' style={{
            backgroundColor: i % 2 === 0 ? 'salmon' : 'violet'
          }}>
            <image class='w-[64px] h-[64px]' sprite={ability.Icon} />
            <div class='grow'></div>
            <label text={ability.Name} />
            <div class='grow'></div>
            <label text={ability.Cost.toString()} />
          </div>
        ))}
      </div>
    </div>
  )
}