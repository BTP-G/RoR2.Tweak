using RoR2;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class Bandit2Tweak : TweakBase<Bandit2Tweak>{

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.ModifyBullet += FireSidearmResetRevolver_ModifyBullet;
            On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += BaseFireSidearmRevolverState_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.ModifyBullet -= FireSidearmResetRevolver_ModifyBullet;
            On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter -= BaseFireSidearmRevolverState_OnEnter;
        }

        public void Load() {
            EntityStates.Bandit2.Weapon.EnterReload.baseDuration *= 0.5f;
            EntityStates.Bandit2.Weapon.Reload.baseDuration *= 0.5f;
        }

        private void BaseFireSidearmRevolverState_OnEnter(On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.orig_OnEnter orig, EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState self) {
            orig(self);
            if (NetworkServer.active && self is EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver) {
                var body = self.characterBody;
                var inventory = body.inventory;
                int buffCount = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                buffCount -= buffCount / (5 * (int)body.level);
                body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, buffCount);
                inventory.GiveItem(JunkContent.Items.SkullCounter.itemIndex, buffCount - inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
            }
        }

        private void FireSidearmResetRevolver_ModifyBullet(On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmResetRevolver self, BulletAttack bulletAttack) {
            orig(self, bulletAttack);
            bulletAttack.damage *= 2f;
        }
    }
}