import { h } from "preact"
import { useEventfulState } from "onejs"
import { font } from "preload"
import { ModeIndicator } from "./ModeIndicator"
import { TeamIndicator } from "./TeamIndicator"
import { TurnIndicator } from "./TurnIndicator"

export const Statusbar = ({ battleUI }: { battleUI: any }) => {
  const [panelState, _setPanelState] = useEventfulState(battleUI, 'PanelState')
  const [team, _setTeam] = useEventfulState(battleUI, 'Team')
  const [turn, _setTurn] = useEventfulState(battleUI, 'Turn')

  return (
    <div class='absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold' style={{
      unityFontDefinition: font
    }}>
      <ModeIndicator panelState={panelState} />
      <TeamIndicator team={team} />
      <TurnIndicator turn={turn} />
    </div>
  )
}