"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ModeIndicator = void 0;
var preact_1 = require("preact");
var ModeIndicator = function (_a) {
    var mode = _a.mode;
    return ((0, preact_1.h)("span", { class: 'relative px-4 text-3xl bg-slate-500 text-slate-50' }, "MODE: HELLO"));
};
exports.ModeIndicator = ModeIndicator;
