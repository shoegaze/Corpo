"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TeamIndicator = void 0;
var preact_1 = require("preact");
var TeamIndicator = function (_a) {
    var team = _a.team;
    var baseClass = 'relative w-auto mx-auto px-4 text-white font-bold';
    return team === 0 ? ((0, preact_1.h)("div", { class: "".concat(baseClass, " bg-[#80DD83]") }, '>> ALLY <<')) : ((0, preact_1.h)("div", { class: "".concat(baseClass, " bg-[#EC4242]") }, '<< ENEMY >>'));
};
exports.TeamIndicator = TeamIndicator;
