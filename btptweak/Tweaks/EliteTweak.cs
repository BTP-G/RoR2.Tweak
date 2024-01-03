using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class EliteTweak : TweakBase<EliteTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private readonly List<HealthComponent> SearchedObjects = [];

        private readonly BullseyeSearch search = new() {
            filterByDistinctEntity = true,
            filterByLoS = false,
            searchDirection = Vector3.zero,
            sortMode = BullseyeSearch.SortMode.Distance,
        };

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.CharacterBody.UpdateAffixPoison += CharacterBody_UpdateAffixPoison;
            Stage.onStageStartGlobal += AspectsDropCountReset;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.LightningStake.LoadComponent<ProjectileImpactExplosion>().blastRadius = 10f;
        }

        private void AspectsDropCountReset(Stage stage) {
            TPDespair.ZetAspects.DropHooks.runDropCount = 0;
        }

        private void CharacterBody_UpdateAffixPoison(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 6f))) {
                cursor.Remove();
                cursor.Emit(OpCodes.Ldc_R4, 3f);
                if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 0f))) {
                    cursor.Emit(OpCodes.Ldarg_0);
                    cursor.EmitDelegate((CharacterBody body) => {
                        if (NetworkServer.active) {
                            search.searchOrigin = body.corePosition;
                            search.teamMaskFilter = TeamMask.allButNeutral;
                            search.teamMaskFilter.RemoveTeam(body.teamComponent.teamIndex);
                            search.maxDistanceFilter = body.bestFitRadius * 10f;
                            search.RefreshCandidates();
                            SearchedObjects.Clear();
                            foreach (var hurtBox in search.GetResults()) {
                                if (SearchedObjects.Contains(hurtBox.healthComponent)) {
                                    continue;
                                }
                                SearchedObjects.Add(hurtBox.healthComponent);
                                hurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, 4f);
                            }
                        }
                    });
                } else {
                    Main.Logger.LogError("AffixPoison Hook2 Failed!");
                }
            } else {
                Main.Logger.LogError("AffixPoison Hook1 Failed!");
            }
        }
    }
}