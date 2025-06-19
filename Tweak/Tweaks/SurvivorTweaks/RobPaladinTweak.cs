using BTP.RoR2Plugin.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class RobPaladinTweak : TweakBase<RobPaladinTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.GrandParentSunController.Start += GrandParentSunController_Start;
            IL.RoR2.GrandParentSunController.ServerFixedUpdate += GrandParentSunController_ServerFixedUpdate;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.RobPaladin, RecalculateRobPaladinStats);
        }

        private void GrandParentSunController_Start(On.RoR2.GrandParentSunController.orig_Start orig, GrandParentSunController self) {
            orig(self);
            if (self.ownership.ownerObject && self.ownership.ownerObject.TryGetComponent<CharacterBody>(out var ownerBody)) {
                self.cycleInterval /= 1 + ownerBody.inventory.GetItemCount(RoR2Content.Items.ParentEgg.itemIndex);
            }
        }

        private void GrandParentSunController_ServerFixedUpdate(ILContext il) {
            var cursor = new ILCursor(il);
            cursor.GotoNext(i => i.MatchStloc(10));
            cursor.Emit(OpCodes.Ldarg_0).Emit(OpCodes.Ldloc, 4).EmitDelegate((int buffCount, GrandParentSunController sun, CharacterBody victimBody) => {
                if (buffCount > 0 && sun.ownership.ownerObject && sun.ownership.ownerObject.TryGetComponent<CharacterBody>(out var attackerBody)) {
                    var inflictDotInfo = new InflictDotInfo {
                        attackerObject = attackerBody.gameObject,
                        damageMultiplier = 1f,
                        dotIndex = DotController.DotIndex.Burn,
                        totalDamage = attackerBody.damage * sun.burnDuration * buffCount,
                        victimObject = victimBody.gameObject,
                    };
                    if (attackerBody.inventory) {
                        var boostCoefficient = 1 + attackerBody.inventory.GetItemCount(RoR2Content.Items.ParentEgg.itemIndex);
                        inflictDotInfo.damageMultiplier *= boostCoefficient;
                        inflictDotInfo.totalDamage *= boostCoefficient;
                        inflictDotInfo.TryUpgrade(attackerBody.inventory, victimBody);
                    }
                    DotController.InflictDot(ref inflictDotInfo);
                }
            });
            cursor.Emit(OpCodes.Ldc_I4_0);
        }

        private void RecalculateRobPaladinStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.baseDamageAdd += 3 * inventory.GetItemCount(vanillaVoid.Items.ExeBlade.instance.ItemDef);
        }
    }
}