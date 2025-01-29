using BtpTweak.Pools;
using BtpTweak.Pools.ProjectilePools;
using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class RingsTweak : TweakBase<RingsTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float FireRingDamageCoefficient = 1f;
        public const float FireRingProcCoefficientPerTick = 0.2f;
        public const float FireRingInterval = 1f;
        public const float IceRingDamageCoefficient = 1f;
        public const float IceRingSlow80BuffDuration = 4f;
        public const float IceRingRadius = 4f;
        public const float IceRingInterval = 1f;
        public const float IceRingProcCoefficient = 1f;
        public const float VoidRingDamageCoefficient = 1f;
        public const float VoidRingProcCoefficient = 1f;
        public const float VoidRingInterval = 2f;
        public const float VoidRingBaseRadius = 15f;
        public const float VoidRingStackRadius = 3f;
        public const float RingDamageRequired = 1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var projectileOverlapAttack = AssetReferences.fireTornado.GetComponent<ProjectileOverlapAttack>();
            projectileOverlapAttack.overlapProcCoefficient = FireRingProcCoefficientPerTick;
            projectileOverlapAttack.SetDamageCoefficient(0.1f);
            AssetReferences.fireTornado.GetComponent<ProjectileSimple>().lifetime = 3f;
            AssetReferences.elementalRingVoidBlackHole.AddComponent<ElementalRingVoidBlackHoleAction>();
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(1),
                                     x => x.MatchLdflda<DamageInfo>("procChainMask"),
                                     x => x.MatchLdcI4(12),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                cursor.Emit(OpCodes.Ldarg_1)
                      .Emit(OpCodes.Ldloc, 1)
                      .Emit(OpCodes.Ldloc, 6)
                      .EmitDelegate((bool hasRingsProc, DamageInfo damageInfo, CharacterBody attackerBody, Inventory inventory) => {
                          if (hasRingsProc) {
                              return;
                          }
                          if (attackerBody.HasBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex)) {
                              int iceRingCount = inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                              int fireRingCount = inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                              if (damageInfo.damage < (iceRingCount > fireRingCount ? iceRingCount : fireRingCount) * attackerBody.damage && attackerBody.bodyIndex != BodyIndexes.Mage) {
                                  return;
                              }
                              attackerBody.RemoveBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex);
                              var cooldown = (attackerBody.bodyIndex == BodyIndexes.Mage ? 1f : 10f) * Mathf.Pow(0.9f, iceRingCount < fireRingCount ? iceRingCount : fireRingCount);
                              while (cooldown > 0) {
                                  attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, cooldown--);
                              }
                              var ringProcChainMask = damageInfo.procChainMask;
                              ringProcChainMask.AddRYProcs();
                              if (iceRingCount > 0) {
                                  var attackInfo = new IceBlastPoolKey {
                                      attacker = damageInfo.attacker,
                                      crit = damageInfo.crit,
                                      procChainMask = ringProcChainMask,
                                      teamIndex = attackerBody.teamComponent.teamIndex,
                                  };
                                  IceBlastPool.RentPool(damageInfo.attacker).AddIceBlast(attackInfo,
                                                                                       damageInfo.position,
                                                                                       Util.OnHitProcDamage(damageInfo.damage, 0, IceRingDamageCoefficient * iceRingCount));
                              }
                              if (fireRingCount > 0) {
                                  var simpleProjectileInfo = new ProjectilePoolKey {
                                      attacker = damageInfo.attacker,
                                      isCrit = damageInfo.crit,
                                      procChainMask = ringProcChainMask,
                                  };
                                  FireTornadoPool.RentPool(damageInfo.attacker).AddProjectile(simpleProjectileInfo,
                                                                                                 damageInfo.position,
                                                                                                 Util.OnHitProcDamage(damageInfo.damage, 0, FireRingDamageCoefficient * fireRingCount));
                              }
                          } else if (attackerBody.HasBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex)) {
                              var voidRingCount = inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
                              if (damageInfo.damage < voidRingCount * attackerBody.damage) {
                                  return;
                              }
                              attackerBody.RemoveBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex);
                              var cooldown = (attackerBody.bodyIndex == BodyIndexes.VoidSurvivor ? 2f : 20f) * Mathf.Pow(0.9f, voidRingCount - 1);
                              while (cooldown > 0) {
                                  attackerBody.AddTimedBuff(DLC1Content.Buffs.ElementalRingVoidCooldown, cooldown--);
                              }
                              var simpleProjectileInfo = new ProjectilePoolKey {
                                  attacker = damageInfo.attacker,
                                  isCrit = damageInfo.crit,
                                  procChainMask = damageInfo.procChainMask,
                              };
                              simpleProjectileInfo.procChainMask.AddRYProcs();
                              ElementalRingVoidBlackHolePool.RentPool(damageInfo.attacker).AddProjectile(simpleProjectileInfo,
                                                                                                         damageInfo.position,
                                                                                                         Util.OnHitProcDamage(damageInfo.damage, 0, VoidRingDamageCoefficient * voidRingCount));
                          }
                      });
                cursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                Main.Logger.LogError("Rings :: Hook Failed!");
            }
        }

        private class ElementalRingVoidBlackHoleAction : MonoBehaviour {
            private ProjectileFuse projectileFuse;
            private RadialForce radialForce;
            private float fullFuse;
            private float minForceMagnitude;
            private float maxForceMagnitude;
            private float minRadius;
            private float maxRadius;
            private float scale = 1f;

            private void Awake() {
                projectileFuse = GetComponent<ProjectileFuse>();
                radialForce = GetComponent<RadialForce>();
                fullFuse = projectileFuse.fuse;
                minRadius = radialForce.radius;
                minForceMagnitude = radialForce.forceMagnitude;
            }

            private void Start() {
                var pc = GetComponent<ProjectileController>();
                if (pc.owner) {
                    scale += 0.2f * pc.owner.GetComponent<CharacterBody>().inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
                }
                transform.localScale = new Vector3(0, 0, 0);
                maxRadius = minRadius * scale;
                maxForceMagnitude = minForceMagnitude * scale;
            }

            private void FixedUpdate() {
                var t = 1 - projectileFuse.fuse / fullFuse;
                if (t < 0.9f) {
                    t = t / 9f * 10f;
                    var currentLocalScale = Mathf.SmoothStep(0f, scale, t);
                    transform.localScale = new Vector3(currentLocalScale, currentLocalScale, currentLocalScale);
                    radialForce.radius = Mathf.SmoothStep(0f, maxRadius, t);
                    radialForce.forceMagnitude = Mathf.SmoothStep(0f, maxForceMagnitude, t);
                } else {
                    t = (t - 0.9f) * 10f;
                    var currentLocalScale = Mathf.Lerp(scale, 0f, t);
                    transform.localScale = new Vector3(currentLocalScale, currentLocalScale, currentLocalScale);
                    radialForce.radius = Mathf.Lerp(maxRadius, 0f, t);
                    radialForce.forceMagnitude = Mathf.Lerp(maxForceMagnitude, 0f, t);
                }
            }
        }
    }
}