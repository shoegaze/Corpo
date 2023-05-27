"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TeamIndicator = void 0;
var preact_1 = require("preact");
var TeamIndicator = function (_a) {
    var team = _a.team;
    var baseClass = 'w-auto mx-auto px-4 text-white font-bold';
    return team === 0 ? ((0, preact_1.h)("div", { class: "".concat(baseClass, " bg-lime-200") }, '>> ALLY <<')) : ((0, preact_1.h)("div", { class: "".concat(baseClass, " bg-rose-400") }, '<< ENEMY >>'));
};
exports.TeamIndicator = TeamIndicator;
