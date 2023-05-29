"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Menu = void 0;
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var preload_1 = require("preload");
var Menu = function (_a) {
    var battleUI = _a.battleUI;
    var _b = (0, onejs_1.useEventfulState)(battleUI, 'ActiveActor'), activeActor = _b[0], _setActiveActor = _b[1];
    return ((0, preact_1.h)("div", { class: 'absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px]', style: {
            unityFontDefinition: preload_1.font,
            backgroundColor: 'cyan'
        } },
        (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-2 text-5xl bg-teal-400' },
            (0, preact_1.h)("label", { text: "* ".concat(activeActor === null || activeActor === void 0 ? void 0 : activeActor.Name) }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "".concat(activeActor === null || activeActor === void 0 ? void 0 : activeActor.Health, " / ").concat(activeActor === null || activeActor === void 0 ? void 0 : activeActor.MaxHealth) })),
        (0, preact_1.h)("div", { class: 'flex flex-row px-6 text-4xl', style: {
                backgroundColor: 'skyblue'
            } },
            (0, preact_1.h)("label", { text: "Icon" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Name" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Cost" })),
        (0, preact_1.h)("div", { class: 'text-4xl' }, activeActor === null || activeActor === void 0 ? void 0 : activeActor.Abilities.map(function (ability, i) { return ((0, preact_1.h)("div", { class: 'flex flex-row px-6 py-3', style: {
                backgroundColor: i % 2 === 0 ? 'salmon' : 'violet'
            } },
            (0, preact_1.h)("image", { class: 'w-[64px] h-[64px]', sprite: ability.Icon }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: ability.Name }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: ability.Cost.toString() }))); }))));
};
exports.Menu = Menu;
