"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TurnIndicator = void 0;
var preact_1 = require("preact");
var TurnIndicator = function (_a) {
    var turn = _a.turn;
    return ((0, preact_1.h)("span", { class: 'relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50' },
        (0, preact_1.h)("span", null, "TURN: "),
        (0, preact_1.h)("span", { class: 'grow' }),
        (0, preact_1.h)("span", null, turn.toString().padStart(4, '0')),
        (0, preact_1.h)("span", { class: 'grow' })));
};
exports.TurnIndicator = TurnIndicator;
