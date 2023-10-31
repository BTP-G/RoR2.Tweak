using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class EquipmentTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.Inventory.UpdateEquipment += IL_Inventory_UpdateEquipment;
            FireballVehicleHook();
            GoldGatFireHook();
            AspectGoldHook();
            RecycleHook();
        }

        public override void Load() {
            base.Load();
            var sawmerang = "RoR2/Base/Saw/Sawmerang.prefab".Load<GameObject>();
            var boomerangProjectile = sawmerang.GetComponent<BoomerangProjectile>();
            boomerangProjectile.transitionDuration *= 5f;
            boomerangProjectile.travelSpeed = 36f;
            FireballVehicle fireballVehicle = "RoR2/Base/FireBallDash/FireballVehicle.prefab".LoadComponent<FireballVehicle>();
            fireballVehicle.duration = 6;
            fireballVehicle.overlapResetFrequency = 3f;
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
            RoR2Content.Equipment.Saw.cooldown = 20f;
            RoR2Content.Equipment.LifestealOnHit.cooldown = 40f;
            DLC1Content.Equipment.Molotov.cooldown = 30f;
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
                    Main.Logger.LogError("AspectGold Hook Failed!");
                }
            };
        }

        private void FireballVehicleHook() => On.RoR2.FireballVehicle.FixedUpdate += delegate (On.RoR2.FireballVehicle.orig_FixedUpdate orig, FireballVehicle self) {
            if (NetworkServer.active && self.overlapResetAge + Time.fixedDeltaTime >= 1f / self.overlapResetFrequency) {
                CharacterBody currentPassengerBody = self.vehicleSeat.currentPassengerBody;
                if (currentPassengerBody != null) {
                    FireProjectileInfo fireProjectileInfo = new() {
                        crit = currentPassengerBody.RollCrit(),
                        damage = currentPassengerBody.damage * 3f * (1 + currentPassengerBody.inventory.GetItemCount(RoR2Content.Items.FireballsOnHit.itemIndex)),
                        damageColorIndex = DamageColorIndex.Item,
                        force = 100f,
                        fuseOverride = 3,
                        owner = currentPassengerBody.gameObject,
                        position = currentPassengerBody.corePosition,
                        procChainMask = default,
                        projectilePrefab = AssetReferences.magmaOrbProjectile,
                        rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f))),
                        speedOverride = Random.Range(10, 30),
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

        private void GoldGatFireHook() => IL.EntityStates.GoldGat.GoldGatFire.FireBullet += delegate (ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdsfld<EntityStates.GoldGat.GoldGatFire>(x, "baseMoneyCostPerBullet"))) {
                ++iLCursor.Index;
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.Emit<EntityStates.GoldGat.GoldGatFire>(OpCodes.Ldfld, "totalStopwatch");
                iLCursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("GoldGat Hook Failed!");
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
                    Main.Logger.LogError("GoldGat DamageHook 2 Failed!");
                }
            } else {
                Main.Logger.LogError("GoldGat DamageHook 1 Failed!");
            }
        };

        private void IL_Inventory_UpdateEquipment(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 6))) {
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((float chargeFinishTime, Inventory inventory) => {
                    return Mathf.Max(0.15f * inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment.itemIndex), chargeFinishTime);
                });
            } else {
                Main.Logger.LogError("AutoCastEquipment Hook Failed!");
            }
        }

        private void RecycleHook() => On.RoR2.EquipmentSlot.FireRecycle += delegate (On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self) {
            self.UpdateTargets(RoR2Content.Equipment.Recycle.equipmentIndex, false);
            GenericPickupController pickupController = self.currentTarget.pickupController;
            if (!pickupController || pickupController.pickupIndex.pickupDef.equipmentIndex == RoR2Content.Equipment.Recycle.equipmentIndex) {
                self.InvalidateCurrentTarget();
                return false;
            }
            PickupIndex initialPickupIndex = pickupController.pickupIndex;
            self.subcooldownTimer = 0.2f;
            PickupIndex[] array = (from pickupIndex in PickupTransmutationManager.GetAvailableGroupFromPickupIndex(pickupController.pickupIndex)
                                   where pickupIndex != initialPickupIndex
                                   select pickupIndex).ToArray();
            if (array == null || array.Length == 0) {
                return false;
            }
            PickupIndex newPickupIndex = Run.instance.treasureRng.NextElementUniform(array);
            switch (newPickupIndex.pickupDef.itemTier) {
                case ItemTier.Tier1:
                    if (Util.CheckRoll(5)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapWhite.itemIndex);
                    }
                    break;

                case ItemTier.Tier2:
                    if (Util.CheckRoll(10)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapGreen.itemIndex);
                    }
                    break;

                case ItemTier.Tier3:
                    if (Util.CheckRoll(20)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapRed.itemIndex);
                    }
                    break;

                case ItemTier.Lunar:
                    if (!SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("moon") && Util.CheckRoll(15)) {
                        Object.Destroy(pickupController.gameObject);
                        EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
                            origin = pickupController.pickupDisplay.transform.position,
                            rotation = default,
                        }, true);
                        return true;
                    }
                    break;

                case ItemTier.Boss:
                    if (Util.CheckRoll(25)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapYellow.itemIndex);
                    }
                    break;

                case ItemTier.VoidTier1:
                case ItemTier.VoidTier2:
                case ItemTier.VoidTier3:
                case ItemTier.VoidBoss:
                    if (Util.CheckRoll(Mathf.Pow((float)newPickupIndex.pickupDef.itemTier, 1.5f))) {
                        Object.Destroy(pickupController.gameObject);
                        CharacterBody body = self.characterBody;
                        FireProjectileInfo fireProjectileInfo = new() {
                            projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
                            position = pickupController.pickupDisplay.transform.position,
                            rotation = Quaternion.identity,
                            owner = body.gameObject,
                            damage = body.damage,
                            crit = body.RollCrit()
                        };
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                        return true;
                    }
                    break;

                case ItemTier.NoTier:
                    if (newPickupIndex.pickupDef.equipmentIndex != EquipmentIndex.None && newPickupIndex.pickupDef.isLunar) {
                        if (!SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("moon") && Util.CheckRoll(15)) {
                            Object.Destroy(pickupController.gameObject);
                            EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
                                origin = pickupController.pickupDisplay.transform.position,
                                rotation = default,
                            }, true);
                            return true;
                        }
                    }
                    break;
            }
            pickupController.NetworkpickupIndex = newPickupIndex;
            EffectManager.SimpleEffect(AssetReferences.omniRecycleEffect, pickupController.pickupDisplay.transform.position, Quaternion.identity, true);
            return true;
        };
    }
}