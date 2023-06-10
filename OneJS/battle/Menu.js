"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Menu = void 0;
var UIElements_1 = require("UnityEngine/UIElements");
var onejs_1 = require("onejs");
var preact_1 = require("preact");
var hooks_1 = require("preact/hooks");
var preload_1 = require("preload");
var Menu = function (_a) {
    var _b, _c, _d, _e, _f, _g, _h;
    var battleView = _a.battleView;
    var _j = (0, onejs_1.useEventfulState)(battleView, 'PanelState'), panelState = _j[0], _setPanelState = _j[1];
    var _k = (0, onejs_1.useEventfulState)(battleView, 'FocusState'), focusState = _k[0], _setFocusState = _k[1];
    var _l = (0, onejs_1.useEventfulState)(battleView, 'ActiveActor'), activeActor = _l[0], _setActiveActor = _l[1];
    var _m = (0, onejs_1.useEventfulState)(battleView, 'AbilityIndex'), abilityIndex = _m[0], _setAbilityIndex = _m[1];
    var _o = (0, hooks_1.useState)(null), hoveredAbility = _o[0], setHoveredAbility = _o[1];
    return ((0, preact_1.h)("div", { class: 'menu-container absolute top-[10px] bottom-[50px] right-0 w-[890px] m-[8px] bg-slate-700', style: {
            unityFontDefinition: preload_1.font,
            borderColor: 'white',
            borderTopWidth: panelState === 0 ? 0 : 6,
            borderBottomWidth: panelState === 0 ? 0 : 6
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
        hoveredAbility ? ((0, preact_1.h)("div", { class: 'ability-desc relative bottom w-full' },
            (0, preact_1.h)("div", { class: 'ability-desc-title flex flex-row text-5xl bg-slate-400' },
                (0, preact_1.h)("label", { text: (_f = hoveredAbility.Name) !== null && _f !== void 0 ? _f : 'null' }),
                (0, preact_1.h)("div", { class: 'grow' }),
                (0, preact_1.h)("label", { text: "$".concat((_g = hoveredAbility.Cost) !== null && _g !== void 0 ? _g : 'NaN') })),
            (0, preact_1.h)("scrollview", { class: 'ability-desc-text h-64 text-4xl bg-slate-200', "vertical-scroller-visibility": UIElements_1.ScrollerVisibility.Auto },
                (0, preact_1.h)("label", { text: (_h = hoveredAbility.Description) !== null && _h !== void 0 ? _h : 'no desc.' })))) : ''));
};
exports.Menu = Menu;
