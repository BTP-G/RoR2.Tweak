using BTP.RoR2Plugin.Language;
using BTP.RoR2Plugin.Utils;
using GuestUnion;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Difficulities {

    internal class HarryRoadGreatWhirlwind : ModComponent, IModLoadMessageHandler {
        public static DifficultyDef DifficultyDef { get; private set; }
        public static DifficultyIndex DifficultyIndex { get; private set; }

        void IModLoadMessageHandler.Handle() {
            DifficultyDef = new DifficultyDef(3f,
                                   "DIFFICULTY_great_whirlwind_NAME",
                                   null,
                                   "DIFFICULTY_great_whirlwind_DESC",
                                   ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin),
                                   "btp",
                                   true);
            DifficultyIndex = DifficultyAPI.AddDifficulty(DifficultyDef, Texture2DPaths.texVoidCoinIcon.Load<Sprite>());
            if (DifficultyIndex == DifficultyIndex.Invalid) {
                $"Difficulties {DifficultyDef.nameToken} add failed!".LogError();
            }
            "DIFFICULTY_great_whirlwind_NAME".AddOverlay("哈里路大旋风");
            "DIFFICULTY_great_whirlwind_DESC".AddOverlay("准备好面对哈里路大旋风了吗？\n\n".ToGold().ToStringBuilder()
                .Append("以标准的季风难度开局，但难度随情况变化。\n\n")
                .Append(">玩家根据所选角色获得不同初始物品\n".ToHealing())
                .Append(">每一关每种可交互物品将至少生成1个\n".ToHealing())
                .Append(">敌人将会根据情况获得各种增益\n".ToDeath())
                .Append(">充能半径将与充能进度成反比\n".ToDeath())
                .Append(">未充能时进度缓慢衰减\n".ToDeath()).ToStringAndReturn());
        }
    }
}