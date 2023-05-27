"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Statusbar = void 0;
var preact_1 = require("preact");
var ModeIndicator_1 = require("./ModeIndicator");
var preload_1 = require("preload");
var TeamIndicator_1 = require("./TeamIndicator");
var TurnIndicator_1 = require("./TurnIndicator");
var Statusbar = function () {
    return ((0, preact_1.h)("div", { class: 'absolute flex flex-row w-full bottom-0 bg-slate-400 text-3xl font-bold', style: {
            unityFontDefinition: preload_1.font
        } },
        (0, preact_1.h)(ModeIndicator_1.ModeIndicator, { mode: 0 }),
        (0, preact_1.h)(TeamIndicator_1.TeamIndicator, { team: 0 }),
        (0, preact_1.h)(TurnIndicator_1.TurnIndicator, { turn: 0 })));
};
exports.Statusbar = Statusbar;
