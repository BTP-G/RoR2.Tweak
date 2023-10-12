using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class EliteTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.CharacterBody.UpdateAffixPoison += CharacterBody_UpdateAffixPoison;
        }

        public override void Load() {
            base.Load();
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LightningStake").GetComponent<ProjectileImpactExplosion>().blastRadius = 10f;
        }

        private void CharacterBody_UpdateAffixPoison(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 6f))) {
                iLCursor.Remove();
                iLCursor.Emit(OpCodes.Ldc_R4, 3f);
                if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 0f))) {
                    iLCursor.Emit(OpCodes.Ldarg_0);
                    iLCursor.EmitDelegate((CharacterBody body) => {
                        if (NetworkServer.active) {
                            List<HealthComponent> list = new();
                            Collider[] array = Physics.OverlapSphere(body.corePosition, 20 * body.bestFitRadius, LayerIndex.entityPrecise.mask);
                            for (int i = 0; i < array.Length; ++i) {
                                HealthComponent healthComponent = array[i].GetComponent<HurtBox>()?.healthComponent;
                                if (!healthComponent || list.Contains(healthComponent)) {
                                    continue;
                                }
                                var victimBody = healthComponent.body;
                                if (!victimBody) {
                                    continue;
                                }
                                list.Add(healthComponent);
                                if (victimBody.teamComponent.teamIndex == body.teamComponent.teamIndex) {
                                    continue;
                                }
                                victimBody.AddTimedBuff(RoR2Content.Buffs.HealingDisabled, 4);
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