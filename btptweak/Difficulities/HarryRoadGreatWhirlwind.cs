using BTP.RoR2Plugin.Language;
using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Difficulities {

    internal class HarryRoadGreatWhirlwind : TweakBase<HarryRoadGreatWhirlwind>, IOnModLoadBehavior {
        public static DifficultyDef 造物 { get; private set; }
        public static DifficultyIndex 造物索引 { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            造物 = new DifficultyDef(3f,
                                   "DIFFICULTY_great_whirlwind_NAME",
                                   null,
                                   "DIFFICULTY_great_whirlwind_DESC",
                                   ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin),
                                   "btp",
                                   true);
            造物索引 = DifficultyAPI.AddDifficulty(造物, Texture2DPaths.texVoidCoinIcon.Load<Sprite>());
            if (造物索引 == DifficultyIndex.Invalid) {
                $"Difficulties {造物.nameToken} add failed!".LogError();
            }
            Localizer.AddOverlay("DIFFICULTY_great_whirlwind_NAME", "哈里路大旋风");
            Localizer.AddOverlay("DIFFICULTY_great_whirlwind_DESC", "准备好面对哈里路大旋风了吗？\n\n".ToRainbowWavy()
                + "以标准的季风难度开局，但难度随情况变化。\n\n".ToShaky()
                + ">玩家根据所选角色获得初始物品\n".ToHealing()
                + ">敌人将会根据情况获得各种增益\n".ToDeath()
                + ">充能半径将与充能进度成反比\n".ToDeath()
                + ">未充能时进度缓慢衰减\n".ToDeath());
        }
    }
}