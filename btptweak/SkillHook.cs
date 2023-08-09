using AncientScepter;
using EntityStates.Merc;
using EntityStates.Toolbot;
using EntityStates;
using HIFUCaptainTweaks.Skills;
using HIFUEngineerTweaks.Skills;
using HIFULoaderTweaks.Skills;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.CharacterAI;
using EntityStates.Treebot.TreebotFlower;

namespace BtpTweak {

    internal class SkillHook {
        public static ProjectileDotZone fireRainDotZone_;  // 火雨
        public static ProjectileProximityBeamController projectileProximityBeamController;  // 电塔
        public static ItemIndex 古代权杖_;
        public static GameObject 异端万刃风暴_;

        public static Dictionary<int, int> 盗贼标记_ = new();

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
            //=== other
            古代权杖_ = AncientScepterItem.instance.ItemDef.itemIndex;
            //=== Bandit2
            //=== Captain
            CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<ProjectileTargetComponent>();  // 莉莉丝打击
            CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<跟随目标>();
            ProjectileSphereTargetFinder sphereTargetFinder = CaptainAirstrikeAlt2.airstrikePrefab.AddComponent<ProjectileSphereTargetFinder>();
            sphereTargetFinder.lookRange = float.MaxValue;
            sphereTargetFinder.allowTargetLoss = true;
            sphereTargetFinder.onlySearchIfNoTarget = true;
            sphereTargetFinder.ignoreAir = false;
            //=== Croco
            PlasmaCoreSpikestripContent.Content.Skills.DeepRot.scriptableObject.buffs[0].canStack = true;
            //=== Engi
            //=== Heretic
            foreach (SkillDef skillDef in SkillCatalog.allSkillDefs) {
                if (skillDef.skillName == "HereticDefaultSkillScepter") {
                    skillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Heretic.Weapon.Squawk));
                }
            }
            //Assets / RoR2 / Base / Items / LunarSkillReplacements / LunarSecondaryReplacement / Skills / LunarSecondaryProjectile.prefab
            //异端万刃风暴_ = R2API.PrefabAPI.InstantiateClone(, "异端万刃风暴");
            //异端万刃风暴_.AddComponent<ProjectileTargetComponent>();
            //ProjectileDirectionalTargetFinder directionalTargetFinder = 异端万刃风暴_.AddComponent<ProjectileDirectionalTargetFinder>();
            //directionalTargetFinder.lookCone = 90;
            //directionalTargetFinder.lookRange = 12;
            //directionalTargetFinder.allowTargetLoss = true;
            //directionalTargetFinder.onlySearchIfNoTarget = true;
            //directionalTargetFinder.ignoreAir = false;
            //steerTowardTarget = 异端万刃风暴_.AddComponent<ProjectileSteerTowardTarget>();
            //steerTowardTarget.rotationSpeed = 360;
            //=== Huntress
            GameObject fireRain = ProjectileCatalog.GetProjectilePrefab(ProjectileCatalog.FindProjectileIndex("AncientScepterHuntressRain"));
            fireRain.AddComponent<ProjectileTargetComponent>();
            fireRain.AddComponent<粘住目标>();
            sphereTargetFinder = fireRain.AddComponent<ProjectileSphereTargetFinder>();
            sphereTargetFinder.lookRange = float.MaxValue;
            sphereTargetFinder.onlySearchIfNoTarget = true;
            sphereTargetFinder.ignoreAir = false;
            sphereTargetFinder.allowTargetLoss = false;
            fireRainDotZone_ = fireRain.GetComponent<ProjectileDotZone>();
            //=== Loader
            GameObject thunderCrash = ProjectileCatalog.GetProjectilePrefab(ProjectileCatalog.FindProjectileIndex("AncientScepterLoaderThundercrash"));
            ProjectileProximityBeamController projectileProximityBeamController = thunderCrash.GetComponent<ProjectileProximityBeamController>();
            projectileProximityBeamController.attackFireCount *= 3;
            projectileProximityBeamController.bounces = 3;
            projectileProximityBeamController.attackRange *= 2f;
            projectileProximityBeamController.procCoefficient = 1f;
            //=== Railgunner
        }

        public static void RunStartInit() {
            //=== Bandit2
            MiscHook.bandit2Count = 0;
            盗贼标记_.Clear();
            //=== Captain
            HealingBeacon.Healing = 0.1f;
            //=== Engi
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges;
            pressure.baseMaxStock = PressureMines.charges;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f;
            Thunderslam.yVelocityCoeff = 0.23f;
            //=== Railgunner
            HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = 0.15f;
        }

        public static void LevelUp(int level) {
            int upLevel = level - 1;
            //=== Captain
            HealingBeacon.Healing = 0.1f + 0.01f * upLevel;
            //=== Engi
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges + upLevel;
            pressure.baseMaxStock = PressureMines.charges + upLevel;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (ModConfig.女猎人射程每级增加距离_.Value * upLevel);
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f + 0.005f * upLevel;
            Thunderslam.yVelocityCoeff = 0.23f + 0.007f * upLevel;
        }

        private static void 船长() {
            GameObject TazerP = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/CaptainTazer.prefab").WaitForCompletion();
            TazerP.GetComponent<ProjectileSimple>().lifetime = 6f;
            TazerP.AddComponent<爆炸产生闪电链>();
            ProjectileImpactExplosion TPPIE = TazerP.GetComponent<ProjectileImpactExplosion>();
            TPPIE.lifetimeAfterImpact = 6f;
            TPPIE.lifetime = 6f;
            //======
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += delegate (On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.force *= 1 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.Behemoth.itemIndex);
            };
            //======
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.OnEnter += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeBase.orig_OnEnter orig, EntityStates.Captain.Weapon.CallAirstrikeBase self) {
                orig(self);
                CaptainAirstrikeAlt2.airstrikePrefab.GetComponent<跟随目标>().speed = self.characterBody.level * self.characterBody.inventory.GetItemCount(古代权杖_);
            };
        }

        private static void 磁轨炮手() {
            On.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += delegate (On.EntityStates.Railgunner.Weapon.BaseFireSnipe.orig_ModifyBullet orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    CharacterMaster master = self.characterBody.master;
                    int itemCount = master.inventory.GetItemCount(古代权杖_);
                    if (itemCount > 0) {
                        float moneyToDamage = 0.1f * itemCount * master.money;
                        bulletAttack.damage += moneyToDamage;
                        master.money -= Convert.ToUInt32(Mathf.Min(moneyToDamage, master.money));
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
                HurtBox.sniperTargetRadius = 1 + self.characterBody.inventory.GetItemCount(DLC1Content.Items.CritDamage.itemIndex);
                HurtBox.sniperTargetRadiusSqr = HurtBox.sniperTargetRadius * HurtBox.sniperTargetRadius;
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
                bulletAttack.damage *= 2;
            };
            //======
            On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += delegate (On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.orig_OnEnter orig, EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState self) {
                orig(self);
                if (NetworkServer.active && self is EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver) {
                    CharacterBody body = self.characterBody;
                    if (body.isPlayerControlled) {
                        Inventory inventory = body.inventory;
                        int buffCount = 盗贼标记_[inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex)] = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                        int buffReduce = buffCount / (3 * (int)body.level * (1 + inventory.GetItemCount(古代权杖_)));
                        while (buffReduce-- > 0) {
                            body.RemoveBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                        }
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
                bulletAttack.damage *= 1 + 0.1f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.BleedOnHit.itemIndex);
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
            GameObject engiTurretMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiTurretMaster.prefab").WaitForCompletion();
            AISkillDriver[] drivers = engiTurretMaster.GetComponents<AISkillDriver>();
            foreach (AISkillDriver driver in drivers) {
                if (driver.maxDistance < float.MaxValue) {
                    driver.maxDistance = 360;
                }
            }
            //======
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
            SteppedSkillDef mageFireFirebolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireFirebolt.asset").WaitForCompletion();
            SteppedSkillDef mageFireLightningBolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireLightningBolt.asset").WaitForCompletion();
            mageFireFirebolt.baseRechargeInterval = 0.5f;
            mageFireLightningBolt.baseRechargeInterval = 0.5f;
            //======
            On.EntityStates.Mage.Weapon.BaseThrowBombState.OnEnter += delegate (On.EntityStates.Mage.Weapon.BaseThrowBombState.orig_OnEnter orig, EntityStates.Mage.Weapon.BaseThrowBombState self) {
                orig(self);
                if (self is EntityStates.Mage.Weapon.ThrowNovabomb) {
                    爆炸发射闪电球 fireBall = self.projectilePrefab.GetComponent<爆炸发射闪电球>();
                    if (fireBall) {
                        fireBall.meatballCount = Mathf.FloorToInt(self.characterBody.level * 0.25f);
                    } else {
                        self.projectilePrefab.AddComponent<爆炸发射闪电球>();
                    }
                } else if (self is EntityStates.Mage.Weapon.ThrowIcebomb) {
                    爆炸产生冰冻炸弹 iceExplosion = self.projectilePrefab.GetComponent<爆炸产生冰冻炸弹>();
                    if (iceExplosion) {
                        iceExplosion.explosionRadius = 6 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                    } else {
                        self.projectilePrefab.AddComponent<爆炸产生冰冻炸弹>();
                    }
                }
            };
            //======
            On.EntityStates.Mage.Weapon.FireFireBolt.FireGauntlet += delegate (On.EntityStates.Mage.Weapon.FireFireBolt.orig_FireGauntlet orig, EntityStates.Mage.Weapon.FireFireBolt self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
                }
                orig(self);
            };
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
                            evis.stopwatch = 0;
                            evis.characterBody.RemoveBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                        }
                        if (evis.inputBank.jump.down) {
                            evis.stopwatch = 99;
                        }
                    });
                } else {
                    Main.logger_.LogError("Evis Hook Error");
                }
            };
            //======
            On.EntityStates.Merc.Evis.OnEnter += delegate (On.EntityStates.Merc.Evis.orig_OnEnter orig, Evis self) {
                int itemCount = self.characterBody.inventory.GetItemCount(古代权杖_);
                if (itemCount > 0) {
                    Evis.duration = 0.5f * (1 + itemCount);
                } else {
                    Evis.duration = 1f;
                }
                orig(self);
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
                    TreebotFlower2Projectile.healthFractionYieldPerHit = 0.11f + 0.01f * ownerInventory.GetItemCount(RoR2Content.Items.TPHealingNova.itemIndex);
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
                fireRainDotZone_.lifetime = 6 + 3 * self.characterBody.inventory.GetItemCount(古代权杖_);
                orig(self);
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
            //=== 腐化二技能
            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab").WaitForCompletion();
            ProjectileSimple component = gameObject.GetComponent<ProjectileSimple>();
            component.desiredForwardSpeed = 40f;
            component.lifetime = 6.6f;
            component.lifetimeExpiredEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosionCorrupted.prefab").WaitForCompletion();
            ProjectileImpactExplosion component2 = gameObject.GetComponent<ProjectileImpactExplosion>();
            component2.blastRadius = 25f;
            RadialForce radialForce = gameObject.AddComponent<RadialForce>();
            radialForce.radius = 25f;
            radialForce.damping = 0.5f;
            radialForce.forceMagnitude = -2500f;
            radialForce.forceCoefficientAtEdge = 0.5f;
            GameObject ghostPrefab = gameObject.GetComponent<ProjectileController>().ghostPrefab;
            ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
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
                float duration = 3;
                int itemCount = self.characterBody.inventory.GetItemCount(古代权杖_);
                if (itemCount > 0) {
                    buffDef = AncientScepterMain.perishSongDebuff;
                    duration = 10 * itemCount;
                }
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (characterBody) {
                        if (TeamIndex.Lunar == characterBody.teamComponent.teamIndex) {
                            duration *= 3;
                        }
                        characterBody.AddTimedBuff(buffDef, duration);
                    }
                }
            };
        }

        private static void 指挥官() {
            On.EntityStates.Commando.CommandoWeapon.FireBarrage.OnEnter += delegate (On.EntityStates.Commando.CommandoWeapon.FireBarrage.orig_OnEnter orig, EntityStates.Commando.CommandoWeapon.FireBarrage self) {
                orig(self);
            };
        }

        private static void 装卸工() {
            GameObject pylon = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderPylon.prefab").WaitForCompletion();
            projectileProximityBeamController = pylon.GetComponent<ProjectileProximityBeamController>();
            GameObject grap = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderHook.prefab").WaitForCompletion();
            ProjectileSimple projectileSimple = grap.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed *= 2f;
            ProjectileGrappleController grapC = grap.GetComponent<ProjectileGrappleController>();
            grapC.maxTravelDistance *= 1.5f;
            //======  雷冲
            SteppedSkillDef thunder = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Loader/ChargeZapFist.asset").WaitForCompletion();
            thunder.baseRechargeInterval = 3f;
            //======
            On.EntityStates.Loader.ThrowPylon.OnEnter += delegate (On.EntityStates.Loader.ThrowPylon.orig_OnEnter orig, EntityStates.Loader.ThrowPylon self) {
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                if (itemCount > 1) {
                    projectileProximityBeamController.attackRange = M551Pylon.aoe * (1 + itemCount);
                    projectileProximityBeamController.bounces = M551Pylon.bounces + itemCount;
                }
                orig(self);
            };
            //======
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += delegate (On.EntityStates.Loader.BaseSwingChargedFist.orig_OnEnter orig, EntityStates.Loader.BaseSwingChargedFist self) {
                if (self.isAuthority) {
                    int 提升 = 1 + self.characterBody.inventory.GetItemCount(古代权杖_);
                    if (提升 > 1) {
                        if ((self is EntityStates.Loader.SwingChargedFist)) {
                            self.maxPunchForce *= 2;
                            self.maxLungeSpeed *= 提升;
                        } else {
                            self.characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, self.baseDuration);
                            self.maxLungeSpeed *= 1 + 提升;
                        }
                    }
                }
                orig(self);
            };
        }
    }
}