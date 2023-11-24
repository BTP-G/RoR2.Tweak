using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class RingsTweak : TweakBase<RingsTweak> {
        public const int FireRingDamageCoefficient = 3;
        public const int IceRingDamageCoefficient = 3;
        public const int IceRingSlow80BuffDuration = 3;
        public const int VoidRingDamageCoefficient = 1;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void Load() {
            AssetReferences.fireTornado.GetComponent<ProjectileOverlapAttack>().overlapProcCoefficient = 0.1f;
            AssetReferences.fireTornado.GetComponent<ProjectileSimple>().lifetime = 3f;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(1),
                                     x => x.MatchLdflda<DamageInfo>("procChainMask"),
                                     x => x.MatchLdcI4(12),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.Emit(OpCodes.Ldloc, 5);
                ilcursor.EmitDelegate((bool hasRingsProc, DamageInfo damageInfo, CharacterBody attackerBody, Inventory inventory) => {
                    if (hasRingsProc || damageInfo.damage < 4f * attackerBody.damage) {
                        return;
                    }
                    if (attackerBody.HasBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex)) {
                        attackerBody.RemoveBuff(RoR2Content.Buffs.ElementalRingsReady);
                        int iceRingCount = inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                        int fireRingCount = inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                        if (attackerBody.bodyIndex == BodyIndexes.MageBody) {
                            attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, 1f);
                        } else {
                            for (int i = Mathf.Max(1, 10 - (iceRingCount > fireRingCount ? fireRingCount : iceRingCount)); i > 0; --i) {
                                attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, i);
                            }
                        }
                        var ringProcChainMask = damageInfo.procChainMask;
                        ringProcChainMask.AddProc(ProcType.Rings);
                        ringProcChainMask.AddGreenProcs();
                        if (iceRingCount > 0) {
                            var blastAttack = new BlastAttack() {
                                attacker = damageInfo.attacker,
                                baseDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, IceRingDamageCoefficient * iceRingCount),
                                canRejectForce = true,
                                crit = damageInfo.crit,
                                damageColorIndex = DamageColorIndex.Item,
                                damageType = DamageType.AOE | DamageType.Freeze2s,
                                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                                position = damageInfo.position,
                                procChainMask = ringProcChainMask,
                                procCoefficient = 1f,
                                radius = 12f,
                                teamIndex = attackerBody.teamComponent.teamIndex,
                            };
                            var result = blastAttack.Fire();
                            if (result.hitCount > 0) {
                                foreach (var hitPoint in result.hitPoints) {
                                    var healthComponent = hitPoint.hurtBox.healthComponent;
                                    if (healthComponent.alive) {
                                        healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Slow80, IceRingSlow80BuffDuration * iceRingCount);
                                    }
                                }
                            }
                            EffectManager.SpawnEffect(AssetReferences.affixWhiteExplosion, new EffectData() {
                                origin = blastAttack.position,
                                scale = blastAttack.radius,
                            }, true);
                        }
                        if (fireRingCount > 0) {
                            ProjectileManager.instance.FireProjectile(new FireProjectileInfo {
                                crit = damageInfo.crit,
                                damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, FireRingDamageCoefficient * fireRingCount) * 0.1f,
                                damageColorIndex = DamageColorIndex.Item,
                                owner = damageInfo.attacker,
                                position = damageInfo.position,
                                procChainMask = ringProcChainMask,
                                projectilePrefab = AssetReferences.fireTornado,
                                rotation = Quaternion.identity,
                            });
                        }
                    } else if (attackerBody.HasBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex)) {
                        attackerBody.RemoveBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex);
                        int voidRingCount = inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        if (attackerBody.bodyIndex == BodyIndexes.VoidSurvivorBody) {
                            attackerBody.AddTimedBuff(DLC1Content.Buffs.ElementalRingVoidCooldown, 2f);
                        } else {
                            for (int i = Mathf.Max(2, 20 - voidRingCount); i > 0; --i) {
                                attackerBody.AddTimedBuff(DLC1Content.Buffs.ElementalRingVoidCooldown, i);
                            }
                        }
                        var fireProjectileInfo = new FireProjectileInfo {
                            crit = damageInfo.crit,
                            damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, voidRingCount),
                            damageColorIndex = DamageColorIndex.Void,
                            force = 6000f,
                            owner = damageInfo.attacker,
                            position = damageInfo.position,
                            procChainMask = damageInfo.procChainMask,
                            projectilePrefab = AssetReferences.elementalRingVoidBlackHole,
                        };
                        fireProjectileInfo.procChainMask.AddProc(ProcType.Rings);
                        fireProjectileInfo.procChainMask.AddGreenProcs();
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                Main.Logger.LogError("Rings :: Hook Failed!");
            }
        }
    }
}