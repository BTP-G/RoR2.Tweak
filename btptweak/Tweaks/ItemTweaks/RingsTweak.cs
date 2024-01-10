using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class RingsTweak : TweakBase<RingsTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const int FireRingDamageCoefficient = 3;
        public const int IceRingDamageCoefficient = 3;
        public const int IceRingSlow80BuffDuration = 3;
        public const int VoidRingDamageCoefficient = 1;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AssetReferences.fireTornado.GetComponent<ProjectileOverlapAttack>().overlapProcCoefficient = 0.2f;
            AssetReferences.fireTornado.GetComponent<ProjectileSimple>().lifetime = 3f;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(1),
                                     x => x.MatchLdflda<DamageInfo>("procChainMask"),
                                     x => x.MatchLdcI4(12),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.Emit(OpCodes.Ldloc, 1);
                cursor.Emit(OpCodes.Ldloc, 5);
                cursor.EmitDelegate((bool hasRingsProc, DamageInfo damageInfo, CharacterBody attackerBody, Inventory inventory) => {
                    if (hasRingsProc) {
                        return;
                    }
                    if (attackerBody.HasBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex)) {
                        int iceRingCount = inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                        int fireRingCount = inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                        if (damageInfo.damage < (3 + (iceRingCount > fireRingCount ? iceRingCount : fireRingCount)) * attackerBody.damage) {
                            return;
                        }
                        attackerBody.RemoveBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex);
                        var cooldown = (attackerBody.bodyIndex == BodyIndexes.VoidSurvivor ? 1f : 10f) * Mathf.Pow(0.9f, iceRingCount < fireRingCount ? fireRingCount : iceRingCount);
                        while (cooldown > 0) {
                            attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, cooldown--);
                        }
                        var ringProcChainMask = damageInfo.procChainMask;
                        ringProcChainMask.AddProc(ProcType.Rings);
                        ringProcChainMask.AddGreenProcs();
                        if (iceRingCount > 0) {
                            var blastAttack = new BlastAttack {
                                attacker = damageInfo.attacker,
                                baseDamage = Util.OnHitProcDamage(damageInfo.damage, 0, IceRingDamageCoefficient * iceRingCount),
                                canRejectForce = true,
                                crit = damageInfo.crit,
                                damageColorIndex = DamageColorIndex.Item,
                                damageType = DamageType.AOE,
                                falloffModel = BlastAttack.FalloffModel.None,
                                position = damageInfo.position,
                                procChainMask = ringProcChainMask,
                                procCoefficient = 1f,
                                radius = 12f,
                                teamIndex = attackerBody.teamComponent.teamIndex,
                            };
                            foreach (var hitPoint in blastAttack.Fire().hitPoints) {
                                var healthComponent = hitPoint.hurtBox.healthComponent;
                                if (healthComponent.alive) {
                                    healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Slow80, IceRingSlow80BuffDuration * iceRingCount);
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
                                damage = Util.OnHitProcDamage(damageInfo.damage, 0, FireRingDamageCoefficient * fireRingCount * 0.1f),
                                damageColorIndex = DamageColorIndex.Item,
                                damageTypeOverride = DamageType.IgniteOnHit,
                                owner = damageInfo.attacker,
                                position = damageInfo.position,
                                procChainMask = ringProcChainMask,
                                projectilePrefab = AssetReferences.fireTornado,
                                rotation = Quaternion.identity,
                            });
                        }
                    } else if (attackerBody.HasBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex)) {
                        if (damageInfo.damage < 4 * attackerBody.damage) {
                            return;
                        }
                        int voidRingCount = inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        attackerBody.RemoveBuff(DLC1Content.Buffs.ElementalRingVoidReady.buffIndex);
                        var cooldown = (attackerBody.bodyIndex == BodyIndexes.VoidSurvivor ? 2f : 20f) * Mathf.Pow(0.9f, voidRingCount - 1);
                        while (cooldown > 0) {
                            attackerBody.AddTimedBuff(DLC1Content.Buffs.ElementalRingVoidCooldown, cooldown--);
                        }
                        var fireProjectileInfo = new FireProjectileInfo {
                            crit = damageInfo.crit,
                            damage = Util.OnHitProcDamage(damageInfo.damage, 0, voidRingCount),
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
                cursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                Main.Logger.LogError("Rings :: Hook Failed!");
            }
        }
    }
}