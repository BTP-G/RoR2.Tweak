using RoR2;

namespace BtpTweak {

    internal class CombatHook {
        public static int 当前敌人生成数_ = 0;
        public static int 敌人最大生成数_ = 0;
        public static EliteDef 特殊环境精英属性_;
        public static short 精英转化几率 = 0;

        public static void AddHook() {
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate;
            On.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
        }

        public static void RemoveHook() {
            On.RoR2.CombatDirector.Simulate -= CombatDirector_Simulate;
            On.RoR2.CombatDirector.Spawn -= CombatDirector_Spawn;
        }

        public static void LateInit() {
            foreach (CombatDirector.EliteTierDef eliteTierDef in CombatDirector.eliteTiers) {
                if (eliteTierDef.costMultiplier == 36f) {
                    eliteTierDef.eliteTypes = EliteCatalog.eliteDefs;
                    eliteTierDef.costMultiplier = 18f;
                }
            }
        }

        private static void CombatDirector_Simulate(On.RoR2.CombatDirector.orig_Simulate orig, CombatDirector self, float deltaTime) {
            if (当前敌人生成数_ > 敌人最大生成数_ * (TeleporterInteraction.instance?.activationState == TeleporterInteraction.ActivationState.Charging ? 1.5f : 1)) {
                当前敌人生成数_ = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
                return;
            }
            orig(self, deltaTime);
        }

        private static bool CombatDirector_Spawn(On.RoR2.CombatDirector.orig_Spawn orig, CombatDirector self, SpawnCard spawnCard, EliteDef eliteDef, UnityEngine.Transform spawnTarget, DirectorCore.MonsterSpawnDistance spawnDistance, bool preventOverhead, float valueMultiplier, DirectorPlacementRule.PlacementMode placementMode) {
            当前敌人生成数_ = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
            if (eliteDef) {
                if (特殊环境精英属性_ && Util.CheckRoll(精英转化几率)) {
                    eliteDef = 特殊环境精英属性_;
                }
            }
            return orig(self, spawnCard, eliteDef, spawnTarget, spawnDistance, preventOverhead, valueMultiplier, placementMode);
        }
    }
}