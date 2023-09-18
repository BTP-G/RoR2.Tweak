using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class EquipmentTweak : TweakBase {
        public readonly GameObject magmaOrbProjectile = "RoR2/Base/MagmaWorm/MagmaOrbProjectile.prefab".Load<GameObject>();

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.Inventory.UpdateEquipment += IL_Inventory_UpdateEquipment;
            FireballVehicleHook();
            GoldGatFireHook();
            LightningHook();
            AspectGoldHook();
        }

        private void AspectGoldHook() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += delegate (ILContext il) {
                ILCursor iLCursor = new(il);
                if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Items).GetField("GoldOnHit")))) {
                    iLCursor.Emit(OpCodes.Ldarg_1);
                    iLCursor.Emit(OpCodes.Ldarg_2);
                    iLCursor.Emit(OpCodes.Ldloc_1);
                    iLCursor.EmitDelegate(delegate (DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) {
                        if (attackerBody.HasBuff(GoldenCoastPlus.GoldenCoastPlus.affixGoldDef)) {
                            if (victim.TryGetComponent<DeathRewards>(out var deathRewards)) {
                                attackerBody.master.GiveMoney((uint)(deathRewards.goldReward * damageInfo.procCoefficient));
                            }
                        }
                    });
                } else {
                    Main.logger_.LogError("AspectGold Hook Failed!");
                }
            };
        }

        public override void Load() {
            var sawmerang = "RoR2/Base/Saw/Sawmerang.prefab".Load<GameObject>();
            var boomerangProjectile = sawmerang.GetComponent<BoomerangProjectile>();
            boomerangProjectile.transitionDuration *= 3f;
            boomerangProjectile.travelSpeed = 36;
            FireballVehicle fireballVehicle = "RoR2/Base/FireBallDash/FireballVehicle.prefab".LoadComponent<FireballVehicle>();
            fireballVehicle.duration = 6;
            fireballVehicle.overlapResetFrequency = 1f;
            EntityStates.GoldGat.GoldGatFire.maxFireFrequency *= 2;
            GameObject beamSphere = "RoR2/Base/BFG/BeamSphere.prefab".Load<GameObject>();
            ProjectileProximityBeamController proximityBeamController = beamSphere.GetComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange = 66.6f;
            proximityBeamController.damageCoefficient = 6.66f;
            ProjectileImpactExplosion projectileImpactExplosion = beamSphere.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.blastDamageCoefficient = 66.6f;
            projectileImpactExplosion.lifetime = 66.6f;
            ProjectileSimple projectileSimple = beamSphere.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 10f;
            projectileSimple.lifetime = 66.6f;
        }

        private void FireballVehicleHook() {
            On.RoR2.FireballVehicle.FixedUpdate += delegate (On.RoR2.FireballVehicle.orig_FixedUpdate orig, FireballVehicle self) {
                if (NetworkServer.active && self.overlapResetAge + Time.fixedDeltaTime >= 1f / self.overlapResetFrequency) {
                    CharacterBody currentPassengerBody = self.vehicleSeat.currentPassengerBody;
                    if (currentPassengerBody != null) {
                        FireProjectileInfo fireProjectileInfo = new() {
                            crit = currentPassengerBody.RollCrit(),
                            damage = currentPassengerBody.damage * 4f * (1 + currentPassengerBody.inventory.GetItemCount(RoR2Content.Items.FireballsOnHit.itemIndex)),
                            damageColorIndex = DamageColorIndex.Item,
                            force = 100f,
                            fuseOverride = 3,
                            owner = currentPassengerBody.gameObject,
                            position = currentPassengerBody.corePosition,
                            procChainMask = default,
                            projectilePrefab = magmaOrbProjectile,
                            rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f))),
                            speedOverride = Random.Range(15, 30),
                            target = null,
                            useFuseOverride = true,
                            useSpeedOverride = true,
                        };
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                        fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f)));
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                        fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f)));
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    }
                }
                orig(self);
            };
        }

        private void GoldGatFireHook() {
            IL.EntityStates.GoldGat.GoldGatFire.FireBullet += delegate (ILContext il) {
                ILCursor iLCursor = new(il);
                if (iLCursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdsfld<EntityStates.GoldGat.GoldGatFire>(x, "baseMoneyCostPerBullet"))) {
                    ++iLCursor.Index;
                    iLCursor.Emit(OpCodes.Ldarg_0);
                    iLCursor.Emit<EntityStates.GoldGat.GoldGatFire>(OpCodes.Ldfld, "totalStopwatch");
                    iLCursor.Emit(OpCodes.Add);
                } else {
                    Main.logger_.LogError("GoldGat Hook Failed!");
                }
                if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld<EntityStates.GoldGat.GoldGatFire>(x, "damageCoefficient"))) {
                    --iLCursor.Index;
                    iLCursor.Remove();
                    iLCursor.EmitDelegate((CharacterBody body) => {
                        return body.damage * body.inventory.GetItemCount(DLC1Content.Items.GoldOnHurt.itemIndex);
                    });
                    if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<BulletAttack>(x, "damage"))) {
                        iLCursor.Emit(OpCodes.Ldloc_2);
                        iLCursor.Emit(OpCodes.Conv_R4);
                        iLCursor.Emit(OpCodes.Add);
                    } else {
                        Main.logger_.LogError("GoldGat DamageHook 2 Failed!");
                    }
                } else {
                    Main.logger_.LogError("GoldGat DamageHook 1 Failed!");
                }
            };
        }

        private void IL_Inventory_UpdateEquipment(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 6))) {
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((float chargeFinishTime, Inventory inventory) => {
                    return Mathf.Max(0.15f * inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment.itemIndex), chargeFinishTime);
                });
            } else {
                Main.logger_.LogError("AutoCastEquipment Hook Failed!");
            }
        }

        private void LightningHook() {
            IL.RoR2.EquipmentSlot.FireLightning += delegate (ILContext il) {
                ILCursor iLCursor = new(il);
                if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<RoR2.Orbs.GenericDamageOrb>(x, "damageValue"))) {
                    iLCursor.Index -= 3;
                    iLCursor.RemoveRange(3);
                    iLCursor.EmitDelegate((CharacterBody body) => {
                        return body.damage * 30f * (1 + body.inventory.GetItemCount(RoR2Content.Items.LightningStrikeOnHit.itemIndex));
                    });
                } else {
                    Main.logger_.LogError("FireLightning Hook Failed!");
                }
            };
        }
    }
}