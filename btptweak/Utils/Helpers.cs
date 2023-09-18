using RoR2;
using UnityEngine;
using RoR2.Skills;

namespace BtpTweak.Utils {

    public static class Helpers {

        public static void LogInfo(this object loginfo) => Main.logger_.LogInfo(loginfo);

        public static float GetExpAdjustedDropChancePercent(float baseChancePercent, GameObject victim) {
            DeathRewards component = victim.GetComponent<DeathRewards>();
            if (component) {
                return baseChancePercent * Mathf.Log(component.spawnValue, 2f + Run.instance.loopClearCount);
            } else {
                return 0;
            }
        }

        public static GameObject FindProjectilePrefab(string projectileName) => ProjectileCatalog.GetProjectilePrefab(ProjectileCatalog.FindProjectileIndex(projectileName));

        public static SkillDef FindSkillDef(string skillName) => SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName(skillName));

        public static bool HasItem(this Inventory inventory, ItemIndex itemIndex) => inventory.GetItemCount(itemIndex) > 0;
    }
}