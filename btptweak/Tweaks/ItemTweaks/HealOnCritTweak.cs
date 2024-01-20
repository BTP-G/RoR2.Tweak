using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class HealOnCritTweak : TweakBase<HealOnCritTweak>, IOnModLoadBehavior {
        public const int BaseCrit = 10;
        public const int StackCrit = 5;
        public const float HealFraction = 0.02f;

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var attackerBody = damageReport.attackerBody;
            if (!attackerBody || !attackerBody.inventory) {
                return;
            }
            var itemCount = attackerBody.inventory.GetItemCount(RoR2Content.Items.HealOnCrit.itemIndex);
            var procChainMask = damageReport.damageInfo.procChainMask;
            if (itemCount > 0 && !procChainMask.HasProc(ProcType.HealOnCrit)) {
                procChainMask.AddProc(ProcType.HealOnCrit);
                attackerBody.healthComponent.HealFraction(HealFraction * itemCount * (damageReport.damageInfo.crit ? attackerBody.critMultiplier : 1f), procChainMask);
                Util.PlaySound("Play_item_proc_crit_heal", attackerBody.gameObject);
            }
        }
    }
}