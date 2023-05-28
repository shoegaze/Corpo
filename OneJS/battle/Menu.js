"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Menu = void 0;
var preact_1 = require("preact");
var preload_1 = require("preload");
var Menu = function (_a) {
    var battleUI = _a.battleUI;
    return ((0, preact_1.h)("div", { class: 'absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px]', style: {
            unityFontDefinition: preload_1.font,
            backgroundColor: 'cyan'
        } },
        (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-2 text-5xl bg-teal-400' },
            (0, preact_1.h)("span", null, "* djimpp"),
            (0, preact_1.h)("span", { class: 'grow' }),
            (0, preact_1.h)("span", null, "4 / 4")),
        (0, preact_1.h)("div", { class: 'flex flex-row px-6 text-4xl', style: {
                backgroundColor: 'skyblue'
            } },
            (0, preact_1.h)("span", null, "Icon"),
            (0, preact_1.h)("span", { class: 'grow' }),
            (0, preact_1.h)("span", null, "Name"),
            (0, preact_1.h)("span", { class: 'grow' }),
            (0, preact_1.h)("span", null, "Cost")),
        (0, preact_1.h)("div", { class: 'text-4xl' },
            (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-3', style: {
                    backgroundColor: 'salmon'
                } },
                (0, preact_1.h)("span", { class: '' }, "%"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "Tackle"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "20")),
            (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-3', style: {
                    backgroundColor: 'violet'
                } },
                (0, preact_1.h)("span", { class: '' }, "!"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "Snap"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "30")),
            (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-3', style: {
                    backgroundColor: 'salmon'
                } },
                (0, preact_1.h)("span", { class: '' }, "#"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "Censor"),
                (0, preact_1.h)("span", { class: 'grow' }),
                (0, preact_1.h)("span", { class: '' }, "50")))));
};
exports.Menu = Menu;
