"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Statusbar = void 0;
var preact_1 = require("preact");
var onejs_1 = require("onejs");
var preload_1 = require("preload");
var ModeIndicator_1 = require("./ModeIndicator");
var TeamIndicator_1 = require("./TeamIndicator");
var TurnIndicator_1 = require("./TurnIndicator");
var Statusbar = function (_a) {
    var battleUI = _a.battleUI;
    var _b = (0, onejs_1.useEventfulState)(battleUI, 'Mode'), mode = _b[0], _setMode = _b[1];
    var _c = (0, onejs_1.useEventfulState)(battleUI, 'Team'), team = _c[0], _setTeam = _c[1];
    var _d = (0, onejs_1.useEventfulState)(battleUI, 'Turn'), turn = _d[0], _setTurn = _d[1];
    return ((0, preact_1.h)("div", { class: 'absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold', style: {
            unityFontDefinition: preload_1.font
        } },
        (0, preact_1.h)(ModeIndicator_1.ModeIndicator, { mode: mode }),
        (0, preact_1.h)(TeamIndicator_1.TeamIndicator, { team: team }),
        (0, preact_1.h)(TurnIndicator_1.TurnIndicator, { turn: turn })));
};
exports.Statusbar = Statusbar;
