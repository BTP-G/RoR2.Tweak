using BTP.RoR2Plugin.Messages;
using BTP.RoR2Plugin.Utils;
using R2API.Networking;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class Bandit2Tweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float ResetRevolverDamageCoefficient = 12f;

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += FireSidearmSkullRevolver_ModifyBullet;
            EntityStateConfigurationPaths.EntityStatesBandit2WeaponFireSidearmResetRevolver.Load<EntityStateConfiguration>().Set("damageCoefficient", ResetRevolverDamageCoefficient.ToString());
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            EntityStates.Bandit2.Weapon.EnterReload.baseDuration *= 0.5f;
            EntityStates.Bandit2.Weapon.Reload.baseDuration *= 0.5f;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            if ((damageReport.damageInfo.damageType.damageType & DamageType.GiveSkullOnKill) == DamageType.GiveSkullOnKill) {
                var count = (damageReport.victimIsElite ? 2 : 0)
                          + (damageReport.victimIsChampion ? 4 : 0)
                          + (damageReport.victimIsBoss ? 5 : 0);
                if (count > 0) {
                    count += damageReport.attackerBody.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                    damageReport.attackerBody.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, count);
                }
            }
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