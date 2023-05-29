"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Result = void 0;
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var Result = function (_a) {
    var battleUI = _a.battleUI;
    var _b = (0, onejs_1.useEventfulState)(battleUI, 'DoAlliesWin'), doAlliesWin = _b[0], _0 = _b[1];
    var _c = (0, onejs_1.useEventfulState)(battleUI, 'DoEnemiesWin'), doEnemiesWin = _c[0], _1 = _c[1];
    var baseClass = 'w-full py-[40px] text-[120px] text-center font-bold italic bg-slate-400';
    return ((0, preact_1.h)("div", { class: 'absolute flex flex-col w-full h-full' },
        (0, preact_1.h)("div", { class: 'grow' }),
        (0, preact_1.h)("div", { class: baseClass, style: {
                visibility: doAlliesWin || doEnemiesWin ? 'Visible' : 'Hidden'
            } }, doAlliesWin ? 'WIN' : 'GAME OVER'),
        (0, preact_1.h)("div", { class: 'grow' })));
};
exports.Result = Result;
