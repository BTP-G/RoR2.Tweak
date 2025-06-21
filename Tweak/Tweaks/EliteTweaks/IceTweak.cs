using BTP.RoR2Plugin.Pools.OrbPools;
using BTP.RoR2Plugin.Utils;
using RoR2;
using TPDespair.ZetAspects;

namespace BTP.RoR2Plugin.Tweaks.EliteTweaks {

    internal class IceTweak : ModComponent, IModLoadMessageHandler {
        public const float Interval = 0.1f;

        void IModLoadMessageHandler.Handle() {
            BetterEvents.OnHitEnemy += BetterEvents_OnHitEnemy;
        }

        private void BetterEvents_OnHitEnemy(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) {
            if (attackerBody.TryGetAspectStackMagnitude(RoR2Content.Buffs.AffixWhite.buffIndex, out var stack) && victimBody.mainHurtBox) {
                var damageCoefficient = Configuration.AspectWhiteBaseDamage.Value + Configuration.AspectWhiteStackDamage.Value * (stack - 1f);
                if (attackerBody.teamComponent.teamIndex != TeamIndex.Player) {
                    damageCoefficient *= Configuration.AspectWhiteMonsterDamageMult.Value;
                }
                var simpleOrbInfo = new OrbPoolKey {
                    attackerBody = attackerBody,
                    isCrit = damageInfo.crit,
                    procChainMask = damageInfo.procChainMask,
                    target = victimBody.mainHurtBox,
                };
                FrostBladeOrbPool.RentPool(victimBody.gameObject).AddOrb(simpleOrbInfo,
                    Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient));
            }
        }
    }
}