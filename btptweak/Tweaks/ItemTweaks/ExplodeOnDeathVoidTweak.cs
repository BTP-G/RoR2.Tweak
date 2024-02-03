using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExplodeOnDeathVoidTweak : TweakBase<ExplodeOnDeathVoidTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BaseDamageCoefficient = 0.5f;
        public const float 半数 = 1f;
        public const int BaseRadius = 12;
        public const int StackRadius = 3;
        public static readonly GameObject explosionEffect = GameObjectPaths.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>();

        private static readonly BlastAttack _blastAttack = new() {
            damageColorIndex = DamageColorIndex.Void,
            damageType = DamageType.AOE | DamageType.IgniteOnHit,
            falloffModel = BlastAttack.FalloffModel.None,
        };

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var delayBlast = HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab.GetComponent<DelayBlast>();
            delayBlast.baseForce = 0f;
            delayBlast.damageColorIndex = DamageColorIndex.Void;
            delayBlast.damageType = DamageType.AOE | DamageType.IgniteOnHit;
            delayBlast.falloffModel = BlastAttack.FalloffModel.None;
            delayBlast.inflictor = null;
            delayBlast.maxTimer = 0.25f;
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
                        var victimBody = healthComponent.body;
                        if (victimBody.HasBuff(BtpContent.Buffs.VoidFire)) {
                            return;
                        }
                        victimBody.AddBuff(BtpContent.Buffs.VoidFire);
                        var combinedHealthFraction = Mathf.Clamp01(healthComponent.combinedHealthFraction);
                        _blastAttack.attacker = damageInfo.attacker;
                        _blastAttack.baseDamage = Util.OnHitProcDamage(damageInfo.damage, 0, BtpUtils.简单逼近(itemCount, 半数, combinedHealthFraction));
                        _blastAttack.crit = damageInfo.crit;
                        _blastAttack.position = damageInfo.position;
                        _blastAttack.procChainMask = damageInfo.procChainMask;
                        _blastAttack.procCoefficient = combinedHealthFraction;
                        _blastAttack.radius = combinedHealthFraction * (BaseRadius + StackRadius * (itemCount - 1) + victimBody.bestFitRadius);
                        _blastAttack.teamIndex = attacterBody.teamComponent.teamIndex;
                        _blastAttack.Fire();
                        EffectManager.SpawnEffect(explosionEffect, new EffectData {
                            origin = _blastAttack.position,
                            scale = _blastAttack.radius,
                        }, true);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ExplodeOnDeathVoid Hook Failed!");
            }
            if (ilcursor.TryGotoPrev(MoveType.Before,
                                     x => x.MatchLdloc(4),
                                     x => x.MatchLdarg(0),
                                     x => x.MatchCall<HealthComponent>("get_fullCombinedHealth"))) {
                var labels = ilcursor.IncomingLabels;
                ilcursor.RemoveRange(7);
                foreach (var label in labels) {
                    ilcursor.MarkLabel(label);
                }
            } else {
                Main.Logger.LogError("ExplodeOnDeathVoid ProcHook Failed!");
            }
        }
    }
}