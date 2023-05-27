"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var App = function () {
    var game = require('game');
    var _a = (0, onejs_1.useEventfulState)(game, ''), mode = _a[0], _ = _a[1];
    switch (mode) {
        case 0:
            return ((0, preact_1.h)("div", null, "Hello, world!"));
        case 1:
            return ((0, preact_1.h)("div", null, "Hello, battle!"));
        default:
            return (0, preact_1.h)("div", null);
    }
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
