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
            NovaOnHealHook();
            RandomlyLunarHook();
            RemoveOrigSawBleedOnHitHook();
            RepeatHealHook();
            SiphonOnLowHealthHook();
        }

        public override void Load() {
            base.Load();
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ElementalRingVoidBlackHole").AddComponent<ElementalRingVoidBlackHoleAction>();
            PickupDropletController.pickupDropletPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(5f);
            GenericPickupController.pickupPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(100f);
            "RoR2/Base/BonusGoldPackOnKill/BonusMoneyPack.prefab".LoadComponentInChildren<GravitatePickup>().maxSpeed = 50;
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.NovaOnHeal);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ShockNearby);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.BearVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.MinorConstructOnKill);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.BeetleGland);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ExecuteLowHealthElite);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.RoboBallBuddy);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ShockNearby);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Thorns);
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectAragonite) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectAragonite);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBackup) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBackup);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBarrier) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBarrier);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBlackHole) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlackHole);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBlighted) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlighted);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBlue) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlue);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectBuffered) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBuffered);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectEarth) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectEarth);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectGold) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectGold);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectHaunted) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectHaunted);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectLunar) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectLunar);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectMoney) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectMoney);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectNight) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNight);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectNullifier) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNullifier);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectOppressive) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectOppressive);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectPlated) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPlated);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectPoison) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPoison);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectPurity) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPurity);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectRealgar) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRealgar);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectRed) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRed);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectSanguine) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSanguine);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectSepia) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSepia);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectVeiled) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVeiled);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectVoid) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVoid);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectWarped) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWarped);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectWater) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWater);
            }
            if (TPDespair.ZetAspects.Catalog.Item.ZetAspectWhite) {
                R2API.ItemAPI.ApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWhite);
            }
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            RoR2Content.Items.Firework.tags = new ItemTag[] { ItemTag.Damage };
            RoR2Content.Items.FlatHealth.tags = new ItemTag[] { ItemTag.Healing };
        }

        public override void StageStartAction(Stage stage) {
            base.StageStartAction(stage);
            _RerolledCount = 0;
        }

        private void ExecuteLowHealthEliteHook() => IL.RoR2.CharacterBody.OnInventoryChanged += delegate (ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(new Func<Instruction, bool>[] {
                x => ILPatternMatchingExt.MatchLdarg(x, 0),
                x => ILPatternMatchingExt.MatchLdcR4(x, 13f),
                x => ILPatternMatchingExt.MatchLdarg(x, 0),
            })) {
                ++cursor.Index;
                cursor.RemoveRange(11);
                cursor.EmitDelegate(delegate (CharacterBody body) {
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
                    ilcursor.EmitDelegate((DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                        int itemCount = attackerBody.inventory.GetItemCount(RoR2Content.Items.Firework);
                        if (Util.CheckRoll(5 * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                            (damageInfo.attacker.GetComponent<FireworkPool>() ?? damageInfo.attacker.AddComponent<FireworkPool>()).AddMissile(
                                Util.OnHitProcDamage(damageInfo.damage, 0, 1f),
                                damageInfo.crit,
                                victim,
                                damageInfo.procChainMask);
                        }
                    });
                } else {
                    Main.Logger.LogError("Firework :: FireHook Failed!");
                }
            };
        }

        private void IgniteOnKillHook() {
            On.RoR2.GlobalEventManager.ProcIgniteOnKill += (On.RoR2.GlobalEventManager.orig_ProcIgniteOnKill orig, DamageReport damageReport, int igniteOnKillCount, CharacterBody victimBody, TeamIndex attackerTeamIndex) => {
                var attackerBody = damageReport.attackerBody;
                var blastAttack = new BlastAttack {
                    attacker = damageReport.attacker,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = Util.OnKillProcDamage(attackerBody.damage, 1.5f),
                    crit = attackerBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.AOE,
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    position = damageReport.damageInfo.position,
                    procCoefficient = damageReport.damageInfo.procCoefficient,
                    radius = 8 + 4 * igniteOnKillCount + 0.4f * victimBody.bestFitRadius,
                    teamIndex = attackerTeamIndex,
                };
                EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData {
                    origin = blastAttack.position,
                    scale = blastAttack.radius,
                    rotation = default,
                }, true);
                var result = blastAttack.Fire();
                if (result.hitCount > 0) {
                    var baseInflictDotInfo = new InflictDotInfo {
                        attackerObject = damageReport.attacker,
                        totalDamage = attackerBody.damage * 0.75f * igniteOnKillCount,
                        dotIndex = DotController.DotIndex.Burn,
                        damageMultiplier = 1f
                    };
                    StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.inventory, ref baseInflictDotInfo);
                    foreach (var hitPoint in result.hitPoints) {
                        var healthComponent = hitPoint.hurtBox.healthComponent;
                        if (healthComponent.alive) {
                            InflictDotInfo inflictDotInfo = baseInflictDotInfo;
                            inflictDotInfo.victimObject = healthComponent.gameObject;
                            DotController.InflictDot(ref inflictDotInfo);
                        }
                    }
                }
            };
        }

        private void IL_OnCharacterDeathHooks() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += (ILContext il) => {
                ILCursor ilcursor = new(il);
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("ExplodeOnDeath")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc_2);
                    ilcursor.EmitDelegate(delegate (int itemCount, DamageReport damageReport, CharacterBody victimBody) {
                        if (itemCount > 0) {
                            GameObject explodeOnDeath = UnityEngine.Object.Instantiate(GlobalEventManager.CommonAssets.explodeOnDeathPrefab, victimBody.corePosition, Quaternion.identity);
                            explodeOnDeath.GetComponent<TeamFilter>().teamIndex = damageReport.attackerTeamIndex;
                            DelayBlast delayBlast = explodeOnDeath.GetComponent<DelayBlast>();
                            delayBlast.attacker = damageReport.attacker;
                            delayBlast.baseDamage = Util.OnKillProcDamage(damageReport.attackerBody.damage, 3 * itemCount);
                            delayBlast.crit = damageReport.attackerBody.RollCrit();
                            delayBlast.position = damageReport.damageInfo.position;
                            delayBlast.procCoefficient = damageReport.damageInfo.procCoefficient;
                            delayBlast.radius = 12f + 1.2f * victimBody.bestFitRadius;
                            NetworkServer.Spawn(explodeOnDeath);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("ExplodeOnDeath :: Hook Failed!");
                }
                //======
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("Infusion")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc, 15);
                    ilcursor.Emit(OpCodes.Ldloc, 17);
                    ilcursor.Emit(OpCodes.Ldloc, 6);
                    ilcursor.EmitDelegate((int itemCount, DamageReport damageReport, CharacterBody attackerBody, Inventory inventory, Vector3 pos) => {
                        int teamItemCount = Util.GetItemCountForTeam(damageReport.attackerTeamIndex, RoR2Content.Items.Infusion.itemIndex, true, false);
                        if (teamItemCount > 0) {
                            if (inventory.infusionBonus < (uint)(attackerBody.level * attackerBody.levelMaxHealth * teamItemCount)) {
                                InfusionOrb infusionOrb = new() {
                                    origin = pos,
                                    target = attackerBody.mainHurtBox,
                                    maxHpValue = teamItemCount
                                };
                                OrbManager.instance.AddOrb(infusionOrb);
                            }
                            var ownerBody = damageReport.attackerOwnerMaster?.GetBody();
                            if (ownerBody && ownerBody.inventory.infusionBonus < (uint)(ownerBody.level * ownerBody.levelMaxHealth * teamItemCount)) {
                                InfusionOrb infusionOrb = new() {
                                    origin = pos,
                                    target = ownerBody.mainHurtBox,
                                    maxHpValue = teamItemCount
                                };
                                OrbManager.instance.AddOrb(infusionOrb);
                            }
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("Infusion :: Hook Failed!");
                }
                //======
                if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/NetworkedObjects/BonusMoneyPack"))) {
                    ilcursor.RemoveRange(15);
                    ilcursor.Emit(OpCodes.Ldloc, 15);
                    ilcursor.Emit(OpCodes.Ldloc, 18);
                    ilcursor.Emit(OpCodes.Ldloc, 6);
                    ilcursor.EmitDelegate((CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) => {
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
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("BleedOnHitAndExplode")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc_2);
                    ilcursor.EmitDelegate((int itemCount, DamageReport damageReport, CharacterBody victimBody) => {
                        if (itemCount > 0 && (victimBody.HasBuff(RoR2Content.Buffs.Bleeding.buffIndex) || victimBody.HasBuff(RoR2Content.Buffs.SuperBleed.buffIndex))) {
                            Util.PlaySound("Play_bleedOnCritAndExplode_explode", victimBody.gameObject);
                            GameObject bleedExplode = UnityEngine.Object.Instantiate(GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect, victimBody.corePosition, Quaternion.identity);
                            bleedExplode.GetComponent<TeamFilter>().teamIndex = damageReport.attackerTeamIndex;
                            DelayBlast delayBlast = bleedExplode.GetComponent<DelayBlast>();
                            delayBlast.attacker = damageReport.attacker;
                            delayBlast.baseDamage = Util.OnKillProcDamage(damageReport.attackerBody.damage, 4 * itemCount);
                            delayBlast.crit = damageReport.attackerBody.RollCrit();
                            delayBlast.position = damageReport.damageInfo.position;
                            delayBlast.procCoefficient = damageReport.damageInfo.procCoefficient;
                            delayBlast.radius = 16f + 1.6f * victimBody.bestFitRadius;
                            NetworkServer.Spawn(bleedExplode);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("BleedOnHitAndExplode :: Hook Failed!");
                }
            };
        }

        private void IL_OnHitEnemyHooks() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += delegate (ILContext il) {
                ILCursor ilcursor = new(il);
                //void ChangeItemAction(string ItemName, bool isDlCItem, Action<DamageInfo, CharacterBody, CharacterBody, TeamIndex> newAction) {
                //    if (ilcursor.TryGotoNext(MoveType.Before, new Func<Instruction, bool>[] {
                //        x => ILPatternMatchingExt.MatchLdsfld(x, isDlCItem ? typeof(DLC1Content.Items) : typeof(RoR2Content.Items), ItemName),
                //        x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                //    })) {
                //        --ilcursor.Index;
                //        ilcursor.RemoveRange(3);
                //        ilcursor.Emit(OpCodes.Ldc_I4_0);
                //    } else {
                //        Main.Logger.LogError($"{ItemName} :: RemoveHook Failed!");
                //    }
                //}
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdcI4(x, 5),
                    x => ILPatternMatchingExt.MatchCall<ProcChainMask>(x, "HasProc"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldarg_2);
                    ilcursor.Emit(OpCodes.Ldloc, 1);
                    ilcursor.Emit(OpCodes.Ldloc, 5);
                    ilcursor.Emit(OpCodes.Ldloc, 0);
                    ilcursor.EmitDelegate((bool hasProc, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody, Inventory inventory, uint? maxStacksFromAttacker) => {
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal) && Util.CheckRoll((10 + 10 * inventory.GetItemCount(RoR2Content.Items.Seed)) * damageInfo.procCoefficient, attackerBody.master)) {
                            DotController.InflictDot(victim, damageInfo.attacker, damageInfo.crit ? DotController.DotIndex.SuperBleed : DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                        }
                        if (hasProc) {
                            return true;
                        }
                        int itemCount = inventory.GetItemCount(RoR2Content.Items.BleedOnHitAndExplode);
                        if (itemCount > 0 && damageInfo.crit) {
                            DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                        }
                        if ((damageInfo.damageType & DamageType.BleedOnHit) != DamageType.Generic) {
                            DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                        }
                        //if ((damageInfo.damageType & DamageType.SuperBleedOnCrit) != DamageType.Generic && damageInfo.crit) {
                        //    DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.SuperBleed, 15f * damageInfo.procCoefficient, 1f, maxStacksFromAttacker);
                        //}
                        if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                            DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                        }
                        if (damageInfo.inflictor?.GetComponent<BoomerangProjectile>() && inventory.currentEquipmentIndex == RoR2Content.Equipment.Saw.equipmentIndex) {
                            DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                        }
                        return true;
                    });
                } else {
                    Main.Logger.LogError("BleedOnHit :: Hook Failed!");
                }
                //=================================================
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
                        DotController.InflictDot(victim, attackerBody.gameObject, DotController.DotIndex.Fracture, 3,
                            Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 0.44f)
                            * (damageInfo.crit ? attackerBody.critMultiplier : 1f)
                            / (attackerBody.damage * 4));
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
                    ilcursor.Emit(OpCodes.Ldloc, 32);
                    ilcursor.EmitDelegate((int itemCount) => {
                        return 100f * (itemCount / (itemCount + 9f));
                    });
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
                        (damageInfo.attacker.GetComponent<AtgMissileMK_1Pool>() ?? damageInfo.attacker.AddComponent<AtgMissileMK_1Pool>()).AddMissile(
                            Util.OnHitProcDamage(damageInfo.damage, 0, 2 * itemCount),
                            damageInfo.crit,
                            victim,
                            damageInfo.procChainMask);
                    });
                } else {
                    Main.Logger.LogError("AtgMissile :: FireHook Failed!");
                }
                //============================================================================
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(DLC1Content.Items).GetField("MissileVoid")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc, 1);
                    ilcursor.Emit(OpCodes.Ldloc, 2);
                    ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) {
                        if (itemCount < 1 || !victimBody.mainHurtBox) {
                            return;
                        }
                        var shieldFraction = attackerBody.healthComponent.shield / attackerBody.healthComponent.fullShield;
                        if (!Util.CheckRoll((33 * itemCount) * shieldFraction * damageInfo.procCoefficient, attackerBody.master)) {
                            return;
                        }
                        var itemMoreMissileCount = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile);
                        var damageValue = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 0.6f * itemCount) * shieldFraction * (itemMoreMissileCount > 1 ? 0.5f * (1 + itemMoreMissileCount) : 1);
                        var procChainMask = damageInfo.procChainMask;
                        procChainMask.AddProc(ProcType.Missile);
                        procChainMask.AddPoolProcs();
                        for (int i = 3 * (itemMoreMissileCount > 0 ? 3 : 1); i > 0; --i) {
                            OrbManager.instance.AddOrb(new MissileVoidOrb() {
                                origin = attackerBody.aimOrigin,
                                damageValue = damageValue,
                                isCrit = damageInfo.crit,
                                teamIndex = attackerBody.teamComponent.teamIndex,
                                attacker = damageInfo.attacker,
                                procChainMask = procChainMask,
                                procCoefficient = 0.2f,
                                damageColorIndex = DamageColorIndex.Void,
                                target = victimBody.mainHurtBox
                            });
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("MissileVoid :: Hook Failed!");
                }
                //============================================================================
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("ChainLightning")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc, 4);
                    ilcursor.Emit(OpCodes.Ldarg_2);
                    ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && Util.CheckRoll(100f * (itemCount / (itemCount + 3f)) * damageInfo.procCoefficient, attackerMaster)) {
                            (victim.GetComponent<LightningOrbPool>() ?? victim.AddComponent<LightningOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                                damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 0.6f),
                                isCrit = damageInfo.crit,
                                bouncesRemaining = 2 + itemCount,
                                teamIndex = attackerMaster.teamIndex,
                                attacker = damageInfo.attacker,
                                bouncedObjects = new List<HealthComponent> { victim.GetComponent<HealthComponent>() },
                                procChainMask = damageInfo.procChainMask,
                                procCoefficient = 0.2f,
                                lightningType = LightningOrb.LightningType.Ukulele,
                                damageColorIndex = DamageColorIndex.Item,
                                range = 18 + 3 * itemCount
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
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && victimBody.mainHurtBox && Util.CheckRoll(100f * (itemCount / (itemCount + 3f)) * damageInfo.procCoefficient, attackerMaster)) {
                            var voidLightningOrb = new VoidLightningOrb() {
                                origin = damageInfo.position,
                                damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 0.4f),
                                isCrit = damageInfo.crit,
                                totalStrikes = 2 + itemCount,
                                teamIndex = attackerMaster.teamIndex,
                                attacker = damageInfo.attacker,
                                procChainMask = damageInfo.procChainMask,
                                procCoefficient = 0.2f,
                                damageColorIndex = DamageColorIndex.Void,
                                secondsPerStrike = 0.1f,
                                target = victimBody.mainHurtBox
                            };
                            voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                            voidLightningOrb.procChainMask.AddPoolProcs();
                            OrbManager.instance.AddOrb(voidLightningOrb);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
                }
                //============================================================================
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x,typeof(RoR2Content.Items).GetField("BounceNearby") ),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {
                    ilcursor.Emit(OpCodes.Ldarg_1);
                    ilcursor.Emit(OpCodes.Ldloc, 4);
                    ilcursor.Emit(OpCodes.Ldloc, 2);
                    ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) {
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.BounceNearby) && Util.CheckRoll((16.5f + 16.5f * itemCount) * damageInfo.procCoefficient, attackerMaster)) {
                            (victimBody.GetComponent<BounceOrbPool>() ?? victimBody.AddComponent<BounceOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                                damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 1f),
                                isCrit = damageInfo.crit,
                                teamIndex = attackerMaster.teamIndex,
                                attacker = damageInfo.attacker,
                                procChainMask = damageInfo.procChainMask,
                                procCoefficient = 0.33f,
                                damageColorIndex = DamageColorIndex.Item,
                            });
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("BounceNearby :: Hook Failed!");
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
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Count) && Util.CheckRoll(5 * itemCount * damageInfo.procCoefficient, attackerMaster)) {
                            (victim.GetComponent<StickyBombFountain>() ?? victim.AddComponent<StickyBombFountain>()).AddProjectile(
                                AssetReferences.stickyBombProjectile,
                                damageInfo.attacker,
                                Util.OnHitProcDamage(damageInfo.damage, 0, 1f),
                                damageInfo.crit,
                                damageInfo.procChainMask);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("StickyBomb :: Hook Failed!");
                }
                //============================================================================
                if (ilcursor.TryGotoNext(MoveType.Before, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdarg(x, 1),
                    x => ILPatternMatchingExt.MatchLdflda<DamageInfo>(x, "procChainMask"),
                    x => ILPatternMatchingExt.MatchLdcI4(x, 12),
                    x => ILPatternMatchingExt.MatchCall<ProcChainMask>(x, "HasProc"),
                })) {
                    ilcursor.Index += 1;
                    ilcursor.RemoveRange(3);
                    ilcursor.Emit(OpCodes.Ldloc, 1);
                    ilcursor.Emit(OpCodes.Ldloc, 5);
                    ilcursor.EmitDelegate((DamageInfo damageInfo, CharacterBody attackerBody, Inventory inventory) => {
                        if (damageInfo.procChainMask.HasProc(ProcType.Rings) || damageInfo.damage < 4f * attackerBody.damage) {
                            return;
                        }
                        if (attackerBody.HasBuff(RoR2Content.Buffs.ElementalRingsReady.buffIndex)) {
                            attackerBody.RemoveBuff(RoR2Content.Buffs.ElementalRingsReady);
                            int iceRingCount = inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                            int fireRingCount = inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                            if (attackerBody.bodyIndex == BodyIndexCollection.MageBody) {
                                attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, 1f);
                            } else {
                                for (int i = Mathf.Max(1, 10 - (iceRingCount > fireRingCount ? fireRingCount : iceRingCount)); i > 0; --i) {
                                    attackerBody.AddTimedBuff(RoR2Content.Buffs.ElementalRingsCooldown, i);
                                }
                            }
                            ProcChainMask ringProcChainMask = damageInfo.procChainMask;
                            ringProcChainMask.AddProc(ProcType.Rings);
                            ringProcChainMask.AddPoolProcs();
                            if (iceRingCount > 0) {
                                var blastAttack = new BlastAttack() {
                                    attacker = damageInfo.attacker,
                                    baseDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 3 * iceRingCount),
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
                                            healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Slow80, 3 * iceRingCount);
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
                                    damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, 0.3f * fireRingCount),
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
                            if (attackerBody.bodyIndex == BodyIndexCollection.VoidSurvivorBody) {
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
                            fireProjectileInfo.procChainMask.AddPoolProcs();
                            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_1);
                } else {
                    Main.Logger.LogError("Rings :: Hook Failed!");
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
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Meatball) && Util.CheckRoll(100f * (itemCount / (itemCount + 9f)) * damageInfo.procCoefficient, attackerMaster)) {
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
                        if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit) && victimBody.mainHurtBox && Util.CheckRoll(100f * (itemCount / (itemCount + 9f)) * damageInfo.procCoefficient, attackerMaster)) {
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
        }

        private void IL_TakeDamageHooks() {
            IL.RoR2.HealthComponent.TakeDamage += delegate (ILContext il) {
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
                        var body = healthComponent.body;
                        if (body.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex) || Util.CheckRoll(50f)) {
                            EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab, new EffectData() {
                                origin = damageInfo.position,
                                rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                            }, true);
                            damageInfo.rejected = true;
                        }
                        body.RemoveBuff(DLC1Content.Buffs.BearVoidReady.buffIndex);
                        int itemCount = body.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                        body.AddTimedBuff(DLC1Content.Buffs.BearVoidCooldown, 15f * Mathf.Pow(0.9f, itemCount));
                    });
                } else {
                    Main.Logger.LogError("BearVoid Hook Failed!");
                }
                //====================================
                if (ilcursor.TryGotoNext(MoveType.After, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdsfld(x, typeof(DLC1Content.Items).GetField("ExplodeOnDeathVoid")),
                    x => ILPatternMatchingExt.MatchCallvirt<Inventory>(x, "GetItemCount"),
                })) {  //378
                    ilcursor.Emit(OpCodes.Ldarg, 0);
                    ilcursor.Emit(OpCodes.Ldarg, 1);
                    ilcursor.Emit(OpCodes.Ldloc, 1);
                    ilcursor.EmitDelegate((int itemCount, HealthComponent healthComponent, DamageInfo damageInfo, CharacterBody attacterBody) => {
                        if (itemCount > 0) {
                            CharacterBody victimBody = healthComponent.body;
                            if (victimBody.HasBuff(RoR2Content.Buffs.OnFire) || victimBody.HasBuff(DLC1Content.Buffs.StrongerBurn)) {
                                return;
                            }
                            GameObject explodeOnDeathVoidExplosion = UnityEngine.Object.Instantiate(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, victimBody.corePosition, Quaternion.identity);
                            explodeOnDeathVoidExplosion.GetComponent<TeamFilter>().teamIndex = attacterBody.teamComponent.teamIndex;
                            float combinedHealthFraction = healthComponent.combinedHealthFraction;
                            DelayBlast delayBlast = explodeOnDeathVoidExplosion.GetComponent<DelayBlast>();
                            delayBlast.attacker = damageInfo.attacker;
                            delayBlast.baseDamage = Util.OnHitProcDamage(damageInfo.damage, 0f, 0.5f * combinedHealthFraction) + (attacterBody.damage * itemCount);
                            delayBlast.crit = damageInfo.crit;
                            delayBlast.position = damageInfo.position;
                            delayBlast.procCoefficient = damageInfo.procCoefficient;
                            delayBlast.radius = 10f + combinedHealthFraction * victimBody.bestFitRadius;
                            NetworkServer.Spawn(explodeOnDeathVoidExplosion);
                            InflictDotInfo inflictDotInfo = new() {
                                attackerObject = damageInfo.attacker,
                                damageMultiplier = 1f,
                                dotIndex = DotController.DotIndex.PercentBurn,
                                totalDamage = healthComponent.combinedHealth,
                                victimObject = healthComponent.gameObject,
                            };
                            DotController.InflictDot(ref inflictDotInfo);
                        }
                    });
                    ilcursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("ExplodeOnDeathVoid Hook Failed!");
                }
                if (ilcursor.TryGotoPrev(MoveType.Before, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdarg(x, 0),
                    x => ILPatternMatchingExt.MatchCall<HealthComponent>(x, "get_fullCombinedHealth"),
                })) {
                    ilcursor.RemoveRange(3);
                    ilcursor.Emit(OpCodes.Pop);
                } else {
                    Main.Logger.LogError("ExplodeOnDeathVoid ProcHook Failed!");
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
        }

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

        private void NovaOnHealHook() {
            IL.RoR2.HealthComponent.ServerFixedUpdate += (ILContext il) => {
                ILCursor iLCursor = new(il);
                if (iLCursor.TryGotoNext(MoveType.Before, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdcR4(x,0.1f),
                    x => ILPatternMatchingExt.MatchAdd(x),
                    x => ILPatternMatchingExt.MatchStfld<HealthComponent>(x,"devilOrbTimer"),
                })) {
                    iLCursor.Remove();
                    iLCursor.Emit(OpCodes.Ldc_R4, 0.666f);
                } else {
                    Main.Logger.LogError("NovaOnHeal Hook Failed!");
                }
            };
        }

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

        private void RemoveOrigSawBleedOnHitHook() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += (ILContext il) => {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(MoveType.Before, new Func<Instruction, bool>[] {
                    x => ILPatternMatchingExt.MatchLdcI4(x, 0),
                    x => ILPatternMatchingExt.MatchStloc(x, 108),
                    x => ILPatternMatchingExt.MatchLdloc(x, 5),
                })) {
                    cursor.Index -= 17;
                    cursor.Emit(OpCodes.Pop);
                    cursor.Emit(OpCodes.Ldc_I4_0);
                } else {
                    Main.Logger.LogError("Saw Bleed RemoveOrigHook Failed!");
                }
                //if (cursor.TryGotoNext(MoveType.Before, new Func<Instruction, bool>[] {
                //    x => ILPatternMatchingExt.MatchLdarg(x, 1),
                //    x => ILPatternMatchingExt.MatchLdfld<DamageInfo>(x, "crit"),
                //    x => ILPatternMatchingExt.Match(x, OpCodes.Brfalse),
                //    x => ILPatternMatchingExt.MatchLdarg(x, 1),
                //})) {
                //    cursor.Index += 2;
                //    cursor.Emit(OpCodes.Pop);
                //    cursor.Emit(OpCodes.Ldc_I4_0);
                //} else {
                //    Main.Logger.LogError("SuperBleedOnCrit RemoveOrigHook Failed!");
                //}
            };
        }

        private void RepeatHealHook() {
            IL.RoR2.HealthComponent.Heal += (ILContext il) => {
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

        private void SiphonOnLowHealthHook() {
            On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.FixedUpdate += (On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.orig_FixedUpdate orig, RoR2.Items.SiphonOnLowHealthItemBodyBehavior self) => {
                if (self.siphonNearbyController.NetworkmaxTargets != self.stack) {
                    self.siphonNearbyController.NetworkmaxTargets = self.stack;
                    self.siphonNearbyController.Networkradius = 10 + 5 * self.stack;
                    self.siphonNearbyController.damagePerSecondCoefficient = self.stack;
                }
            };
        }

        [RequireComponent(typeof(ProjectileExplosion))]
        [RequireComponent(typeof(RadialForce))]
        [RequireComponent(typeof(ProjectileFuse))]
        private class ElementalRingVoidBlackHoleAction : MonoBehaviour {
            private ProjectileExplosion projectileExplosion;
            private ProjectileFuse projectileFuse;
            private RadialForce radialForce;

            private void Awake() {
                projectileExplosion = GetComponent<ProjectileExplosion>();
                projectileFuse = GetComponent<ProjectileFuse>();
                radialForce = GetComponent<RadialForce>();
            }

            private void FixedUpdate() {
                if ((projectileFuse.fuse - Time.fixedDeltaTime) <= 0) {
                    List<HurtBox> hurtBoxes = new();
                    radialForce.SearchForTargets(hurtBoxes);
                    projectileExplosion.blastDamageCoefficient += hurtBoxes.Count;
                    hurtBoxes.Clear();
                }
            }
        }
    }
}