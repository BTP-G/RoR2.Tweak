using BtpTweak.IndexCollections;
using BtpTweak.MissilePools;
using BtpTweak.OrbPools;
using BtpTweak.ProjectileFountains;
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
        private int _RerolledCount;

        public override void AddHooks() {
            base.AddHooks();
            ExecuteLowHealthEliteHook();
            FireworkHook();
            IgniteOnKillHook();
            IL_OnCharacterDeathHooks();
            IL_OnHitEnemyHooks();
            IL_TakeDamageHooks();
            LunarSunHook();
            RandomlyLunarHook();
            RepeatHealHook();
        }

        public override void Load() {
            base.Load();
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ElementalRingVoidBlackHole").AddComponent<ElementalRingVoidBlackHoleAction>();
            PickupDropletController.pickupDropletPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(5f);
            GenericPickupController.pickupPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(100f);
            "RoR2/Base/BonusGoldPackOnKill/BonusMoneyPack.prefab".LoadComponentInChildren<GravitatePickup>().maxSpeed = 50;
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
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectAragonite ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBackup ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBarrier ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlackHole ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlighted ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlue ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBuffered ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectEarth ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectGold ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectHaunted ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectLunar ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectMoney ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNight ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNullifier ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectOppressive ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPlated ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPoison ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPurity ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRealgar ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRed ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSanguine ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSepia ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVeiled ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVoid ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWarped ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWater ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWhite ?? JunkContent.Items.SkullCounter);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, "RoR2/DLC1/LunarWings/LunarWings.asset".Load<ItemDef>());
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            RoR2Content.Items.Firework.tags = new ItemTag[] { ItemTag.Damage };
            RoR2Content.Items.FlatHealth.tags = new ItemTag[] { ItemTag.Healing };
            RoR2Content.Items.BounceNearby.deprecatedTier = ItemTier.NoTier;
            RoR2Content.Items.Clover.deprecatedTier = ItemTier.NoTier;
            DLC1Content.Items.CloverVoid.deprecatedTier = ItemTier.NoTier;
            RoR2Content.Items.LunarBadLuck.deprecatedTier = ItemTier.NoTier;
        }

        public override void StageStartAction(Stage stage) {
            base.StageStartAction(stage);
            _RerolledCount = 0;
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
                Main.Logger.LogError("ExecuteLowHealthElite Hook Failed!");
            }
        };

        private void FireworkHook() {
            IL.RoR2.GlobalEventManager.OnInteractionBegin += delegate (ILContext il) {
                ILCursor ilcursor = new(il);
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchStloc(x, 6))) {
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                    ilcursor.Emit(OpCodes.Stloc, 6);
                } else {
                    Main.Logger.LogError("Firework :: DisableOriginalActionHook Failed!");
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
                    Main.Logger.LogError("Firework :: FireHook Failed!");
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
                Main.Logger.LogError("IgniteOnKill RadiusHook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 0.75f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 1.5f);
            } else {
                Main.Logger.LogError("IgniteOnKill DamageHook Failed!");
            }
        };

        private void IL_OnCharacterDeathHooks() => IL.RoR2.GlobalEventManager.OnCharacterDeath += delegate (ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 53))) {
                ilcursor.Index -= 10;
                ilcursor.RemoveRange(10);
                ilcursor.Emit(OpCodes.Ldc_R4, 4f);
            } else {
                Main.Logger.LogError("ExplodeOnDeath DamageHook Failed!");
            }
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 2.4f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 6f);
            } else {
                Main.Logger.LogError("ExplodeOnDeath RadiusHook Failed!");
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
                    int itemCountAll = Util.GetItemCountForTeam(attackerBody.teamComponent.teamIndex, RoR2Content.Items.Infusion.itemIndex, true, false);
                    if (inventory.infusionBonus < Convert.ToUInt64(attackerBody.level * attackerBody.levelMaxHealth * itemCountAll)) {
                        InfusionOrb infusionOrb = new() {
                            origin = pos,
                            target = attackerBody.mainHurtBox,
                            maxHpValue = itemCountAll
                        };
                        OrbManager.instance.AddOrb(infusionOrb);
                    }
                    var ownerBody = attackerBody.master?.minionOwnership.ownerMaster?.GetBody();
                    if (ownerBody && ownerBody.inventory.infusionBonus < Convert.ToUInt64(ownerBody.level * ownerBody.levelMaxHealth * itemCountAll)) {
                        InfusionOrb infusionOrb = new() {
                            origin = pos,
                            target = ownerBody.mainHurtBox,
                            maxHpValue = itemCountAll
                        };
                        OrbManager.instance.AddOrb(infusionOrb);
                    }
                });
            } else {
                Main.Logger.LogError("Infusion Hook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/NetworkedObjects/BonusMoneyPack"))) {
                ilcursor.RemoveRange(15);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 18);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) {
                    GameObject BonusMoneyPack = UnityEngine.Object.Instantiate(AssetReferences.bonusMoneyPack, pos, UnityEngine.Random.rotation);
                    TeamFilter TeamFilter = BonusMoneyPack.GetComponent<TeamFilter>();
                    if (TeamFilter) {
                        TeamFilter.teamIndex = attacterTeamindex;
                        BonusMoneyPack.GetComponentInChildren<GravitatePickup>().gravitateTarget = attacterBody.coreTransform;
                    }
                    NetworkServer.Spawn(BonusMoneyPack);
                });
            } else {
                Main.Logger.LogError("BonusGoldPackOnKill Hook Failed!");
            }
            //======
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 51))) {
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate(delegate (int count, CharacterBody attackerBody, CharacterBody victimBody) {
                    if (count > 0) {
                        int bleedBuffCount = victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) + 1000 * victimBody.GetBuffCount(RoR2Content.Buffs.SuperBleed.buffIndex);
                        if (bleedBuffCount > 0) {
                            Util.PlaySound("Play_bleedOnCritAndExplode_explode", victimBody.gameObject);
                            GameObject bleedExplode = UnityEngine.Object.Instantiate(GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect, victimBody.corePosition, Quaternion.identity);
                            DelayBlast delayBlast = bleedExplode.GetComponent<DelayBlast>();
                            delayBlast.position = victimBody.corePosition;
                            delayBlast.baseDamage = Util.OnKillProcDamage(attackerBody.damage, count * (400 + bleedBuffCount) * 0.01f);
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
                    }
                    return 0;
                });
            } else {
                Main.Logger.LogError("ShatterSpleen :: BleedExplodeHook Failed!");
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
                    float damageMultiplier = damageInfo.damage * 0.3f;
                    if (damageInfo.crit) {
                        damageMultiplier *= attackerBody.critMultiplier;
                    }
                    damageMultiplier /= attackerBody.damage * dotDef.damageCoefficient;
                    DotController.InflictDot(victim, attackerBody.gameObject, DotController.DotIndex.Fracture, dotDef.interval, damageMultiplier, null);
                });
            } else {
                Main.Logger.LogError("FractureOnHit Hook Failed!");
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
                Main.Logger.LogError("AtgMissile :: ProcChanceHook Failed!");
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
                Main.Logger.LogError("AtgMissile :: FireHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("ChainLightning") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && Util.CheckRoll((20 + 5 * itemCount) * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<LightningOrbPool>() ?? victim.AddComponent<LightningOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 0.8f),
                            isCrit = damageInfo.crit,
                            bouncesRemaining = 4,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            bouncedObjects = new List<HealthComponent> { victim.GetComponent<HealthComponent>() },
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.2f,
                            lightningType = LightningOrb.LightningType.Ukulele,
                            damageColorIndex = DamageColorIndex.Item,
                            range = 20 + 4 * itemCount
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightning :: Hook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(DLC1Content.Items).GetField("ChainLightningVoid") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && Util.CheckRoll((20 + 5 * itemCount) * damageInfo.procCoefficient, attackerMaster)) {
                        VoidLightningOrb voidLightningOrb = new() {
                            origin = damageInfo.position,
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 0.6f),
                            isCrit = damageInfo.crit,
                            totalStrikes = 3,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.2f,
                            damageColorIndex = DamageColorIndex.Void,
                            secondsPerStrike = 0.1f
                        };
                        HurtBox target = victimBody.mainHurtBox;
                        if (target) {
                            voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                            voidLightningOrb.target = target;
                            OrbManager.instance.AddOrb(voidLightningOrb);
                        }
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("StickyBomb") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (Util.CheckRoll(5 * itemCount * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<StickyBombFountain>() ?? victim.AddComponent<StickyBombFountain>()).AddProjectile(
                            AssetReferences.stickyBombProjectile,
                            damageInfo.attacker,
                            Util.OnHitProcDamage(damageInfo.damage, 0, 1.8f),
                            damageInfo.crit,
                            damageInfo.procChainMask);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("StickyBomb :: ForceHook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Items).GetField("FireRing")))) {
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 10f))) {
                    ilcursor.Emit(OpCodes.Ldloc, 80);
                    ilcursor.Emit(OpCodes.Ldloc, 81);
                    ilcursor.EmitDelegate((int iceCount, int fireCount) => Mathf.Min(Mathf.Min(fireCount, iceCount), 9f));
                    ilcursor.Emit(OpCodes.Sub);
                } else {
                    Main.Logger.LogError("IceRing & FireRing :: CoolDownHook Failed!");
                }
                if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 2.5f))) {
                    ilcursor.Remove();
                    ilcursor.Emit(OpCodes.Ldc_R4, 3f);
                } else {
                    Main.Logger.LogError("IceRing :: DamageHook Failed!");
                }
                if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 20f))) {
                    ilcursor.Emit(OpCodes.Ldloc, 97);
                    ilcursor.EmitDelegate((int voidRingCount) => Mathf.Min(voidRingCount, 18f));
                    ilcursor.Emit(OpCodes.Sub);
                } else {
                    Main.Logger.LogError("VoidRing :: CoolDownHook Failed!");
                }
            } else {
                Main.Logger.LogError("Rings :: Hooks Failed!");
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
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Meatball) && Util.CheckRoll(30f * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<FireFountain>() ?? victim.AddComponent<FireFountain>()).AddProjectile(
                            AssetReferences.fireMeatBallProjectile,
                            damageInfo.attacker,
                            Util.OnHitProcDamage(damageInfo.damage, 0, 3 * itemCount),
                            damageInfo.crit,
                            damageInfo.procChainMask);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("FireballsOnHit :: Hook Failed!");
            }
            //============================================================================
            if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("LightningStrikeOnHit") ),
                x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
            })) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit) && Util.CheckRoll(30f * damageInfo.procCoefficient, attackerMaster)) {
                        (victimBody.GetComponent<SimpleLightningStrikeOrbPool>() ?? victimBody.AddComponent<SimpleLightningStrikeOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                            attacker = damageInfo.attacker,
                            damageColorIndex = DamageColorIndex.Item,
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 3 * itemCount),
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.5f,
                            target = victimBody.mainHurtBox,
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("LightningStrikeOnHit :: Hook Failed!");
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
                Main.Logger.LogError("BearVoid Hook Failed!");
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
                    component.baseDamage = Util.OnKillProcDamage(attacterBody.damage, 3.6f);
                    component.baseForce = 1000f;
                    component.radius = 12 + 3 * itemCount;
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
                        damageMultiplier = itemCount,
                        dotIndex = DotController.DotIndex.Burn,
                        totalDamage = healthComponent.fullHealth,
                        victimObject = healthComponent.gameObject,
                    };
                    if (attacterBody.inventory) {
                        StrengthenBurnUtils.CheckDotForUpgrade(attacterBody.inventory, ref inflictDotInfo);
                    }
                    DotController.InflictDot(ref inflictDotInfo);
                });
            } else {
                Main.Logger.LogError("ExplodeOnDeathVoid Hook Failed!");
            }
            //====================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 32))) {  // 665
                ilcursor.Index -= 7;
                ilcursor.RemoveRange(2);
                ilcursor.EmitDelegate((HealthComponent healthComponent) => (healthComponent.barrier + healthComponent.shield) > 0);
            } else {
                Main.Logger.LogError("BossDamageBonus Hook Failed!");
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
                Main.Logger.LogError("parentEgg hook error");
            }
            //====================================
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(HealthComponent.AssetReferences).GetFieldCached("critGlassesVoidExecuteEffectPrefab")))) {  // 1359
                ilcursor.Index -= 2;
                ilcursor.RemoveRange(10);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.EmitDelegate((HealthComponent healthComponent) => healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex));
            } else {
                Main.Logger.LogError("critGlassesVoid Hook Failed!");
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

        private void RandomlyLunarHook() {
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacement += delegate (On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacement orig, PickupIndex pickupIndex, Xoroshiro128Plus rng) {
                if (GlobalInfo.CurrentSceneIndex == SceneIndexCollection.arena) {
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
                if (GlobalInfo.CurrentSceneIndex == SceneIndexCollection.arena) {
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
                        ++_RerolledCount;
                    }
                    int itemCountGlobal = Util.GetItemCountGlobal(DLC1Content.Items.RandomlyLunar.itemIndex, false);
                    self.Networkcost = _RerolledCount + 1 + _RerolledCount * 3 * itemCountGlobal;
                    if (_RerolledCount >= 9 + 3 * itemCountGlobal) {
                        newAvailable = false;
                    }
                }
                orig(self, newAvailable);
            };
        }

        private void RepeatHealHook() {
            IL.RoR2.HealthComponent.Heal += delegate (ILContext il) {
                ILCursor iLCursor = new(il);
                if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<HealthComponent.RepeatHealComponent>(x, "healthFractionToRestorePerSecond"))) {
                    iLCursor.GotoPrev(MoveType.Before, x => ILPatternMatchingExt.MatchLdcR4(x, 0.1f));
                    iLCursor.Remove();
                    iLCursor.Emit(OpCodes.Ldc_R4, 0.5f);
                } else {
                    Main.Logger.LogError("RepeatHeal Hook Failed!");
                }
            };
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileExplosion))]
        [RequireComponent(typeof(RadialForce))]
        [RequireComponent(typeof(ProjectileFuse))]
        private class ElementalRingVoidBlackHoleAction : MonoBehaviour {
            private float timer;

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void FixedUpdate() {
                if ((timer -= Time.fixedDeltaTime) <= 0) {
                    List<HurtBox> hurtBoxes = new();
                    GetComponent<RadialForce>().SearchForTargets(hurtBoxes);
                    GetComponent<ProjectileExplosion>().blastDamageCoefficient += hurtBoxes.Count;
                    enabled = false;
                }
            }

            private void Start() {
                timer = GetComponent<ProjectileFuse>().fuse - 0.025f;
            }
        }
    }
}