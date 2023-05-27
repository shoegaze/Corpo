import { h } from "preact"
import { MutableRef, useEffect, useRef } from "preact/hooks"

export const ModeIndicator = ({ mode }: { mode: number }) => {
  return (
    <span class='relative px-4 text-3xl bg-slate-500 text-slate-50'>
      {/* MODE: { mode === 0 ? 'GRID' : 'MENU' } */}
      MODE: HELLO
    </span>
  )
}