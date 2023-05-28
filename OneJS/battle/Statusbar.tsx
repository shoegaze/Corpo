import { h } from "preact"
import { ModeIndicator } from "./ModeIndicator"
import { useEventfulState } from "onejs"
import { font } from "preload"
import { TeamIndicator } from "./TeamIndicator"
import { TurnIndicator } from "./TurnIndicator"

export const Statusbar = ({ battleUI }: { battleUI: any }) => {
  const [mode, _0] = useEventfulState(battleUI, 'Mode')
  const [team, _1] = useEventfulState(battleUI, 'Team')
  const [turn, _2] = useEventfulState(battleUI, 'Turn')

  return (
    <div class='absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold' style={{
      unityFontDefinition: font
    }}>
      <ModeIndicator mode={mode} />
      <TeamIndicator team={team} />
      <TurnIndicator turn={turn} />
    </div>
  )
}