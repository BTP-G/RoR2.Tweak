using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class HealOnCritTweak : TweakBase<HealOnCritTweak>, IOnModLoadBehavior {
        public const float RegenDuration = 0.1f;

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            if (!damageReport.damageInfo.crit) {
                return;
            }
            var attackerBody = damageReport.attackerBody;
            if (!attackerBody) {
                return;
            }
            var itemCount = attackerBody.inventory?.GetItemCount(RoR2Content.Items.HealOnCrit.itemIndex) ?? 0;
            if (itemCount > 0) {
                attackerBody.AddTimedBuff(RoR2Content.Buffs.CrocoRegen, RegenDuration * itemCount);
            }
        }
    }
}