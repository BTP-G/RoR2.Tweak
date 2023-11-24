using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class SeedTweak : TweakBase<SeedTweak> {
        public const float Leech = 0.01f;
        public const float 指数 = 0.5f;

        public override void SetEventHandlers() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        public override void ClearEventHandlers() {
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            var attackerBody = damageReport.attackerBody;
            if (!attackerBody) {
                return;
            }
            var itemCount = attackerBody.inventory?.GetItemCount(RoR2Content.Items.Seed.itemIndex) ?? 0;
            if (itemCount > 0) {
                attackerBody.healthComponent.Heal(Mathf.Sqrt(damageReport.damageDealt * Leech * itemCount + 1), damageReport.damageInfo.procChainMask, true);
            }
        }

        
    }
}