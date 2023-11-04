using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExplodeOnDeathVoidTweak : TweakBase<ExplodeOnDeathVoidTweak> {
        public const float BaseDamageCoefficient = 0.5f;
        public const float BaseRadius = 10f;

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("ExplodeOnDeathVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {  //378
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.EmitDelegate((int itemCount, HealthComponent healthComponent, DamageInfo damageInfo, CharacterBody attacterBody) => {
                    if (itemCount > 0) {
                        CharacterBody victimBody = healthComponent.body;
                        if (victimBody.HasBuff(Main.VoidFire)) {
                            return;
                        }
                        float combinedHealth = healthComponent.combinedHealth;
                        float fullCombinedHealth = healthComponent.fullCombinedHealth;
                        float combinedHealthFraction = fullCombinedHealth < combinedHealth ? 0 : combinedHealth / fullCombinedHealth;
                        GameObject explodeOnDeathVoidExplosion = UnityEngine.Object.Instantiate(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, victimBody.corePosition, Quaternion.identity);
                        explodeOnDeathVoidExplosion.GetComponent<TeamFilter>().teamIndex = attacterBody.teamComponent.teamIndex;
                        DelayBlast delayBlast = explodeOnDeathVoidExplosion.GetComponent<DelayBlast>();
                        delayBlast.attacker = damageInfo.attacker;
                        delayBlast.baseDamage = Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient * combinedHealthFraction) + attacterBody.damage * itemCount;
                        delayBlast.crit = damageInfo.crit;
                        delayBlast.position = damageInfo.position;
                        delayBlast.procCoefficient = combinedHealthFraction * damageInfo.procCoefficient;
                        delayBlast.radius = BaseRadius + combinedHealthFraction * victimBody.bestFitRadius;
                        NetworkServer.Spawn(explodeOnDeathVoidExplosion);
                        victimBody.AddBuff(Main.VoidFire);
                        InflictDotInfo inflictDotInfo = new() {
                            attackerObject = damageInfo.attacker,
                            damageMultiplier = 1f,
                            dotIndex = DotController.DotIndex.PercentBurn,
                            totalDamage = combinedHealth,
                            victimObject = healthComponent.gameObject,
                        };
                        DotController.InflictDot(ref inflictDotInfo);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ExplodeOnDeathVoid Hook Failed!");
            }
            if (ilcursor.TryGotoPrev(MoveType.After,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchCall<HealthComponent>("get_fullCombinedHealth"))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_R4, 0f);
            } else {
                Main.Logger.LogError("ExplodeOnDeathVoid ProcHook Failed!");
            }
        }
    }
}