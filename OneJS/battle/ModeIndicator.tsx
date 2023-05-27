import { h } from "preact"

export const ModeIndicator = ({ mode }: { mode: number }) => {
  return (
    <div>
      MODE: { mode === 0 ? 'GRID' : 'MENU' }
    </div>
  )
}