using BtpTweak.Utils;
using EntityStates.Merc;
using RoR2;
using RoR2.Skills;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class MercTweak : TweakBase<MercTweak>{

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.Merc.Evis.OnEnter += Evis_OnEnter;
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += GroundLight2_OnEnter;
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += ThrowEvisProjectile_OnEnter;
            On.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.Merc.Evis.OnEnter -= Evis_OnEnter;
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter -= GroundLight2_OnEnter;
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter -= ThrowEvisProjectile_OnEnter;
            On.EntityStates.Merc.WhirlwindBase.OnEnter -= WhirlwindBase_OnEnter;
        }

        public void Load() {
            "RoR2/Base/Merc/MercBodyEvis.asset".Load<SkillDef>().keywordTokens = new string[] { "KEYWORD_FLEETING" };
            "RoR2/Base/Merc/MercBodyEvisProjectile.asset".Load<SkillDef>().keywordTokens = new string[] { "KEYWORD_FLEETING" };
        }

        private void Evis_OnEnter(On.EntityStates.Merc.Evis.orig_OnEnter orig, EntityStates.Merc.Evis self) {
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

        private void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, EntityStates.Merc.WhirlwindBase self) {
            orig(self);
            self.moveSpeedStat = 7f;
        }
    }
}