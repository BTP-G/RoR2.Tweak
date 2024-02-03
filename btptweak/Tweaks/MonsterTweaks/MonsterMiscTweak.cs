using RoR2;

namespace BtpTweak.Tweaks.MonsterTweaks {

    internal class MonsterMiscTweak : TweakBase<MonsterMiscTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (damageReport.victimIsBoss && damageReport.damageDealt > damageReport.victim.fullCombinedHealth * 0.2f) {
                damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10f);
                damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.TeamWarCry, 10f);
            }
            if (RunInfo.已选择造物难度 && damageReport.hitLowHealth && damageReport.victim.alive && damageReport.victimTeamIndex != TeamIndex.Player) {
                var victimBody = damageReport.victimBody;
                if (!victimBody.inventory) {
                    return;
                }
                if (damageReport.victimTeamIndex == TeamIndex.Monster
                    && victimBody.inventory.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) {
                    victimBody.AddTimedBuff(RoR2Content.Buffs.TonicBuff, victimBody.isBoss ? 40f : 20f);
                    victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                    Util.CleanseBody(victimBody, true, false, false, false, true, false);
                } else if (damageReport.victimTeamIndex == TeamIndex.Void
                    && !victimBody.HasBuff(DLC1Content.Buffs.VoidSurvivorCorruptMode.buffIndex)) {
                    victimBody.AddBuff(DLC1Content.Buffs.VoidSurvivorCorruptMode.buffIndex);
                    victimBody.AddBuff(DLC1Content.Buffs.KillMoveSpeed.buffIndex);
                }
            }
        }
    }
}