using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class CommandoTweak : TweakBase<CommandoTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float FMJDamageCoefficient = 4f;
        public const float ThrowGrenadeDamageCoefficient = 12f;

        void IOnModLoadBehavior.OnModLoad() {
            EntityStateConfigurationPaths.EntityStatesCommandoCommandoWeaponFireFMJ.Load<EntityStateConfiguration>().Set("damageCoefficient", FMJDamageCoefficient.ToString());
            EntityStateConfigurationPaths.EntityStatesCommandoCommandoWeaponThrowGrenade.Load<EntityStateConfiguration>().Set("damageCoefficient", ThrowGrenadeDamageCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            EntityStates.Commando.CommandoWeapon.FireBarrage.baseBulletCount = 12;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Commando, RecalculateCommandoStats);
        }

        private void RecalculateCommandoStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            var statUpPercent = 0.03f * inventory.GetItemCount(RoR2Content.Items.Syringe.itemIndex);
            args.attackSpeedMultAdd += statUpPercent;
            args.moveSpeedMultAdd += statUpPercent;
            args.healthMultAdd += statUpPercent;
            args.regenMultAdd += statUpPercent;
            args.baseDamageAdd += 2 * inventory.GetItemCount(RoR2Content.Items.BossDamageBonus.itemIndex);
        }
    }
}