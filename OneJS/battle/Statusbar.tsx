import { h } from "preact"
import { useEventfulState } from "onejs"
import { font } from "preload"
import { ModeIndicator } from "./ModeIndicator"
import { TeamIndicator } from "./TeamIndicator"
import { TurnIndicator } from "./TurnIndicator"

export const Statusbar = ({ battleView }: { battleView: any }) => {
  const [panelState, _setPanelState] = useEventfulState(battleView, 'PanelState')
  const [team, _setTeam] = useEventfulState(battleView, 'Team')
  const [turn, _setTurn] = useEventfulState(battleView, 'Turn')

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