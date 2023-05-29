"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Menu = void 0;
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var preload_1 = require("preload");
var Menu = function (_a) {
    var _b, _c, _d, _e;
    var battleUI = _a.battleUI;
    var _f = (0, onejs_1.useEventfulState)(battleUI, 'ActiveActor'), activeActor = _f[0], _setActiveActor = _f[1];
    return ((0, preact_1.h)("div", { class: 'absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px] bg-slate-700', style: {
            unityFontDefinition: preload_1.font
        } },
        (0, preact_1.h)("div", { class: 'flex flex-row px-6 py-2 text-5xl bg-cyan-500' },
            (0, preact_1.h)("label", { text: '*', class: 'mr-6' }),
            (0, preact_1.h)("label", { text: (_b = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Name) !== null && _b !== void 0 ? _b : '' }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "".concat((_c = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Health) !== null && _c !== void 0 ? _c : 0, " / ").concat((_d = activeActor === null || activeActor === void 0 ? void 0 : activeActor.MaxHealth) !== null && _d !== void 0 ? _d : 0) })),
        (0, preact_1.h)("div", { class: 'flex flex-row mb-2 px-6 text-4xl text-slate-200 bg-cyan-700' },
            (0, preact_1.h)("label", { text: "Icon" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Name" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Cost" })),
        (0, preact_1.h)("div", { class: 'text-4xl' }, (_e = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Abilities.map(function (ability, i) { return ((0, preact_1.h)("div", { class: 'flex flex-row px-6 py-3', style: {
                backgroundColor: i % 2 === 0 ? '#7DAFBD' : '#51778A'
            } },
            (0, preact_1.h)("image", { sprite: ability.Icon, class: 'w-[64px] h-[64px] bg-slate-700' }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: ability.Name }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: ability.Cost.toString() }))); })) !== null && _e !== void 0 ? _e : '')));
};
exports.Menu = Menu;
