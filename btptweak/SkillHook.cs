using AncientScepter;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
using EntityStates.Merc;
using EntityStates.Toolbot;
using EntityStates.Treebot.TreebotFlower;
using HIFUEngineerTweaks.Skills;
using HIFULoaderTweaks.Skills;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using BtpTweak.Utils;
using BtpTweak.Utils.Paths;

namespace BtpTweak {

    public class SkillHook {
        public static BodyIndex Heretic;
        public static ItemIndex 古代权杖_;
        public static ProjectileDotZone fireRainDotZone_;  // 火雨
        public static ProjectileProximityBeamController proximityBeamController;  // 电塔

        public static void AddHook() {
            船长();
            磁轨炮手();
            盗贼();
            多功能抢兵();
            工程师();
            工匠();
            雇佣兵();
            雷克斯();
            女猎人();
            呛鼻毒师();
            虚空恶鬼();
            异教徒();
            指挥官();
            装卸工();
        }

        public static void LateInit() {
            RoR2GameObjects r2GameObjects = new();
            //=== other
            古代权杖_ = AncientScepterItem.instance.ItemDef.itemIndex;
            //=== Bandit2
            //=== Captain
            EntityStates.CaptainSupplyDrop.ShockZoneMainState.shockRadius = 20;
            GameObject captainTazer = Helpers.FindProjectilePrefab("CaptainTazer");
            captainTazer.AddComponent<ProjectileImpactActionCaller>().BindAction(ExplosionActions.闪电链);
            captainTazer.GetComponent<ProjectileSimple>().lifetime = 6f;
            ProjectileImpactExplosion impactExplosion = captainTazer.GetComponent<ProjectileImpactExplosion>();
            impactExplosion.lifetimeAfterImpact = 6f;
            impactExplosion.lifetime = 6f;
            SkillDef 大恶魔打击 = new RoR2CaptainOrbitalSkillDefs().PrepAirstrikeAlt.Load<SkillDef>();
            大恶魔打击.baseRechargeInterval = 10;
            大恶魔打击.baseMaxStock = 4;
            大恶魔打击.requiredStock = 4;
            大恶魔打击.stockToConsume = 4;
            SkillDef 莉莉丝打击 = (from v in SkillCatalog.allSkillDefs where v.baseRechargeInterval == 60 && v.skillName == 大恶魔打击.skillName + "Scepter" select v).FirstOrDefault();
            莉莉丝打击.baseRechargeInterval = 10;
            莉莉丝打击.baseMaxStock = 6;
            莉莉丝打击.requiredStock = 6;
            莉莉丝打击.stockToConsume = 6;
            CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<ProjectileTargetComponent>();  // 莉莉丝打击
            CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<莉莉丝打击追踪>();
            CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<ProjectileNetworkTransform>();
            var projectileImpactExplosion = CaptainAirstrikeAlt2.airstrikePrefab.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.fireChildren = true;
            projectileImpactExplosion.childrenCount = 1;
            projectileImpactExplosion.childrenDamageCoefficient = 1;
            projectileImpactExplosion.childrenProjectilePrefab = Helpers.FindProjectilePrefab("BrassMonolithLargeShockwave");
            ProjectileSphereTargetFinder sphereTargetFinder = CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<ProjectileSphereTargetFinder>();
            sphereTargetFinder.lookRange = float.MaxValue;
            sphereTargetFinder.allowTargetLoss = true;
            sphereTargetFinder.onlySearchIfNoTarget = true;
            sphereTargetFinder.ignoreAir = false;
            //=== Croco
            DeepRot.scriptableObject.buffs[0].canStack = true;
            //=== Engi
            Array.Find(r2GameObjects.EngiTurretMaster.LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 3;
            //=== Heretic
            LegacyResourcesAPI.Load<SkillDef>("SkillDefs/HereticBody/HereticDefaultAbility").baseRechargeInterval = 40;
            Helpers.FindSkillDef("HereticDefaultSkillScepter").activationState = new SerializableEntityStateType(typeof(EntityStates.Heretic.Weapon.Squawk));
            //=== Huntress
            CharacterBody huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();  // 女猎人调整
            huntressBody.baseDamage = 15;
            huntressBody.levelDamage = huntressBody.baseDamage * 0.2f;
            huntressBody.baseCrit = 10;
            huntressBody.levelCrit = 1;
            GameObject fireRain = Helpers.FindProjectilePrefab("AncientScepterHuntressRain");
            fireRain.AddComponent<ProjectileTargetComponent>();
            fireRain.AddComponent<火雨传送>();
            fireRain.AddComponent<ProjectileNetworkTransform>();
            fireRainDotZone_ = fireRain.GetComponent<ProjectileDotZone>();
            sphereTargetFinder = fireRain.AddComponent<ProjectileSphereTargetFinder>();
            sphereTargetFinder.lookRange = float.MaxValue;
            sphereTargetFinder.onlySearchIfNoTarget = true;
            sphereTargetFinder.ignoreAir = false;
            sphereTargetFinder.allowTargetLoss = false;
            //=== Loader
            //=== Mage
            Helpers.FindProjectilePrefab("MageIceBombProjectile").AddComponent<ProjectileImpactActionCaller>().BindAction(ExplosionActions.冰冻炸弹);
            GameObject mageLightningBomb = Helpers.FindProjectilePrefab("MageLightningBombProjectile");
            mageLightningBomb.AddComponent<ProjectileImpactActionCaller>().BindAction(ExplosionActions.纳米炸弹);
            ProjectileProximityBeamController mageProximityBeamController = mageLightningBomb.GetComponent<ProjectileProximityBeamController>();
            mageProximityBeamController.attackFireCount = 3;
            mageProximityBeamController.attackRange = 30;
            mageProximityBeamController.inheritDamageType = true;
            mageProximityBeamController.damageCoefficient *= 0.1f;
            mageProximityBeamController.listClearInterval = 0;
            //=== Railgunner
        }

        public static void RunStartInit() {
            //=== Bandit2
            //=== Captain
            HealingWard healingWard = EntityStates.CaptainSupplyDrop.HealZoneMainState.healZonePrefab.GetComponent<HealingWard>();
            healingWard.radius = 10;
            healingWard.healFraction = 0.02f;
            new RoR2GameObjects().CaptainSupplyDropEquipmentRestock.LoadComponent<GenericEnergyComponent>().chargeRate = 0.5f;
            //=== Engi
            RoR2SkillDefs r2SkillDefs = new();
            r2SkillDefs.EngiBodyPlaceMine.Load<SkillDef>().baseMaxStock = PressureMines.charges;
            r2SkillDefs.EngiBodyPlaceSpiderMine.Load<SkillDef>().baseMaxStock = SpiderMines.charges;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f;
            Thunderslam.yVelocityCoeff = 0.23f;
            //=== Railgunner
        }

        public static void LevelUp(int level) {
            int upLevel = level - 1;
            //=== Captain
            HealingWard healingWard = EntityStates.CaptainSupplyDrop.HealZoneMainState.healZonePrefab.GetComponent<HealingWard>();
            healingWard.radius = 10 + upLevel;
            healingWard.healFraction = 0.02f + 0.002f * upLevel;
            new RoR2GameObjects().CaptainSupplyDropEquipmentRestock.LoadComponent<GenericEnergyComponent>().chargeRate = 0.5f * level;
            //=== Engi
            RoR2SkillDefs r2SkillDefs = new();
            r2SkillDefs.EngiBodyPlaceMine.Load<SkillDef>().baseMaxStock = PressureMines.charges + upLevel;
            r2SkillDefs.EngiBodyPlaceSpiderMine.Load<SkillDef>().baseMaxStock = SpiderMines.charges + upLevel;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (ModConfig.女猎人射程每级增加距离_.Value * upLevel);
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f + 0.006f * upLevel;
            Thunderslam.yVelocityCoeff = 0.23f + 0.008f * upLevel;
        }

        private static void 船长() {
            new RoR2CaptainSupplyDropSkillDefs().PrepSupplyDrop.Load<SkillDef>().baseRechargeInterval = 1;
            RoR2Application.onLoad = (Action)Delegate.Combine(RoR2Application.onLoad, delegate () {
                foreach (SceneDef sceneDef in SceneCatalog.allSceneDefs) {
                    sceneDef.blockOrbitalSkills = false;
                }
            });
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += delegate (On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.force *= 1 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.Behemoth.itemIndex);
            };
            //======
            On.EntityStates.Captain.Weapon.CallAirstrikeAlt.OnExit += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeAlt.orig_OnExit orig, EntityStates.Captain.Weapon.CallAirstrikeAlt self) {
                if (self.characterBody.inventory.GetItemCount(古代权杖_) == 0) {
                    self.damageCoefficient = 500f;
                }
                orig(self);
            };
            //======
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeBase.orig_ModifyProjectile orig, EntityStates.Captain.Weapon.CallAirstrikeBase self, ref FireProjectileInfo fireProjectileInfo) {
                orig(self, ref fireProjectileInfo);
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine.itemIndex);
                if (itemCount > 0) {
                    fireProjectileInfo.useFuseOverride = true;
                    fireProjectileInfo.fuseOverride = fireProjectileInfo.projectilePrefab.GetComponent<ProjectileImpactExplosion>().lifetime * Mathf.Pow(0.666f, itemCount);
                }
            };
        }

        private static void 磁轨炮手() {
            On.EntityStates.Railgunner.Weapon.BaseFireSnipe.OnEnter += delegate (On.EntityStates.Railgunner.Weapon.BaseFireSnipe.orig_OnEnter orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self) {
                orig(self);
                if (NetworkServer.active && self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    CharacterMaster master = self.characterBody.master;
                    int itemCount = master.inventory.GetItemCount(古代权杖_);
                    if (itemCount > 0) {
                        master.money -= Convert.ToUInt32(Mathf.Min(0.1f * itemCount * master.money, master.money));
                    }
                }
            };
            On.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += delegate (On.EntityStates.Railgunner.Weapon.BaseFireSnipe.orig_ModifyBullet orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    CharacterMaster master = self.characterBody.master;
                    int itemCount = master.inventory.GetItemCount(古代权杖_);
                    if (itemCount > 0) {
                        bulletAttack.damage += 0.1f * itemCount * master.money;
                    }
                }
            };
            //==========
            On.EntityStates.Railgunner.Reload.Waiting.OnEnter += delegate (On.EntityStates.Railgunner.Reload.Waiting.orig_OnEnter orig, EntityStates.Railgunner.Reload.Waiting self) {
                int magazineCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                HIFURailgunnerTweaks.Misc.ScopeAndReload.Damage = 5 + magazineCount;
                HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = 0.14f + (0.01f * (self.characterBody.level - magazineCount));
                orig(self);
            };
            //==========
            On.EntityStates.Railgunner.Scope.BaseWindUp.OnEnter += delegate (On.EntityStates.Railgunner.Scope.BaseWindUp.orig_OnEnter orig, EntityStates.Railgunner.Scope.BaseWindUp self) {
                HurtBox.sniperTargetRadiusSqr = Mathf.Pow(self.characterBody.inventory.GetItemCount(DLC1Content.Items.CritDamage.itemIndex) + 1, 2);
                orig(self);
            };
            //==========
            On.RoR2.Projectile.SlowDownProjectiles.Start += delegate (On.RoR2.Projectile.SlowDownProjectiles.orig_Start orig, SlowDownProjectiles self) {
                orig(self);
                if (self.name.StartsWith("Rail")) {
                    self.slowDownCoefficient = 0.01f;
                }
            };
        }

        private static void 盗贼() {
            On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmResetRevolver self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.damage *= 2f;
            };
            //======
            On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += delegate (On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.orig_OnEnter orig, EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState self) {
                orig(self);
                if (NetworkServer.active && self is EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver) {
                    CharacterBody body = self.characterBody;
                    if (body.isPlayerControlled) {
                        Inventory inventory = body.inventory;
                        int buffCount = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                        buffCount -= buffCount / (3 * (int)body.level * (1 + inventory.GetItemCount(古代权杖_)));
                        body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, buffCount);
                        inventory.GiveItem(JunkContent.Items.SkullCounter.itemIndex, buffCount - inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                    }
                }
            };
        }

        private static void 多功能抢兵() {
            On.EntityStates.Toolbot.BaseNailgunState.FireBullet += delegate (On.EntityStates.Toolbot.BaseNailgunState.orig_FireBullet orig, BaseNailgunState self, Ray aimRay, int bulletCount, float spreadPitchScale, float spreadYawScale) {
                float tmp = BaseNailgunState.maxDistance;
                BaseNailgunState.maxDistance += 0.07f * self.fireNumber;
                orig(self, aimRay, bulletCount, spreadPitchScale, spreadYawScale);
                BaseNailgunState.maxDistance = tmp;
            };
            //==========
            On.EntityStates.Toolbot.FireSpear.ModifyBullet += delegate (On.EntityStates.Toolbot.FireSpear.orig_ModifyBullet orig, FireSpear self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.damage *= 1 + 0.05f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.Crowbar.itemIndex);
            };
            //==========
            On.EntityStates.Toolbot.AimGrenade.OnEnter += delegate (On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.StunChanceOnHit.itemIndex);
                }
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.ToolbotDualWieldBase.OnEnter += delegate (On.EntityStates.Toolbot.ToolbotDualWieldBase.orig_OnEnter orig, ToolbotDualWieldBase self) {
                int 权杖层数 = self.characterBody.inventory.GetItemCount(古代权杖_);
                if (权杖层数 == 0) {
                    ToolbotDualWieldBase.bonusBuff = RoR2Content.Buffs.SmallArmorBoost;
                } else if (权杖层数 == 1) {
                    ToolbotDualWieldBase.bonusBuff = RoR2Content.Buffs.ArmorBoost;
                    self.applyPenaltyBuff = false;
                } else if (权杖层数 > 1) {
                    ToolbotDualWieldBase.bonusBuff = RoR2Content.Buffs.ElephantArmorBoost;
                    self.applyPenaltyBuff = false;
                }
                orig(self);
            };
        }

        private static void 工程师() {
            On.EntityStates.Engi.EngiBubbleShield.Deployed.OnEnter += delegate (On.EntityStates.Engi.EngiBubbleShield.Deployed.orig_OnEnter orig, EntityStates.Engi.EngiBubbleShield.Deployed self) {
                orig(self);
                if (NetworkServer.active) {
                    ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
                    for (int i = 0; i < teamMembers.Count; ++i) {
                        teamMembers[i].body.AddTimedBuff(RoR2Content.Buffs.EngiShield, 6);
                    }
                }
            };
        }

        private static void 工匠() {
            RoR2SteppedSkillDefs r2SteppedSkillDefs = new();
            SteppedSkillDef mageFireFirebolt = r2SteppedSkillDefs.MageBodyFireFirebolt.Load<SteppedSkillDef>();
            mageFireFirebolt.baseRechargeInterval = 0;
            mageFireFirebolt.baseMaxStock = 1;
            SteppedSkillDef mageFireLightningBolt = r2SteppedSkillDefs.MageBodyFireIceBolt.Load<SteppedSkillDef>();
            mageFireLightningBolt.baseRechargeInterval = 0;
            mageFireLightningBolt.baseMaxStock = 1;
            //======
            On.EntityStates.Mage.Weapon.FireFireBolt.OnEnter += delegate (On.EntityStates.Mage.Weapon.FireFireBolt.orig_OnEnter orig, EntityStates.Mage.Weapon.FireFireBolt self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                }
                orig(self);
            };
            //======
            On.EntityStates.Mage.Weapon.BaseThrowBombState.ModifyProjectile += delegate (On.EntityStates.Mage.Weapon.BaseThrowBombState.orig_ModifyProjectile orig, EntityStates.Mage.Weapon.BaseThrowBombState self, ref FireProjectileInfo projectileInfo) {
                orig(self, ref projectileInfo);
                if (self is EntityStates.Mage.Weapon.ThrowNovabomb) {
                    projectileInfo.damage *= 1 + 0.1f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex);
                }
            };
            //======
        }

        private static void 雇佣兵() {
            IL.EntityStates.Merc.Evis.FixedUpdate += delegate (ILContext il) {
                ILCursor ilcursor = new(il);
                Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
                array[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 4);
                if (ilcursor.TryGotoNext(array)) {
                    ++ilcursor.Index;
                    ilcursor.Emit(OpCodes.Ldarg, 0);
                    ilcursor.EmitDelegate(delegate (Evis evis) {
                        if (NetworkServer.active) {
                            Evis.damageCoefficient += 0.015f;
                        }
                        if (evis.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex) > 0) {
                            evis.characterBody.RemoveBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                            if (evis.inputBank.jump.down && evis.isAuthority) {
                                evis.stopwatch = 99;
                            } else {
                                evis.stopwatch = 0;
                            }
                        }
                    });
                } else {
                    Main.logger_.LogError("Evis Hook Error");
                }
            };
            //======
            On.EntityStates.Merc.WhirlwindBase.OnEnter += delegate (On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, WhirlwindBase self) {
                orig(self);
                self.moveSpeedStat = 7;
            };
            //======
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += delegate (On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, EntityStates.Merc.Weapon.GroundLight2 self) {
                self.ignoreAttackSpeed = true;
                orig(self);
            };
        }

        private static void 雷克斯() {
            On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.OnEnter += delegate (On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.orig_OnEnter orig, EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile self) {
                orig(self);
                if (self.owner) {
                    Inventory ownerInventory = self.owner.GetComponent<CharacterBody>().inventory;
                    int itemCount = ownerInventory.GetItemCount(古代权杖_);
                    if (itemCount > 1) {
                        TreebotFlower2Projectile.duration = 8 * itemCount;
                        TreebotFlower2Projectile.rootPulseCount = 16 * itemCount;
                        TreebotFlower2Projectile.healPulseCount = 16 * itemCount;
                    }
                    TreebotFlower2Projectile.healthFractionYieldPerHit = 0.11f + 0.05f * ownerInventory.GetItemCount(RoR2Content.Items.TPHealingNova.itemIndex);
                }
            };
        }

        private static void 女猎人() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += delegate (On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnExit orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
                orig(self);
                if (!NetworkServer.active || self.initialOrbTarget == null) {
                    return;
                }
                int itemCount = self.characterBody.inventory.GetItemCount(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                if (itemCount > 0 && Util.CheckRoll(10 * itemCount * self.orbProcCoefficient, self.characterBody.master)) {
                    ++self.maxArrowCount;
                }
                while (self.firedArrowCount++ < self.maxArrowCount) {  // 发射剩余箭矢，防止攻速过快箭矢丢失
                    GenericDamageOrb genericDamageOrb = self.CreateArrowOrb();
                    genericDamageOrb.damageValue = self.damageStat * self.orbDamageCoefficient;
                    genericDamageOrb.isCrit = self.isCrit;
                    genericDamageOrb.teamIndex = self.teamComponent.teamIndex;
                    genericDamageOrb.attacker = self.gameObject;
                    genericDamageOrb.procCoefficient = self.orbProcCoefficient;
                    genericDamageOrb.origin = self.childLocator.FindChild(self.muzzleString).position;
                    genericDamageOrb.target = self.initialOrbTarget;
                    EffectManager.SimpleMuzzleFlash(self.muzzleflashEffectPrefab, self.gameObject, self.muzzleString, true);
                    OrbManager.instance.AddOrb(genericDamageOrb);
                }
            };
            //======
            On.EntityStates.Huntress.ArrowRain.OnEnter += delegate (On.EntityStates.Huntress.ArrowRain.orig_OnEnter orig, EntityStates.Huntress.ArrowRain self) {
                orig(self);
                fireRainDotZone_.lifetime = 6 + 3 * self.characterBody.inventory.GetItemCount(古代权杖_);
            };
        }

        private static void 呛鼻毒师() {
            On.EntityStates.Croco.Bite.OnEnter += delegate (On.EntityStates.Croco.Bite.orig_OnEnter orig, EntityStates.Croco.Bite self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.Tooth.itemIndex);
                }
                orig(self);
            };
        }

        private static void 虚空恶鬼() {
            RoR2GameObjects r2GameObjects = new();
            //=== 腐化二技能
            GameObject gameObject = r2GameObjects.VoidSurvivorMegaBlasterBigProjectileCorrupted.Load<GameObject>();
            ProjectileSimple projectileSimple = gameObject.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 40f;
            projectileSimple.lifetime = 6.6f;
            projectileSimple.lifetimeExpiredEffect = r2GameObjects.VoidSurvivorMegaBlasterExplosionCorrupted.Load<GameObject>();
            gameObject.GetComponent<ProjectileImpactExplosion>().blastRadius = 25f; ;
            RadialForce radialForce = gameObject.AddComponent<RadialForce>();
            radialForce.radius = 25f;
            radialForce.damping = 0.5f;
            radialForce.forceMagnitude = -2500f;
            radialForce.forceCoefficientAtEdge = 0.5f;
            gameObject.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self) {
                self.damageCoefficient = 3.6f;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
                self.baseDuration = 4;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += delegate (On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
                if (self is EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBig) {
                    self.force = 4444;
                    self.damageCoefficient = 44.44f;
                } else {
                    self.force = 666;
                    self.damageCoefficient = 6.66f;
                }
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
                self.damageCoefficient = 25;
                self.selfKnockbackForce = 0;
                orig(self);
            };
        }

        private static void 异教徒() {
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += delegate (On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
                orig(self);
                BuffDef buffDef = RoR2Content.Buffs.LunarSecondaryRoot;
                float duration = 10;
                int itemCount = self.characterBody.inventory.GetItemCount(古代权杖_);
                if (itemCount > 0) {
                    buffDef = AncientScepterMain.perishSongDebuff;
                    duration = 10 * itemCount;
                }
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (TeamIndex.Lunar == characterBody.teamComponent.teamIndex) {
                        duration *= 3;
                    }
                    characterBody.AddTimedBuff(buffDef, duration);
                }
            };
            //=====
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += delegate (On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, FireLunarNeedle self) {
                if (self.characterBody.bodyIndex == Heretic) {
                    FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 360f;
                } else {
                    FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 90f;
                }
                orig(self);
            };
            //=====
            On.EntityStates.GhostUtilitySkillState.FixedUpdate += delegate (On.EntityStates.GhostUtilitySkillState.orig_FixedUpdate orig, GhostUtilitySkillState self) {
                orig(self);
                if (self.isAuthority && self.characterBody.bodyIndex == Heretic && self.inputBank.skill3.justReleased && self.fixedAge > 1f) {
                    self.outer.SetNextStateToMain();
                }
            };
            //=====
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += delegate (On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self) {
                if (self.characterBody.bodyIndex == Heretic) {
                    EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
                } else {
                    EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f;
                }
                orig(self);
            };
        }

        private static void 指挥官() {
        }

        private static void 装卸工() {
            RoR2GameObjects r2GameObjects = new();
            SkillHook.proximityBeamController = r2GameObjects.LoaderPylon.LoadComponent<ProjectileProximityBeamController>();  // 电塔
            GameObject loaderHook = r2GameObjects.LoaderHook.Load<GameObject>();  // 抓钩
            loaderHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 1.5f;
            //====== 雷冲
            new RoR2SteppedSkillDefs().ChargeZapFist.Load<SteppedSkillDef>().baseRechargeInterval = 3f;
            ProjectileProximityBeamController proximityBeamController = r2GameObjects.LoaderZapCone.LoadComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange *= 2;
            proximityBeamController.bounces = 1;
            //======
            On.EntityStates.Loader.ThrowPylon.OnEnter += delegate (On.EntityStates.Loader.ThrowPylon.orig_OnEnter orig, EntityStates.Loader.ThrowPylon self) {
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                SkillHook.proximityBeamController.attackRange = M551Pylon.aoe * (1 + itemCount);
                SkillHook.proximityBeamController.bounces = M551Pylon.bounces + itemCount;
                orig(self);
            };
            //======
            On.EntityStates.Loader.SwingZapFist.OnExit += delegate (On.EntityStates.Loader.SwingZapFist.orig_OnExit orig, EntityStates.Loader.SwingZapFist self) {
                EntityStates.Loader.SwingZapFist.selfKnockback = 100 * self.punchSpeed;
                orig(self);
            };
        }

        public class ExplosionActions {

            public static void 纳米炸弹(ProjectileImpactActionCaller 炸弹, ProjectileImpactInfo impactInfo) {
                ProjectileController projectileController = 炸弹.GetComponent<ProjectileController>();
                if (projectileController.owner == null) {
                    return;
                }
                ProjectileDamage projectileDamage = 炸弹.GetComponent<ProjectileDamage>();
                int itemCount = projectileController.owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex);
                List<HealthComponent> bouncedObjects = new();
                BullseyeSearch search = new() {
                    filterByLoS = false,
                    maxDistanceFilter = 30 + 3 * itemCount,
                    searchDirection = Vector3.zero,
                    searchOrigin = impactInfo.estimatedPointOfImpact,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    teamMaskFilter = TeamMask.allButNeutral,
                };
                search.teamMaskFilter.RemoveTeam(projectileController.teamFilter.teamIndex);
                for (int i = 0 - itemCount; i < 3; ++i) {
                    LightningOrb lightningOrb = new() {
                        attacker = projectileController.owner.gameObject,
                        bouncedObjects = new List<HealthComponent>(),
                        bouncesRemaining = 0,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = projectileDamage.damageType,
                        damageValue = projectileDamage.damage * (0.4f * (1 + itemCount)),
                        isCrit = projectileDamage.crit,
                        lightningType = LightningOrb.LightningType.MageLightning,
                        origin = search.searchOrigin,
                        procChainMask = default,
                        procCoefficient = 0.3f,
                        range = search.maxDistanceFilter,
                        teamIndex = projectileController.teamFilter.teamIndex
                    };
                    search.RefreshCandidates();
                    HurtBox hurtBox = (from v in search.GetResults() where !bouncedObjects.Contains(v.healthComponent) select v).FirstOrDefault<HurtBox>();
                    if (hurtBox) {
                        bouncedObjects.Add(hurtBox.healthComponent);
                        lightningOrb.target = hurtBox;
                        OrbManager.instance.AddOrb(lightningOrb);
                    }
                }
            }

            public static void 冰冻炸弹(ProjectileImpactActionCaller 冰冻, ProjectileImpactInfo impactInfo) {
                ProjectileController projectileController = 冰冻.GetComponent<ProjectileController>();
                if (projectileController.owner == null) {
                    return;
                }
                ProjectileDamage projectileDamage = 冰冻.GetComponent<ProjectileDamage>();
                int explosionRadius = 10 + projectileController.owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                if (projectileDamage.crit) {
                    explosionRadius *= 2;
                }
                GameObject iceExplosion = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast"), impactInfo.estimatedPointOfImpact, Quaternion.identity);
                iceExplosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
                DelayBlast delayBlast = iceExplosion.GetComponent<DelayBlast>();
                if (delayBlast) {
                    delayBlast.position = impactInfo.estimatedPointOfImpact;
                    delayBlast.baseDamage = projectileDamage.damage;
                    delayBlast.baseForce = projectileDamage.force;
                    delayBlast.attacker = projectileController.owner.gameObject;
                    delayBlast.radius = explosionRadius;
                    delayBlast.crit = projectileDamage.crit;
                    delayBlast.procCoefficient = 0.5f;
                    delayBlast.maxTimer = 2f;
                    delayBlast.falloffModel = BlastAttack.FalloffModel.None;
                    delayBlast.explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion");
                    delayBlast.delayEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/AffixWhiteDelayEffect");
                    delayBlast.damageType = DamageType.Freeze2s;
                    TeamFilter teamFilter = iceExplosion.GetComponent<TeamFilter>();
                    if (teamFilter) {
                        teamFilter.teamIndex = projectileController.teamFilter.teamIndex;
                    }
                }
            }

            public static void 闪电链(ProjectileImpactActionCaller 闪电链, ProjectileImpactInfo impactInfo) {
                ProjectileController projectileController = 闪电链.GetComponent<ProjectileController>();
                ProjectileDamage projectileDamage = 闪电链.GetComponent<ProjectileDamage>();
                LightningOrb lightningOrb = new() {
                    attacker = projectileController.owner?.gameObject,
                    bouncedObjects = new List<HealthComponent>(),
                    bouncesRemaining = 30,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = projectileDamage.damageType,
                    damageValue = projectileDamage.damage,
                    isCrit = projectileDamage.crit,
                    lightningType = LightningOrb.LightningType.Ukulele,
                    origin = impactInfo.estimatedPointOfImpact,
                    procChainMask = default,
                    procCoefficient = 1f,
                    range = 30,
                    teamIndex = projectileController.teamFilter.teamIndex
                };
                lightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                HurtBox hurtBox = lightningOrb.PickNextTarget(impactInfo.estimatedPointOfImpact);
                if (hurtBox) {
                    lightningOrb.target = hurtBox;
                    OrbManager.instance.AddOrb(lightningOrb);
                }
            }
        }
    }
}