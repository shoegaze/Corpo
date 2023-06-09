"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Menu = void 0;
var UIElements_1 = require("UnityEngine/UIElements");
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var hooks_1 = require("preact/hooks");
var preload_1 = require("preload");
var Menu = function (_a) {
    var _b, _c, _d, _e, _f;
    var battleUI = _a.battleUI;
    var _g = (0, onejs_1.useEventfulState)(battleUI, 'PanelState'), panelState = _g[0], _setPanelState = _g[1];
    var _h = (0, onejs_1.useEventfulState)(battleUI, 'FocusState'), focusState = _h[0], _setFocusState = _h[1];
    var _j = (0, onejs_1.useEventfulState)(battleUI, 'ActiveActor'), activeActor = _j[0], _setActiveActor = _j[1];
    var _k = (0, onejs_1.useEventfulState)(battleUI, 'AbilityIndex'), abilityIndex = _k[0], _setAbilityIndex = _k[1];
    var _l = (0, hooks_1.useState)(null), hoveredAbility = _l[0], setHoveredAbility = _l[1];
    return ((0, preact_1.h)("div", { class: 'menu-container absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px] bg-slate-700', style: {
            unityFontDefinition: preload_1.font,
            borderColor: 'white',
            borderWidth: panelState === 0 ? 0 : 6
        } },
        (0, preact_1.h)("div", { class: 'actor-info flex flex-row px-6 py-2 text-5xl bg-cyan-500', style: {
                backgroundColor: panelState === 0 ? '#A9D4DE' : '#06B6DD'
            } },
            (0, preact_1.h)("label", { text: '*', class: 'mr-6' }),
            (0, preact_1.h)("label", { text: (_b = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Name) !== null && _b !== void 0 ? _b : '' }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "".concat((_c = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Health) !== null && _c !== void 0 ? _c : 0, " / ").concat((_d = activeActor === null || activeActor === void 0 ? void 0 : activeActor.MaxHealth) !== null && _d !== void 0 ? _d : 0) })),
        (0, preact_1.h)("div", { class: 'ability-labels flex flex-row mb-2 px-6 text-4xl text-slate-200 bg-cyan-700' },
            (0, preact_1.h)("label", { text: "Icon" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Name" }),
            (0, preact_1.h)("div", { class: 'grow' }),
            (0, preact_1.h)("label", { text: "Cost" })),
        (0, preact_1.h)("scrollview", { class: 'abilities-container grow text-4xl', "vertical-scroller-visibility": UIElements_1.ScrollerVisibility.Auto }, (_e = activeActor === null || activeActor === void 0 ? void 0 : activeActor.Abilities.map(function (ability, i) {
            var isEvenEntry = i % 2 === 0;
            var isMenuFocused = panelState === 1;
            var isAbilityFocused = focusState === 1 || focusState === 2;
            var isHovered = i === abilityIndex;
            if (isHovered) {
                setHoveredAbility(ability);
            }
            return ((0, preact_1.h)("div", { class: 'ability-entry flex flex-row px-6 py-3', key: i.toString(), style: {
                    backgroundColor: isEvenEntry ? '#7DAFBD' : '#51778A',
                    borderColor: 'white',
                    marginLeft: isHovered && isAbilityFocused ? -16 : 0,
                    marginRight: isHovered && isAbilityFocused ? 16 : 0
                } },
                (0, preact_1.h)("image", { sprite: ability.Icon, class: 'w-[64px] h-[64px] bg-slate-700' }),
                (0, preact_1.h)("div", { class: 'grow' }),
                (0, preact_1.h)("label", { text: ability.Name }),
                (0, preact_1.h)("div", { class: 'grow' }),
                (0, preact_1.h)("label", { text: ability.Cost.toString() })));
        })) !== null && _e !== void 0 ? _e : ''),
        hoveredAbility ? ((0, preact_1.h)("div", { class: 'ability-desc relative bottom w-full bg-slate-100' },
            (0, preact_1.h)("div", { class: 'ability-desc-title text-5xl flex flex-row' },
                (0, preact_1.h)("label", { text: hoveredAbility.Name }),
                (0, preact_1.h)("div", { class: 'grow' }),
                (0, preact_1.h)("label", { text: hoveredAbility.Cost.toString() })),
            (0, preact_1.h)("div", { class: 'ability-desc-text h-64 text-4xl' },
                (0, preact_1.h)("label", { text: (_f = hoveredAbility.Description) !== null && _f !== void 0 ? _f : 'no desc.' })))) : ''));
};
exports.Menu = Menu;
