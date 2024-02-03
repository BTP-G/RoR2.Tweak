using BtpTweak.Pools.ProjectilePools;
using BtpTweak.Utils;
using RoR2;
using TPDespair.ZetAspects;

namespace BtpTweak.Tweaks.EliteTweaks {

    internal class LightningTweak : TweakBase<LightningTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            BetterEvents.OnHitAll += BetterEvents_OnHitAll;
        }

        private void BetterEvents_OnHitAll(DamageInfo damageInfo, CharacterBody attackerBody) {
            if (attackerBody.TryGetAspectStackMagnitude(RoR2Content.Buffs.AffixBlue.buffIndex, out var stack)) {
                var damageCoefficient = Configuration.AspectBlueBaseDamage.Value + Configuration.AspectBlueStackDamage.Value * (stack - 1f);
                if (attackerBody.teamComponent.teamIndex != TeamIndex.Player) {
                    damageCoefficient *= Configuration.AspectBlueMonsterDamageMult.Value;
                }
                LightningStakePool.RentPool(damageInfo.attacker).AddProjectile(
                    new() { attacker = damageInfo.attacker, isCrit = damageInfo.crit, procChainMask = damageInfo.procChainMask },
                    damageInfo.position,
                    Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient));
            }
        }
    }
}