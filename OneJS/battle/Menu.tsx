import { useEventfulState } from "onejs"
import { h } from "preact"
import { font } from "preload"

export const Menu = ({ battleUI }: { battleUI: any }) => {
  const [panelState, _setPanelState] = useEventfulState(battleUI, 'PanelState')
  const [activeActor, _setActiveActor] = useEventfulState(battleUI, 'ActiveActor')
  const [abilityIndex, _setAbilityIndex] = useEventfulState(battleUI, 'AbilityIndex')

  return (
    <div class='absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px] bg-slate-700' style={{
      unityFontDefinition: font,
      borderColor: 'white',
      borderWidth: panelState === 0 ? 0 : 6
    }}>
      <div class='flex flex-row px-6 py-2 text-5xl bg-cyan-500' style={{
        backgroundColor: panelState === 0 ? '#A9D4DE' : '#06B6DD'
      }}>
        <label text='*' class='mr-6' />
        <label text={activeActor?.Name ?? ''} />
        <div class='grow'></div>
        <label text={`${activeActor?.Health ?? 0} / ${activeActor?.MaxHealth ?? 0}`} />
      </div>

      <div class='flex flex-row mb-2 px-6 text-4xl text-slate-200 bg-cyan-700'>
        <label text="Icon" />
        <div class='grow'></div>
        <label text="Name" />
        <div class='grow'></div>
        <label text="Cost" />
      </div>

      <div class='text-4xl'>
        {/* TODO: Change ability type to Ability def */}
        {/* TODO: Store conditions as variables */}
        {activeActor?.Abilities.map((ability: any, i: number) => (
          <div class='flex flex-row px-6 py-3' style={{
            backgroundColor: i % 2 === 0 ? '#7DAFBD' : '#51778A',
            borderColor: 'white',
            borderTopWidth: panelState === 1 && i === abilityIndex ? 4 : 0,
            borderBottomWidth: panelState === 1 && i === abilityIndex ? 4 : 0
          }}>
            <image sprite={ability.Icon} class='w-[64px] h-[64px] bg-slate-700' />
            <div class='grow'></div>
            <label text={ability.Name} />
            <div class='grow'></div>
            <label text={ability.Cost.toString()} />
          </div>
        )) ?? ''}
      </div>
    </div>
  )
}