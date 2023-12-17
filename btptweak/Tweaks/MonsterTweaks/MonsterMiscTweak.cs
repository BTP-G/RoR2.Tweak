using RoR2;

namespace BtpTweak.Tweaks.MonsterTweaks {

    internal class MonsterMiscTweak : TweakBase<MonsterMiscTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            EntityStates.BrotherMonster.FistSlam.healthCostFraction = 0;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (damageReport.victimIsBoss && damageReport.damageDealt > damageReport.victim.fullCombinedHealth * 0.2f) {
                damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10f);
                damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.TeamWarCry, 10f);
            }
        }
    }
}