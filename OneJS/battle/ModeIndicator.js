"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ModeIndicator = void 0;
var preact_1 = require("preact");
var ModeIndicator = function (_a) {
    var mode = _a.mode;
    return ((0, preact_1.h)("span", { class: 'relative flex flex-row w-[250px] px-4 text-3xl bg-[#4D626F] text-slate-50' },
        (0, preact_1.h)("span", null, "MODE: "),
        (0, preact_1.h)("span", { class: 'grow' }),
        (0, preact_1.h)("span", null, mode === 0 ? 'GRID' : 'MENU'),
        (0, preact_1.h)("span", { class: 'grow' })));
};
exports.ModeIndicator = ModeIndicator;
