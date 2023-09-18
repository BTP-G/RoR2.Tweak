using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.Audio;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class ItemTweak : TweakBase {
        private static GameObject electricOrbProjectile;
        private static GameObject fireMeatBallProjectile;
        private int RerolledCount;
        private BodyIndex 工程师固定炮台_;
        private BodyIndex 工程师移动炮台_;
        private bool 位于虚空之境;

        public override void AddHooks() {
            ExecuteLowHealthEliteHook();
            FireworkHook();
            IgniteOnKillHook();
            IL_OnCharacterDeathHooks();
            IL_OnHitEnemyHooks();
            IL_TakeDamageHooks();
            LunarSunHook();
            MissileVoidHook();
            RandomlyLunarHook();
            RepeatHealHook();
        }

        public override void Load() {
            electricOrbProjectile = Helpers.FindProjectilePrefab("ElectricOrbProjectile");
            fireMeatBallProjectile = Helpers.FindProjectilePrefab("FireMeatBall");
            MissileController missileController = GlobalEventManager.CommonAssets.missilePrefab.GetComponent<MissileController>();
            missileController.maxVelocity *= 2f;
            missileController.acceleration *= 10f;
            missileController.delayTimer *= 0.01f;
            missileController.turbulence = 0;
            missileController.maxSeekDistance = float.MaxValue;
            missileController = FireworkPool.fireworkPrefab.GetComponent<MissileController>();
            missileController.maxVelocity *= 2f;
            missileController.acceleration *= 10f;
            missileController.delayTimer *= 0.01f;
            missileController.turbulence = 0;
            missileController.maxSeekDistance = float.MaxValue;
            工程师固定炮台_ = BodyCatalog.FindBodyIndex("EngiTurretBody");
            工程师移动炮台_ = BodyCatalog.FindBodyIndex("EngiWalkerTurretBody");
            PickupDropletController.pickupDropletPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(5f);
            GenericPickupController.pickupPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(100f);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, DLC1Content.Items.ExtraLifeVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, DLC1Content.Items.MushroomVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.NovaOnHeal);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.NovaOnLowHealth);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ShockNearby);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.BearVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.MinorConstructOnKill);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.BeetleGland);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ExecuteLowHealthElite);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.RoboBallBuddy);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ShockNearby);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Thorns);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, "RoR2/DLC1/LunarWings/LunarWings.asset".Load<ItemDef>());
            RoR2Content.Items.Firework.tags = new ItemTag[] { ItemTag.Damage };
            RoR2Content.Items.FlatHealth.tags = new ItemTag[] { ItemTag.Healing };
            "RoR2/Base/BonusGoldPackOnKill/BonusMoneyPack.prefab".LoadComponentInChildren<GravitatePickup>().maxSpeed = 50;
            ProjectileImpactExplosion projectileImpactExplosion = "RoR2/Base/StickyBomb/StickyBomb.prefab".LoadComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.lifetime *= 0.01f;
            projectileImpactExplosion.lifetimeAfterImpact *= 0.01f;
        }

        public override void StageStartAction(Stage stage) {
            位于虚空之境 = stage.sceneDef.cachedName == "arena";
            RerolledCount = 0;
        }

        private void ExecuteLowHealthEliteHook() => IL.RoR2.CharacterBody.OnInventoryChanged += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdarg(x, 0),
                x => ILPatternMatchingExt.MatchLdcR4(x, 13f),
                x => ILPatternMatchingExt.MatchLdarg(x, 0),
            })) {
                ++ilcursor.Index;
                ilcursor.RemoveRange(11);
                ilcursor.EmitDelegate(delegate (CharacterBody body) {
                    float num = body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                    body.executeEliteHealthFraction = 0.5f * (num / (num + 4));
                });
            } else {
                Main.logger_.LogError("ExecuteLowHealthElite Hook Failed!");
            }
        };

        private void FireworkHook() {
            IL.RoR2.GlobalEventManager.OnInteractionBegin += delegate (ILContext il) {
                ILCursor ilcursor = new(il);
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchStloc(x, 6))) {
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                    ilcursor.Emit(OpCodes.Stloc, 6);
                } else {
                    Main.logger_.LogError("Firework :: RemoveHook Failed!");
                }
            };
            IL.RoR2.GlobalEventManager.OnHitEnemy += delegate (ILContext il) {
                ILCursor ilcursor = new(il);
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Items).GetField("Missile")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                    x => ILPatternMatchingExt.MatchStloc(x, 32),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldarg_2);
                    ilcursor.Emit(OpCodes.Ldloc_1);
                    ilcursor.EmitDelegate(delegate (DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) {
                        int itemCount = attackerBody.inventory.GetItemCount(RoR2Content.Items.Firework);
                        if (Util.CheckRoll(5 * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                            FireworkPool fireworkPool = attackerBody.GetComponent<FireworkPool>();
                            if (fireworkPool) {
                                fireworkPool.AddMissile(Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 1.5f), damageInfo.crit, victim, damageInfo.procChainMask);
                            } else {
                                attackerBody.gameObject.AddComponent<FireworkPool>().AddMissile(Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 1.5f), damageInfo.crit, victim, damageInfo.procChainMask);
                            }
                        }
                    });
                } else {
                    Main.logger_.LogError("Firework :: FireHook Failed!");
                }
            };
        }

        private void IgniteOnKillHook() => IL.RoR2.GlobalEventManager.ProcIgniteOnKill += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdcR4(x, 8f),
                x => ILPatternMatchingExt.MatchLdcR4(x, 4f),
            })) {
                ilcursor.RemoveRange(6);
                ilcursor.Emit(OpCodes.Ldc_R4, 16f);
            } else {
                Main.logger_.LogError("IgniteOnKill RadiusHook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 0.75f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 1.5f);
            } else {
                Main.logger_.LogError("IgniteOnKill DamageHook Failed!");
            }
        };

        private void IL_OnCharacterDeathHooks() => IL.RoR2.GlobalEventManager.OnCharacterDeath += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 53))) {
                ilcursor.Index -= 10;
                ilcursor.RemoveRange(10);
                ilcursor.Emit(OpCodes.Ldc_R4, 4f);
            } else {
                Main.logger_.LogError("ExplodeOnDeath DamageHook Failed!");
            }
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 2.4f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 6f);
            } else {
                Main.logger_.LogError("ExplodeOnDeath RadiusHook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdloc(x, 43),
                    x => ILPatternMatchingExt.MatchLdcI4(x, 100),
                    x => ILPatternMatchingExt.MatchMul(x),
                    x => ILPatternMatchingExt.MatchStloc(x, 63),
            })) {
                ilcursor.RemoveRange(25);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 17);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (CharacterBody attackerBody, Inventory inventory, Vector3 pos) {
                    int itemCountAll = Util.GetItemCountForTeam(attackerBody.teamComponent.teamIndex, RoR2Content.Items.Infusion.itemIndex, true);
                    if (inventory.infusionBonus < Convert.ToUInt64(attackerBody.level * attackerBody.levelMaxHealth * itemCountAll)) {
                        InfusionOrb infusionOrb = new() {
                            origin = pos,
                            target = attackerBody.mainHurtBox,
                            maxHpValue = itemCountAll
                        };
                        OrbManager.instance.AddOrb(infusionOrb);
                    }
                    if (attackerBody.bodyIndex == 工程师固定炮台_ || attackerBody.bodyIndex == 工程师移动炮台_) {
                        foreach (var teamMember in TeamComponent.GetTeamMembers(attackerBody.teamComponent.teamIndex)) {
                            CharacterBody body = teamMember.body;
                            if (body.isPlayerControlled) {
                                if (body.inventory.infusionBonus < Convert.ToUInt64(body.level * body.levelMaxHealth * itemCountAll)) {
                                    InfusionOrb infusionOrb = new() {
                                        origin = pos,
                                        target = body.mainHurtBox,
                                        maxHpValue = itemCountAll
                                    };
                                    OrbManager.instance.AddOrb(infusionOrb);
                                }
                            }
                        }
                    }
                });
            } else {
                Main.logger_.LogError("Infusion Hook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/NetworkedObjects/BonusMoneyPack"))) {
                ilcursor.RemoveRange(15);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 18);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) {
                    GameObject BonusMoneyPack = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BonusMoneyPack"), pos, UnityEngine.Random.rotation);
                    TeamFilter TeamFilter = BonusMoneyPack.GetComponent<TeamFilter>();
                    if (TeamFilter) {
                        TeamFilter.teamIndex = attacterTeamindex;
                        BonusMoneyPack.GetComponentInChildren<GravitatePickup>().gravitateTarget = attacterBody.coreTransform;
                    }
                    NetworkServer.Spawn(BonusMoneyPack);
                });
            } else {
                Main.logger_.LogError("BonusGoldPackOnKill Hook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchStloc(x, 51))) {
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.Emit(OpCodes.Ldloc, 51);
                ilcursor.EmitDelegate(delegate (CharacterBody attackerBody, CharacterBody victimBody, int count) {
                    if (count > 0) {
                        Util.PlaySound("Play_bleedOnCritAndExplode_explode", victimBody.gameObject);
                        GameObject bleedExplode = UnityEngine.Object.Instantiate(GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect, victimBody.corePosition, Quaternion.identity);
                        DelayBlast delayBlast = bleedExplode.GetComponent<DelayBlast>();
                        delayBlast.position = victimBody.corePosition;
                        delayBlast.baseDamage = Util.OnKillProcDamage(attackerBody.damage, 4f * count);
                        delayBlast.baseForce = 0f;
                        delayBlast.radius = 16f;
                        delayBlast.attacker = attackerBody.gameObject;
                        delayBlast.inflictor = null;
                        delayBlast.crit = Util.CheckRoll(attackerBody.crit, attackerBody.master);
                        delayBlast.maxTimer = 0f;
                        delayBlast.damageColorIndex = DamageColorIndex.Item;
                        delayBlast.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                        bleedExplode.GetComponent<TeamFilter>().teamIndex = attackerBody.teamComponent.teamIndex;
                        NetworkServer.Spawn(bleedExplode);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4, 0);
                ilcursor.Emit(OpCodes.Stloc, 51);
            } else {
                Main.logger_.LogError("ShatterSpleen :: BleedExplodeHook 1 Failed!");
            }
        };

        private void IL_OnHitEnemyHooks() => IL.RoR2.GlobalEventManager.OnHitEnemy += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdcI4(x, 8),
                x => ILPatternMatchingExt.MatchCall<DotController>(x, "GetDotDef"),
                x => ILPatternMatchingExt.MatchStloc(x, 26)
            })) {
                ilcursor.RemoveRange(12);
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.EmitDelegate(delegate (DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) {
                    DotController.DotDef dotDef = DotController.GetDotDef(DotController.DotIndex.Fracture);
                    float damageMultiplier = damageInfo.damage * (attackerBody.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex) ? 0.3f : 0.15f);
                    if (damageInfo.crit) {
                        damageMultiplier *= attackerBody.critMultiplier;
                    }
                    float num = attackerBody.damage * (dotDef.damageCoefficient / dotDef.interval);
                    float num2 = damageMultiplier / dotDef.interval;
                    DotController.InflictDot(victim, attackerBody.gameObject, DotController.DotIndex.Fracture, dotDef.interval, num2 / num, null);
                });
            } else {
                Main.logger_.LogError("FractureOnHit Hook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Items).GetField("Missile")))
            && ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdcR4(x, 10f),
                x => ILPatternMatchingExt.MatchLdarg(x, 1),
            })) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 25f);
            } else {
                Main.logger_.LogError("AtgMissile :: ProcChanceHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdcR4(x, 3f),
                x => ILPatternMatchingExt.MatchLdloc(x, 32),
            })) {
                ilcursor.RemoveRange(25);
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 32);
                ilcursor.EmitDelegate(delegate (DamageInfo damageInfo, GameObject victim, int itemCount) {
                    AtgMissileMK_1Pool atgMissileMK_1Pool = damageInfo.attacker.GetComponent<AtgMissileMK_1Pool>();
                    if (atgMissileMK_1Pool) {
                        atgMissileMK_1Pool.AddMissile(Util.OnHitProcDamage(damageInfo.damage, 0, 2.5f * itemCount), damageInfo.crit, victim, damageInfo.procChainMask);
                    } else {
                        damageInfo.attacker.AddComponent<AtgMissileMK_1Pool>().AddMissile(Util.OnHitProcDamage(damageInfo.damage, 0, 2.5f * itemCount), damageInfo.crit, victim, damageInfo.procChainMask);
                    }
                });
            } else {
                Main.logger_.LogError("AtgMissile :: FireHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
               x => ILPatternMatchingExt.MatchLdcR4(x, 25f),
               x => ILPatternMatchingExt.MatchStloc(x, 50),
            })) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldloc, 49);
                ilcursor.EmitDelegate((int count) => 20f + 5f * count);
            } else {
                Main.logger_.LogError("Ukulele :: ProcChanceHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdloc(x, 49),
                x => ILPatternMatchingExt.MatchMul(x),
                x => ILPatternMatchingExt.MatchStfld<LightningOrb>(x, "bouncesRemaining"),
            })) {
                ilcursor.Index += 2;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4, 4);
            } else {
                Main.logger_.LogError("Ukulele :: TargetCountHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdloc(x, 49),
                x => ILPatternMatchingExt.MatchMul(x),
                x => ILPatternMatchingExt.MatchConvR4(x),
                x => ILPatternMatchingExt.MatchAdd(x),
                x => ILPatternMatchingExt.MatchStfld<LightningOrb>(x, "range"),
            })) {
                ilcursor.Index += 4;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldloc, 49);
                ilcursor.EmitDelegate((int count) => 16f + 4f * count);
            } else {
                Main.logger_.LogError("Ukulele :: RangeHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdcR4(x, 25f),
                x => ILPatternMatchingExt.MatchStloc(x, 56),
            })) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldloc, 55);
                ilcursor.EmitDelegate((int count) => 20f + 5f * count);
            } else {
                Main.logger_.LogError("Polylute :: ProcChanceHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<VoidLightningOrb>(x, "totalStrikes"))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4, 3);
            } else {
                Main.logger_.LogError("Polylute :: StrikeCountHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Items).GetField("FireRing")))) {
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 10f))) {
                    ilcursor.Emit(OpCodes.Ldloc, 80);
                    ilcursor.Emit(OpCodes.Ldloc, 81);
                    ilcursor.EmitDelegate((int iceCount, int fireCount) => Mathf.Min(Mathf.Min(fireCount, iceCount), 9f));
                    ilcursor.Emit(OpCodes.Sub);
                } else {
                    Main.logger_.LogError("IceRing & FireRing :: CoolDownHook Failed!");
                }
                if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 2.5f))) {
                    ilcursor.Remove();
                    ilcursor.Emit(OpCodes.Ldc_R4, 3f);
                } else {
                    Main.logger_.LogError("IceRing :: DamageHook Failed!");
                }
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 20f))) {
                    ilcursor.Emit(OpCodes.Ldloc, 97);
                    ilcursor.EmitDelegate((int voidRingCount) => Mathf.Min(voidRingCount, 18f));
                    ilcursor.Emit(OpCodes.Sub);
                } else {
                    Main.logger_.LogError("VoidRing :: CoolDownHook Failed!");
                }
            } else {
                Main.logger_.LogError("Rings :: Hooks Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("FireballsOnHit") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Meatball) && Util.CheckRoll(20f * damageInfo.procCoefficient, attackerMaster)) {
                        FireFountain fireFountain = victim.GetComponent<FireFountain>();
                        if (fireFountain) {
                            fireFountain.AddProjectile(fireMeatBallProjectile, damageInfo.attacker, Util.OnHitProcDamage(damageInfo.damage, 0, 4 * itemCount), damageInfo.crit, damageInfo.procChainMask);
                        } else {
                            victim.AddComponent<FireFountain>().AddProjectile(fireMeatBallProjectile, damageInfo.attacker, Util.OnHitProcDamage(damageInfo.damage, 0, 4 * itemCount), damageInfo.crit, damageInfo.procChainMask);
                        }
                    }
                    return 0;
                });
            } else {
                Main.logger_.LogError("FireballsOnHit :: Hook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("LightningStrikeOnHit") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit) && Util.CheckRoll(20f * damageInfo.procCoefficient, attackerMaster)) {
                        LightningFountain lightningFountain = victim.GetComponent<LightningFountain>();
                        if (lightningFountain) {
                            lightningFountain.AddProjectile(electricOrbProjectile, damageInfo.attacker, Util.OnHitProcDamage(damageInfo.damage, 0, 4 * itemCount), damageInfo.crit, damageInfo.procChainMask);
                        } else {
                            victim.AddComponent<LightningFountain>().AddProjectile(electricOrbProjectile, damageInfo.attacker, Util.OnHitProcDamage(damageInfo.damage, 0, 4 * itemCount), damageInfo.crit, damageInfo.procChainMask);
                        }
                    }
                    return 0;
                });
            } else {
                Main.logger_.LogError("LightningStrikeOnHit :: Hook Failed!");
            }
        };

        private void IL_TakeDamageHooks() => IL.RoR2.HealthComponent.TakeDamage += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.Match(x, OpCodes.Brtrue),
                    x => ILPatternMatchingExt.MatchLdarg(x, 0),
                    x => ILPatternMatchingExt.MatchLdfld(x, typeof(HealthComponent).GetFieldCached("body")),
                    x => ILPatternMatchingExt.MatchLdsfld(x, typeof(DLC1Content.Buffs).GetFieldCached("BearVoidReady")),
                })) {  // 113
                ilcursor.Index += 10;
                ilcursor.RemoveRange(45);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo) {
                    if (healthComponent.body.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex) || Util.CheckRoll(50f)) {
                        EffectData effectData = new() {
                            origin = damageInfo.position,
                            rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                        };
                        EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab, effectData, true);
                        damageInfo.rejected = true;
                    }
                    healthComponent.body.RemoveBuff(DLC1Content.Buffs.BearVoidReady.buffIndex);
                    int itemCount = healthComponent.body.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                    healthComponent.body.AddTimedBuff(DLC1Content.Buffs.BearVoidCooldown, 15f * Mathf.Pow(0.9f, itemCount));
                });
            } else {
                Main.logger_.LogError("BearVoid Hook Failed!");
            }
            //====================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                   x => ILPatternMatchingExt.MatchLdloc(x, 20),
                   x => ILPatternMatchingExt.MatchLdcI4(x, 0),
                   x => ILPatternMatchingExt.Match(x, OpCodes.Ble),
                })) {  //378
                ilcursor.Index += 3;
                ilcursor.RemoveRange(71);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 20);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, int itemCount) {
                    CharacterBody victimBody = healthComponent.body;
                    if (victimBody.HasBuff(DLC1Content.Buffs.StrongerBurn.buffIndex) || victimBody.HasBuff(RoR2Content.Buffs.OnFire.buffIndex)) {
                        return;
                    }
                    CharacterBody attacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    Vector3 corePosition = victimBody.corePosition;
                    GameObject gameObject = UnityEngine.Object.Instantiate(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, corePosition, Quaternion.identity);
                    DelayBlast component = gameObject.GetComponent<DelayBlast>();
                    component.position = corePosition;
                    component.baseDamage = Util.OnKillProcDamage(attacterBody.damage, 2.6f + 1.6f * (itemCount - 1));
                    component.baseForce = 1000f;
                    component.radius = 16f;
                    component.attacker = damageInfo.attacker;
                    component.inflictor = null;
                    component.crit = Util.CheckRoll(attacterBody.crit, attacterBody.master);
                    component.maxTimer = 0.2f;
                    component.damageColorIndex = DamageColorIndex.Void;
                    component.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                    gameObject.GetComponent<TeamFilter>().teamIndex = attacterBody.teamComponent.teamIndex;
                    NetworkServer.Spawn(gameObject);
                    InflictDotInfo inflictDotInfo = new() {
                        attackerObject = damageInfo.attacker,
                        damageMultiplier = 1,
                        dotIndex = DotController.DotIndex.Burn,
                        duration = 100,
                        totalDamage = healthComponent.fullHealth,
                        victimObject = healthComponent.gameObject,
                    };
                    if (attacterBody.inventory) {
                        StrengthenBurnUtils.CheckDotForUpgrade(attacterBody.inventory, ref inflictDotInfo);
                    }
                    DotController.InflictDot(ref inflictDotInfo);
                });
            } else {
                Main.logger_.LogError("ExplodeOnDeathVoid Hook Failed!");
            }
            //====================================
            ////if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
            ////        x => ILPatternMatchingExt.MatchLdcR4(x, 100f),
            ////        x => ILPatternMatchingExt.MatchLdarg(x, 1),
            ////        x => ILPatternMatchingExt.MatchLdfld<DamageInfo>(x, "procCoefficient"),
            ////    })) {  // 566
            ////    ++ilcursor.Index;
            ////    ilcursor.Emit(OpCodes.Ldc_R4, 20f);
            ////} else {
            ////    Main.logger_.LogError("PermanentDebuffOnHit Hook Failed!");
            ////}
            //====================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 32))) {  // 665
                ilcursor.Index -= 7;
                ilcursor.RemoveRange(2);
                ilcursor.EmitDelegate((HealthComponent healthComponent) => (healthComponent.barrier + healthComponent.shield) > 0);
            } else {
                Main.logger_.LogError("BossDamageBonus Hook Failed!");
            }
            //====================================
            if (ilcursor.TryGotoNext(new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdarg(x, 0),
                    x => ILPatternMatchingExt.MatchLdarg(x, 0),
                    x => ILPatternMatchingExt.MatchLdflda<HealthComponent>(x, "itemCounts"),
                    x => ILPatternMatchingExt.MatchLdfld(x, "RoR2.HealthComponent/ItemCounts", "parentEgg"),
                })) {
                ilcursor.RemoveRange(19);
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, float damage) {
                    healthComponent.Heal(healthComponent.itemCounts.parentEgg * (damage * 0.01f + 20f), default, true);
                    EntitySoundManager.EmitSoundServer(LegacyResourcesAPI.Load<NetworkSoundEventDef>("NetworkSoundEventDefs/nseParentEggHeal").index, healthComponent.gameObject);
                });
            } else {
                Main.logger_.LogError("parentEgg hook error");
            }
            //====================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(HealthComponent.AssetReferences).GetFieldCached("critGlassesVoidExecuteEffectPrefab")))) {  // 1359
                ilcursor.Index -= 2;
                ilcursor.RemoveRange(10);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.EmitDelegate((HealthComponent healthComponent) => healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex));
            } else {
                Main.logger_.LogError("critGlassesVoid Hook Failed!");
            }
        };

        private void LunarSunHook() => On.RoR2.LunarSunBehavior.FixedUpdate += delegate (On.RoR2.LunarSunBehavior.orig_FixedUpdate orig, LunarSunBehavior self) {
            self.projectileTimer += Time.fixedDeltaTime;
            if (!self.body.master.IsDeployableLimited(DeployableSlot.LunarSunBomb) && self.projectileTimer > 3f / self.stack) {
                self.projectileTimer = 0f;
                FireProjectileInfo fireProjectileInfo = new() {
                    projectilePrefab = self.projectilePrefab,
                    crit = self.body.RollCrit(),
                    damage = self.body.damage * 3.6f,
                    damageColorIndex = DamageColorIndex.Item,
                    force = 0f,
                    owner = self.gameObject,
                    position = self.body.transform.position,
                    rotation = Quaternion.identity
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            self.transformTimer += Time.fixedDeltaTime;
            if (self.transformTimer > 30f) {
                self.transformTimer = 0f;
                if (self.body.master && self.body.inventory) {
                    Inventory inventory = self.body.inventory;
                    ItemIndex itemIndex = ItemIndex.None;
                    Util.ShuffleList(inventory.itemAcquisitionOrder, self.transformRng);
                    foreach (ItemIndex itemIndex2 in inventory.itemAcquisitionOrder) {
                        if (itemIndex2 != DLC1Content.Items.LunarSun.itemIndex) {
                            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex2);
                            if (itemDef && itemDef.tier != ItemTier.NoTier && itemDef.DoesNotContainTag(ItemTag.CannotSteal)) {
                                itemIndex = itemIndex2;
                                break;
                            }
                        }
                    }
                    if (itemIndex != ItemIndex.None) {
                        inventory.RemoveItem(itemIndex);
                        inventory.GiveItem(DLC1Content.Items.LunarSun.itemIndex);
                        CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, itemIndex, DLC1Content.Items.LunarSun.itemIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
                    }
                }
            }
        };

        private void MissileVoidHook() => IL.RoR2.Orbs.MissileVoidOrb.Begin += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 75f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 150f);
            } else {
                Main.logger_.LogError("MissileVoidOrb Hook Failed!");
            }
        };

        private void RandomlyLunarHook() {
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacement += delegate (On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacement orig, PickupIndex pickupIndex, Xoroshiro128Plus rng) {
                if (位于虚空之境) {
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    if (pickupDef != null && Util.CheckRoll(5)) {
                        switch (pickupDef.itemTier) {
                            case ItemTier.Tier1: {
                                return rng.NextElementUniform(Run.instance.availableVoidTier1DropList);
                            }
                            case ItemTier.Tier2: {
                                return rng.NextElementUniform(Run.instance.availableVoidTier2DropList);
                            }
                            case ItemTier.Tier3: {
                                return rng.NextElementUniform(Run.instance.availableVoidTier3DropList);
                            }
                        }
                    }
                }
                return pickupIndex;
            };
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacementUniqueArray += delegate (On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacementUniqueArray orig, PickupIndex[] pickupIndices, Xoroshiro128Plus rng) {
                if (位于虚空之境) {
                    List<PickupIndex> list = null;
                    for (int i = 0; i < pickupIndices.Length; ++i) {
                        PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndices[i]);
                        if (pickupDef != null && Util.CheckRoll(5)) {
                            List<PickupIndex> list3 = null;
                            if (pickupDef.itemIndex != ItemIndex.None) {
                                if (list == null) {
                                    switch (pickupDef.itemTier) {
                                        case ItemTier.Tier1: {
                                            list = Run.instance.availableVoidTier1DropList;
                                            break;
                                        }
                                        case ItemTier.Tier2: {
                                            list = Run.instance.availableVoidTier2DropList;
                                            break;
                                        }
                                        case ItemTier.Tier3: {
                                            list = Run.instance.availableVoidTier3DropList;
                                            break;
                                        }
                                    }
                                    Util.ShuffleList(list, rng);
                                }
                                list3 = list;
                            }
                            if (list3 != null && list3.Count > 0) {
                                pickupIndices[i] = list3[i % list3.Count];
                            }
                        }
                    }
                }
            };
            On.RoR2.PurchaseInteraction.SetAvailable += delegate (On.RoR2.PurchaseInteraction.orig_SetAvailable orig, PurchaseInteraction self, bool newAvailable) {
                if (self.name.StartsWith("LunarRecycler")) {
                    if (!newAvailable) {
                        ++RerolledCount;
                    }
                    self.Networkcost = self.cost = RerolledCount + 1;
                    if (RerolledCount >= 9 + 3 * Util.GetItemCountGlobal(DLC1Content.Items.RandomlyLunar.itemIndex, false, false)) {
                        newAvailable = false;
                    }
                }
                orig(self, newAvailable);
            };
        }

        private void RepeatHealHook() {
            IL.RoR2.HealthComponent.Heal += delegate (ILContext il) {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<HealthComponent.RepeatHealComponent>(x, "healthFractionToRestorePerSecond"))) {
                    --cursor.Index;
                    cursor.Remove();
                    cursor.Emit(OpCodes.Mul);
                } else {
                    Main.logger_.LogError("尸爆 Hook Failed!");
                }
            };
        }
    }
}