using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class HealOnCritTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const int BaseCrit = 10;
        public const int StackCrit = 5;
        public const float HealFraction = 0.02f;

        void IModLoadMessageHandler.Handle() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            RoR2Content.Items.HealOnCrit.TryApplyTag(ItemTag.AIBlacklist);
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