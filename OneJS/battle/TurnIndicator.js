"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TurnIndicator = void 0;
var preact_1 = require("preact");
var TurnIndicator = function (_a) {
    var turn = _a.turn;
    return ((0, preact_1.h)("div", { class: 'w-auto px-4 text-3xl bg-slate-500 font-bold text-slate-50' },
        "TURN: ",
        0));
};
exports.TurnIndicator = TurnIndicator;
