import { ScrollerVisibility } from "UnityEngine/UIElements"
import { useEventfulState } from "onejs"
import { h } from "preact"
import { useState } from "preact/hooks"
import { font } from "preload"

export const Menu = ({ battleUI }: { battleUI: any }) => {
  const [panelState, _setPanelState] = useEventfulState(battleUI, 'PanelState')
  const [focusState, _setFocusState] = useEventfulState(battleUI, 'FocusState')
  const [activeActor, _setActiveActor] = useEventfulState(battleUI, 'ActiveActor')
  const [abilityIndex, _setAbilityIndex] = useEventfulState(battleUI, 'AbilityIndex')

  const [hoveredAbility, setHoveredAbility] = useState(null)

  return (
    <div class='menu-container absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px] bg-slate-700' style={{
      unityFontDefinition: font,
      borderColor: 'white',
      borderWidth: panelState === 0 ? 0 : 6
    }}>

      <div class='actor-info flex flex-row px-6 py-2 text-5xl bg-cyan-500' style={{
        backgroundColor: panelState === 0 ? '#A9D4DE' : '#06B6DD'
      }}>
        <label text='*' class='mr-6' />
        <label text={activeActor?.Name ?? ''} />
        <div class='grow'></div>
        <label text={`${activeActor?.Health ?? 0} / ${activeActor?.MaxHealth ?? 0}`} />
      </div>

      <div class='ability-labels flex flex-row mb-2 px-6 text-4xl text-slate-200 bg-cyan-700'>
        <label text="Icon" />
        <div class='grow'></div>
        <label text="Name" />
        <div class='grow'></div>
        <label text="Cost" />
      </div>

      <scrollview
        class='abilities-container grow text-4xl'
        vertical-scroller-visibility={ScrollerVisibility.Auto}>

        {/* TODO: Change ability type to Ability def */}
        {/* TODO: Store conditions as variables */}
        {activeActor?.Abilities.map((ability: any, i: number) => {
          const isEvenEntry = i % 2 === 0
          const isMenuFocused = panelState === 1
          const isAbilityFocused = focusState === 1 || focusState === 2
          const isHovered = i === abilityIndex

          // TODO: Will set...() on re-render, probably not a good idea
          if (isHovered) {
            setHoveredAbility(ability)
          }

          return (
            <div class='ability-entry flex flex-row px-6 py-3'
              key={i.toString()}
              style={{
                backgroundColor: isEvenEntry ? '#7DAFBD' : '#51778A',
                borderColor: 'white',
                // borderTopWidth: isSelected && isMenuFocused ? 4 : 0,
                // borderBottomWidth: isSelected && isMenuFocused ? 4 : 0,
                marginLeft: isHovered && isAbilityFocused ? -16 : 0,
                marginRight: isHovered && isAbilityFocused ? 16 : 0
              }}>
              <image sprite={ability.Icon} class='w-[64px] h-[64px] bg-slate-700' />
              <div class='grow'></div>
              <label text={ability.Name} />
              <div class='grow'></div>
              <label text={ability.Cost.toString()} />
            </div>
          )
        }) ?? ''}
      </scrollview>

      {
        hoveredAbility ? (
          <div class='ability-desc relative bottom w-full'>
            <div class='ability-desc-title flex flex-row text-5xl bg-slate-400'>
              <label text={hoveredAbility.Name} />
              <div class='grow'></div>
              <label text={`$${hoveredAbility.Cost}`} />
            </div>

            <scrollview
              class='ability-desc-text h-64 text-4xl bg-slate-200'
              vertical-scroller-visibility={ScrollerVisibility.Auto}>

              <label text={hoveredAbility.Description ?? 'no desc.'} />
            </scrollview>
          </div>
        ) : ''
      }
    </div>
  )
}