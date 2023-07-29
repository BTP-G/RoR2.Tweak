using RoR2;

namespace BtpTweak {

    internal class CombatHook {
        public static int 当前敌人生成数 = 0;
        public static int 敌人最大生成数 = 0;

        public static void AddHook() {
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate;
            On.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
        }

        private static void CombatDirector_Simulate(On.RoR2.CombatDirector.orig_Simulate orig, CombatDirector self, float deltaTime) {
            if (当前敌人生成数 > 敌人最大生成数 * (TeleporterInteraction.instance?.activationState == TeleporterInteraction.ActivationState.Charging ? 1.5f : 1)) {
                当前敌人生成数 = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
                return;
            }
            orig(self, deltaTime);
        }

        private static bool CombatDirector_Spawn(On.RoR2.CombatDirector.orig_Spawn orig, CombatDirector self, SpawnCard spawnCard, EliteDef eliteDef, UnityEngine.Transform spawnTarget, DirectorCore.MonsterSpawnDistance spawnDistance, bool preventOverhead, float valueMultiplier, DirectorPlacementRule.PlacementMode placementMode) {
            当前敌人生成数 = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
            return orig(self, spawnCard, eliteDef, spawnTarget, spawnDistance, preventOverhead, valueMultiplier, placementMode);
        }
    }
}