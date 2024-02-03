using BtpTweak.Pools;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BehemothTweak : TweakBase<BehemothTweak>, IOnModLoadBehavior {
        public const int Radius = 3;
        public const float BaseDamageCoefficient = 0.6f;
        public const float Interval = 0.1f;

        void IOnModLoadBehavior.OnModLoad() {
            BetterEvents.OnHitAll += BetterEvents_OnHitAll;
        }

        private void BetterEvents_OnHitAll(DamageInfo damageInfo, CharacterBody attackerBody) {
            int itemCount = attackerBody.inventory.GetItemCount(RoR2Content.Items.Behemoth);
            if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Behemoth)) {
                var attackInfo = new BehemothPoolKey {
                    crit = damageInfo.crit,
                    damageType = damageInfo.damageType,
                    procCoefficient = damageInfo.procCoefficient,
                    radius = Radius * itemCount,
                    attacker = damageInfo.attacker,
                    procChainMask = damageInfo.procChainMask,
                    teamIndex = attackerBody.teamComponent.teamIndex,
                };
                attackInfo.procChainMask.AddWGRYProcs();
                BehemothPool.RentPool(attackInfo.attacker).AddBlastAttack(attackInfo,
                                                                     damageInfo.position,
                                                                     Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient));
            }
        }
    }
}