import { h } from "preact"
import { ModeIndicator } from "./ModeIndicator"
import { useEventfulState } from "onejs"
import { font } from "preload"
import { TeamIndicator } from "./TeamIndicator"
import { TurnIndicator } from "./TurnIndicator"

export const Statusbar = () => {
  // const battleUI = require('battleUI')
  // const [mode, _] = useEventfulState(battleUI, 'Mode')
  // const [team, _] = useEventfulState(battleUI, 'Team')
  // const [turn, _] = useEventfulState(battleUI, 'Turn')

  return (
    <div class='absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold' style={{
      unityFontDefinition: font
    }}>
      <ModeIndicator mode={0} />
      <TeamIndicator team={0} />
      <TurnIndicator turn={0} />
    </div>
  )
}