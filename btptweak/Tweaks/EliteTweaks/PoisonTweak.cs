using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.EliteTweaks {

    internal class PoisonTweak : TweakBase<PoisonTweak>, IOnModLoadBehavior {
        private readonly List<HealthComponent> SearchedObjects = [];

        private readonly BullseyeSearch search = new() {
            filterByDistinctEntity = true,
            filterByLoS = false,
            searchDirection = Vector3.zero,
            sortMode = BullseyeSearch.SortMode.Distance,
        };

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.CharacterBody.UpdateAffixPoison += CharacterBody_UpdateAffixPoison;
        }

        private void CharacterBody_UpdateAffixPoison(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(x => x.MatchLdcR4(6f))) {
                cursor.Remove();
                cursor.Emit(OpCodes.Ldc_R4, 3f);
                if (cursor.TryGotoNext(x => x.MatchLdcR4(0f))) {
                    cursor.Emit(OpCodes.Ldarg_0);
                    cursor.EmitDelegate((CharacterBody body) => {
                        if (NetworkServer.active) {
                            search.searchOrigin = body.corePosition;
                            search.teamMaskFilter = TeamMask.GetEnemyTeams(body.teamComponent.teamIndex);
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
                    "AffixPoison Hook2 Failed!".LogError();
                }
            } else {
                "AffixPoison Hook1 Failed!".LogError();
            }
        }
    }
}