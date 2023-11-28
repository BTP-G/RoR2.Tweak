using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExplodeOnDeathVoidTweak : TweakBase<ExplodeOnDeathVoidTweak> {
        public const float BaseDamageCoefficient = 0.5f;
        public const float 半数 = 1f;
        public const int BaseRadius = 12;
        public const int StackRadius = 3;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        private void Load() {
            var delayBlast = HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab.GetComponent<DelayBlast>();
            delayBlast.baseForce = 0f;
            delayBlast.damageColorIndex = DamageColorIndex.Void;
            delayBlast.damageType = DamageType.AOE;
            delayBlast.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            delayBlast.inflictor = null;
            delayBlast.maxTimer = 0.2f;
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
                        float combinedHealthFraction = combinedHealth > fullCombinedHealth ? 1f : combinedHealth / fullCombinedHealth;
                        GameObject explodeOnDeathVoidExplosion = Object.Instantiate(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, victimBody.corePosition, Quaternion.identity);
                        explodeOnDeathVoidExplosion.GetComponent<TeamFilter>().teamIndex = attacterBody.teamComponent.teamIndex;
                        DelayBlast delayBlast = explodeOnDeathVoidExplosion.GetComponent<DelayBlast>();
                        delayBlast.attacker = damageInfo.attacker;
                        delayBlast.baseDamage = Util.OnHitProcDamage(damageInfo.damage, 0, BtpUtils.简单逼近(itemCount, 半数, combinedHealthFraction));
                        delayBlast.crit = damageInfo.crit;
                        delayBlast.position = damageInfo.position;
                        delayBlast.procCoefficient = combinedHealthFraction * damageInfo.procCoefficient;
                        delayBlast.radius = combinedHealthFraction * (BaseRadius + StackRadius * (itemCount - 1) + victimBody.bestFitRadius);
                        NetworkServer.Spawn(explodeOnDeathVoidExplosion);
                        victimBody.AddBuff(Main.VoidFire);
                        var inflictDotInfo = new InflictDotInfo {
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