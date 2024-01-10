using BtpTweak.Messages;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using R2API.Networking;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class Bandit2Tweak : TweakBase<Bandit2Tweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float ResetRevolverDamageCoefficient = 12f;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += FireSidearmSkullRevolver_ModifyBullet;
            EntityStateConfigurationPaths.EntityStatesBandit2WeaponFireSidearmResetRevolver.Load<EntityStateConfiguration>().Set("damageCoefficient", ResetRevolverDamageCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            EntityStates.Bandit2.Weapon.EnterReload.baseDuration *= 0.5f;
            EntityStates.Bandit2.Weapon.Reload.baseDuration *= 0.5f;
        }

        private void FireSidearmSkullRevolver_ModifyBullet(On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver self, BulletAttack bulletAttack) {
            orig(self, bulletAttack);
            var body = self.characterBody;
            var inventory = body.inventory;
            var buffCount = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
            buffCount -= buffCount / (5 * (int)body.level);
            body.ApplyBuff(RoR2Content.Buffs.BanditSkull.buffIndex, buffCount);
            inventory.GiveItemAuthority(JunkContent.Items.SkullCounter.itemIndex, buffCount - inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
        }
    }
}