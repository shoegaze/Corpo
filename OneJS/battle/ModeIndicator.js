"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ModeIndicator = void 0;
var preact_1 = require("preact");
var ModeIndicator = function (_a) {
    var mode = _a.mode;
    return ((0, preact_1.h)("div", null,
        "MODE: ",
        mode === 0 ? 'GRID' : 'MENU'));
};
exports.ModeIndicator = ModeIndicator;
