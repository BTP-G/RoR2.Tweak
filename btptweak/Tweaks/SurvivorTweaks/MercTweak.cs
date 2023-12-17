using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.Merc;
using HG;
using RoR2.Skills;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class MercTweak : TweakBase<MercTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Merc.Evis.OnEnter += Evis_OnEnter;
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += GroundLight2_OnEnter;
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += ThrowEvisProjectile_OnEnter;
            On.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyEvis.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyEvisProjectile.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
        }

        private void Evis_OnEnter(On.EntityStates.Merc.Evis.orig_OnEnter orig, Evis self) {
            orig(self);
            Evis.damageCoefficient *= self.attackSpeedStat;
        }

        private void GroundLight2_OnEnter(On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, EntityStates.Merc.Weapon.GroundLight2 self) {
            self.ignoreAttackSpeed = true;
            orig(self);
        }

        private void ThrowEvisProjectile_OnEnter(On.EntityStates.Merc.Weapon.ThrowEvisProjectile.orig_OnEnter orig, EntityStates.Merc.Weapon.ThrowEvisProjectile self) {
            orig(self);
            self.damageCoefficient *= self.attackSpeedStat;
        }

        private void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, WhirlwindBase self) {
            orig(self);
            self.moveSpeedStat = 7f;
        }
    }
}