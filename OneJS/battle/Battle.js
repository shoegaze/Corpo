"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Battle = void 0;
var preact_1 = require("preact");
var Menu_1 = require("./Menu");
var Statusbar_1 = require("./Statusbar");
var Result_1 = require("./Result");
var Battle = function () {
    return ((0, preact_1.h)("div", { class: "w-full h-full", style: { backgroundColor: 'red' } },
        (0, preact_1.h)(Menu_1.Menu, null),
        (0, preact_1.h)(Statusbar_1.Statusbar, null),
        (0, preact_1.h)(Result_1.Result, null)));
};
exports.Battle = Battle;
