"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Battle_1 = require("battle/Battle");
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var App = function () {
    var game = require('game');
    var _a = (0, onejs_1.useEventfulState)(game, 'Mode'), mode = _a[0], _ = _a[1];
    switch (mode) {
        case 0:
            return ((0, preact_1.h)("div", { class: 'w-full h-full' }, "Hello, world!"));
        case 1:
            return (0, preact_1.h)(Battle_1.Battle, null);
        default:
            return (0, preact_1.h)("div", null);
    }
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
