"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Statusbar = void 0;
var preact_1 = require("preact");
var ModeIndicator_1 = require("./ModeIndicator");
var onejs_1 = require("onejs");
var preload_1 = require("preload");
var TeamIndicator_1 = require("./TeamIndicator");
var TurnIndicator_1 = require("./TurnIndicator");
var Statusbar = function (_a) {
    var ui = _a.ui;
    var _b = (0, onejs_1.useEventfulState)(ui, 'Mode'), mode = _b[0], _0 = _b[1];
    var _c = (0, onejs_1.useEventfulState)(ui, 'Team'), team = _c[0], _1 = _c[1];
    var _d = (0, onejs_1.useEventfulState)(ui, 'Turn'), turn = _d[0], _2 = _d[1];
    return ((0, preact_1.h)("div", { class: 'absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold', style: {
            unityFontDefinition: preload_1.font
        } },
        (0, preact_1.h)(ModeIndicator_1.ModeIndicator, { mode: mode }),
        (0, preact_1.h)(TeamIndicator_1.TeamIndicator, { team: team }),
        (0, preact_1.h)(TurnIndicator_1.TurnIndicator, { turn: turn })));
};
exports.Statusbar = Statusbar;
