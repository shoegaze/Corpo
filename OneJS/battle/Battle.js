"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var preact_1 = require("preact");
var Menu_1 = require("./Menu");
var Statusbar_1 = require("./Statusbar");
var Result_1 = require("./Result");
var Battle = function () {
    var battleUI = require('battle_ui');
    return ((0, preact_1.h)("div", { class: "absolute flex w-full h-full" },
        (0, preact_1.h)(Menu_1.Menu, null),
        (0, preact_1.h)(Statusbar_1.Statusbar, { battleUI: battleUI }),
        (0, preact_1.h)(Result_1.Result, null)));
};
(0, preact_1.render)((0, preact_1.h)(Battle, null), document.body);
