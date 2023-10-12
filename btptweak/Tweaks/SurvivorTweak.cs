using BtpTweak.Utils;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
using EntityStates.Merc;
using EntityStates.Toolbot;
using HIFUEngineerTweaks.Skills;
using HIFULoaderTweaks.Skills;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class SurvivorTweak : TweakBase {
        private BodyIndex HereticBodyIndex;

        public override void AddHooks() {
            base.AddHooks();
            GlobalEventManager.onTeamLevelUp += LevelUp;
        }

        public override void Load() {
            base.Load();
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

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            //=== Bandit2
            //=== Captain
            HealingWard healingWard = EntityStates.CaptainSupplyDrop.HealZoneMainState.healZonePrefab.GetComponent<HealingWard>();
            healingWard.radius = 10;
            healingWard.healFraction = 0.02f;
            "RoR2/Base/Captain/CaptainSupplyDrop, EquipmentRestock.prefab".LoadComponent<GenericEnergyComponent>().chargeRate = 0.5f;
            //=== Engi
            "RoR2/Base/Engi/EngiBodyPlaceMine.asset".Load<SkillDef>().baseMaxStock = PressureMines.charges;
            "RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset".Load<SkillDef>().baseMaxStock = SpiderMines.charges;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== Loader
            //=== Railgunner
        }

        private void LevelUp(TeamIndex teamIndex) {
            if (TeamIndex.Player == teamIndex) {
                int upLevel = (int)(TeamManager.instance.GetTeamLevel(teamIndex) - 1);
                //=== Captain
                HealingWard healingWard = EntityStates.CaptainSupplyDrop.HealZoneMainState.healZonePrefab.GetComponent<HealingWard>();
                healingWard.radius = 10 + upLevel;
                healingWard.healFraction = 0.02f + 0.002f * upLevel;
                "RoR2/Base/Captain/CaptainSupplyDrop, EquipmentRestock.prefab".LoadComponent<GenericEnergyComponent>().chargeRate = 0.5f * (1 + upLevel);
                //=== Engi
                "RoR2/Base/Engi/EngiBodyPlaceMine.asset".Load<SkillDef>().baseMaxStock = PressureMines.charges + upLevel;
                "RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset".Load<SkillDef>().baseMaxStock = SpiderMines.charges + upLevel;
                //=== Huntress
                HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (ModConfig.女猎人射程每级增加距离.Value * upLevel);
                //=== Loader
            }
        }

        private void 船长() {
            //====== 1
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += delegate (On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.force *= 1 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.Behemoth.itemIndex);
            };
            //====== 2
            GameObject captainTazer = "RoR2/Base/Captain/CaptainTazer.prefab".Load<GameObject>();
            captainTazer.AddComponent<ChainLightning>();
            captainTazer.GetComponent<ProjectileSimple>().lifetime = 6f;
            ProjectileImpactExplosion impactExplosion = captainTazer.GetComponent<ProjectileImpactExplosion>();
            impactExplosion.lifetimeAfterImpact = 6f;
            impactExplosion.lifetime = 6f;
            //====== 3
            On.EntityStates.Captain.Weapon.CallAirstrikeAlt.OnExit += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeAlt.orig_OnExit orig, EntityStates.Captain.Weapon.CallAirstrikeAlt self) {
                self.damageCoefficient = 500f;
                orig(self);
            };
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeBase.orig_ModifyProjectile orig, EntityStates.Captain.Weapon.CallAirstrikeBase self, ref FireProjectileInfo fireProjectileInfo) {
                orig(self, ref fireProjectileInfo);
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine.itemIndex);
                if (itemCount > 0) {
                    fireProjectileInfo.useFuseOverride = true;
                    fireProjectileInfo.fuseOverride = fireProjectileInfo.projectilePrefab.GetComponent<ProjectileImpactExplosion>().lifetime * Mathf.Pow(0.666f, itemCount);
                }
            };
            SkillDef 大恶魔打击 = "RoR2/Base/Captain/PrepAirstrikeAlt.asset".Load<SkillDef>();
            大恶魔打击.baseRechargeInterval = 10;
            大恶魔打击.baseMaxStock = 4;
            大恶魔打击.requiredStock = 4;
            大恶魔打击.stockToConsume = 4;
            //====== 4
            "RoR2/Base/Captain/PrepSupplyDrop.asset".Load<SkillDef>().baseRechargeInterval = 1;
            EntityStates.CaptainSupplyDrop.ShockZoneMainState.shockRadius = 20;
            //====== other
            RoR2Content.Survivors.Captain.bodyPrefab.GetComponent<CharacterBody>().baseMoveSpeed = 8;
            foreach (var sceneDef in SceneCatalog.allSceneDefs) { sceneDef.blockOrbitalSkills = false; }
        }

        private void 磁轨炮手() {
            "RoR2/DLC1/Railgunner/RailgunnerMineAltDetonated.prefab".LoadComponent<SlowDownProjectiles>().slowDownCoefficient = 0.01f;
            On.EntityStates.Railgunner.Reload.Reloading.AttemptBoost += delegate (On.EntityStates.Railgunner.Reload.Reloading.orig_AttemptBoost orig, EntityStates.Railgunner.Reload.Reloading self) {
                if (orig(self)) {
                    HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = Mathf.Max(0.01f, HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent * 0.9f);
                    HIFURailgunnerTweaks.Misc.ScopeAndReload.Damage *= 1.1f;
                    return true;
                } else {
                    HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = 0.15f * self.characterBody.attackSpeed;
                    HIFURailgunnerTweaks.Misc.ScopeAndReload.Damage = 4f;
                    return false;
                }
            };
            //==========
            On.EntityStates.Railgunner.Scope.BaseWindUp.OnEnter += delegate (On.EntityStates.Railgunner.Scope.BaseWindUp.orig_OnEnter orig, EntityStates.Railgunner.Scope.BaseWindUp self) {
                HurtBox.sniperTargetRadiusSqr = Mathf.Pow(1 + self.characterBody.inventory.GetItemCount(DLC1Content.Items.CritDamage.itemIndex), 2f);
                orig(self);
            };
        }

        private void 盗贼() {
            EntityStates.Bandit2.Weapon.EnterReload.baseDuration *= 0.5f;
            EntityStates.Bandit2.Weapon.Reload.baseDuration *= 0.5f;
            //=====
            On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmResetRevolver self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                bulletAttack.damage *= 2f;
            };
            //======
            On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += delegate (On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.orig_OnEnter orig, EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState self) {
                orig(self);
                if (NetworkServer.active && self is EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver) {
                    CharacterBody body = self.characterBody;
                    Inventory inventory = body.inventory;
                    int buffCount = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                    buffCount -= buffCount / (5 * (int)body.level);
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, buffCount);
                    inventory.GiveItem(JunkContent.Items.SkullCounter.itemIndex, buffCount - inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                }
            };
        }

        private void 多功能抢兵() {
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
        }

        private void 工程师() {
            Array.Find("RoR2/Base/Engi/EngiTurretMaster.prefab".LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 3;
            "RoR2/Base/Engi/EngiBubbleShield.prefab".LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = BubbleShield.duration * 0.9f;
            //======
            On.EntityStates.Engi.EngiBubbleShield.Deployed.OnEnter += delegate (On.EntityStates.Engi.EngiBubbleShield.Deployed.orig_OnEnter orig, EntityStates.Engi.EngiBubbleShield.Deployed self) {
                orig(self);
                if (NetworkServer.active) {
                    foreach (var teamMember in TeamComponent.GetTeamMembers(TeamIndex.Player)) {
                        teamMember.body.AddTimedBuff(RoR2Content.Buffs.EngiShield, 6);
                    }
                }
            };
        }

        private void 工匠() {
            SteppedSkillDef mageFireFirebolt = "RoR2/Base/Mage/MageBodyFireFirebolt.asset".Load<SteppedSkillDef>();
            mageFireFirebolt.baseRechargeInterval = 0;
            mageFireFirebolt.baseMaxStock = 1;
            SteppedSkillDef mageFireLightningBolt = "RoR2/Junk/Mage/MageBodyFireIceBolt.asset".Load<SteppedSkillDef>();
            mageFireLightningBolt.baseRechargeInterval = 0;
            mageFireLightningBolt.baseMaxStock = 1;
            "RoR2/Base/Mage/MageIceBombProjectile.prefab".Load<GameObject>().AddComponent<IceExplosion>();
            GameObject mageLightningBomb = "RoR2/Base/Mage/MageLightningBombProjectile.prefab".Load<GameObject>();
            mageLightningBomb.AddComponent<LightningExplosion>();
            ProjectileProximityBeamController mageProximityBeamController = mageLightningBomb.GetComponent<ProjectileProximityBeamController>();
            mageProximityBeamController.attackFireCount = 3;
            mageProximityBeamController.attackRange = 30;
            mageProximityBeamController.inheritDamageType = true;
            mageProximityBeamController.damageCoefficient *= 0.1f;
            mageProximityBeamController.listClearInterval = 0;
            "RoR2/Base/Mage/MageBodyIceBomb.asset".Load<SkillDef>().mustKeyPress = false;
            "RoR2/Base/Mage/MageBodyNovaBomb.asset".Load<SkillDef>().mustKeyPress = false;
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
        }

        private void 雇佣兵() {
            "RoR2/Base/Merc/MercBodyEvis.asset".Load<SkillDef>().keywordTokens = new string[] { "KEYWORD_FLEETING" };
            "RoR2/Base/Merc/MercBodyEvisProjectile.asset".Load<SkillDef>().keywordTokens = new string[] { "KEYWORD_FLEETING" };
            On.EntityStates.Merc.WhirlwindBase.OnEnter += delegate (On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, WhirlwindBase self) {
                orig(self);
                self.moveSpeedStat = 7f;
            };
            //======
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += delegate (On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, EntityStates.Merc.Weapon.GroundLight2 self) {
                self.ignoreAttackSpeed = true;
                orig(self);
            };
            On.EntityStates.Merc.Evis.OnEnter += delegate (On.EntityStates.Merc.Evis.orig_OnEnter orig, Evis self) {
                orig(self);
                Evis.damageCoefficient *= self.attackSpeedStat;
            };
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += delegate (On.EntityStates.Merc.Weapon.ThrowEvisProjectile.orig_OnEnter orig, EntityStates.Merc.Weapon.ThrowEvisProjectile self) {
                orig(self);
                self.damageCoefficient *= self.attackSpeedStat;
            };
        }

        private void 雷克斯() {
        }

        private void 女猎人() {
            //====== 1
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
            //====== 2
            //====== 3
            //====== 4
            //====== other
            CharacterBody huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();  // 女猎人调整
            huntressBody.baseCrit = 10f;
            huntressBody.levelCrit = 1f;
        }

        private void 呛鼻毒师() {
            DeepRot.scriptableObject.buffs[0].canStack = true;
            //======
            On.EntityStates.Croco.Bite.OnEnter += delegate (On.EntityStates.Croco.Bite.orig_OnEnter orig, EntityStates.Croco.Bite self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.Tooth.itemIndex);
                }
                orig(self);
            };
        }

        private void 虚空恶鬼() {
            //=== 腐化二技能
            GameObject gameObject = "RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab".Load<GameObject>();
            ProjectileSimple projectileSimple = gameObject.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 40f;
            projectileSimple.lifetime = 6.6f;
            projectileSimple.lifetimeExpiredEffect = "RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosionCorrupted.prefab".Load<GameObject>();
            gameObject.GetComponent<ProjectileImpactExplosion>().blastRadius = 25f; ;
            RadialForce radialForce = gameObject.AddComponent<RadialForce>();
            radialForce.radius = 25f;
            radialForce.damping = 0.5f;
            radialForce.forceMagnitude = -2500f;
            radialForce.forceCoefficientAtEdge = 0.5f;
            gameObject.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            //"RoR2/DLC1/VoidSurvivor/EntityStates.VoidSurvivor.Weapon.FireHandBeam.asset".Load<EntityStates.VoidSurvivor.Weapon.FireHandBeam>().damageCoefficient = 3.6f;
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self) {
                self.damageCoefficient = 2.4f;
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

        private void 异教徒() {
            HereticBodyIndex = BodyCatalog.FindBodyIndex("HereticBody");
            "RoR2/Base/Heretic/HereticDefaultAbility.asset".Load<SkillDef>().baseRechargeInterval = 40;
            //=====
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += delegate (On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
                orig(self);
                if (NetworkServer.active) {
                    foreach (var body in CharacterBody.readOnlyInstancesList) {
                        if (TeamIndex.Lunar == body.teamComponent.teamIndex) {
                            body.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 30);
                        } else {
                            body.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 10);
                        }
                    }
                }
            };
            //=====
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += delegate (On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, FireLunarNeedle self) {
                if (self.characterBody.bodyIndex == HereticBodyIndex) {
                    FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 900f;
                } else {
                    FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 90f;
                }
                orig(self);
            };
            //=====
            On.EntityStates.GhostUtilitySkillState.FixedUpdate += delegate (On.EntityStates.GhostUtilitySkillState.orig_FixedUpdate orig, GhostUtilitySkillState self) {
                orig(self);
                if (self.isAuthority && self.characterBody.bodyIndex == HereticBodyIndex && self.inputBank.skill3.justReleased && self.fixedAge > 1f) {
                    self.outer.SetNextStateToMain();
                }
            };
            //=====
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += delegate (On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self) {
                if (self.characterBody.bodyIndex == HereticBodyIndex) {
                    EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
                } else {
                    EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f;
                }
                orig(self);
            };
        }

        private void 指挥官() {
        }

        private void 装卸工() {
            var loaderBody = RoR2Content.Survivors.Loader.bodyPrefab.GetComponent<CharacterBody>();
            loaderBody.baseAcceleration *= 2f;
            "RoR2/Base/Loader/LoaderPylon.prefab".Load<GameObject>().AddComponent<M551PylonStartAction>();  // 电塔
            GameObject loaderHook = "RoR2/Base/Loader/LoaderHook.prefab".Load<GameObject>();  // 抓钩1
            loaderHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            GameObject loaderYankHook = "RoR2/Base/Loader/LoaderYankHook.prefab".Load<GameObject>();  // 抓钩2
            loaderYankHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderYankHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            //====== 雷冲
            var steppedSkillDef = "RoR2/Base/Loader/ChargeZapFist.asset".Load<SteppedSkillDef>();
            steppedSkillDef.baseRechargeInterval = 3f;
            ProjectileProximityBeamController proximityBeamController = "RoR2/Base/Loader/LoaderZapCone.prefab".LoadComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange *= 2;
            proximityBeamController.bounces = 1;
            BetterUI.ProcCoefficientCatalog.AddSkill(steppedSkillDef.skillName, "SKILL_FIST_NAME", 2.1f);
            //======
            On.EntityStates.Loader.SwingZapFist.OnExit += delegate (On.EntityStates.Loader.SwingZapFist.orig_OnExit orig, EntityStates.Loader.SwingZapFist self) {
                EntityStates.Loader.SwingZapFist.selfKnockback = 100 * self.punchSpeed;
                orig(self);
            };
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += delegate (On.EntityStates.Loader.BaseSwingChargedFist.orig_OnEnter orig, EntityStates.Loader.BaseSwingChargedFist self) {
                if (self is EntityStates.Loader.SwingChargedFist) {
                    self.procCoefficient = Mathf.Lerp(0.6f, 2.7f, self.charge);
                } else {
                    self.procCoefficient = 2.1f;
                }
                orig(self);
            };
        }

        [RequireComponent(typeof(ProjectileController))]
        private class ChainLightning : MonoBehaviour, IProjectileImpactBehavior {
            private ProjectileController projectileController;
            private ProjectileDamage projectileDamage;

            public void OnProjectileImpact(ProjectileImpactInfo impactInfo) {
                LightningOrb lightningOrb = new() {
                    attacker = projectileController.owner?.gameObject,
                    bouncedObjects = new List<HealthComponent>(),
                    bouncesRemaining = int.MaxValue,
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

            private void Awake() {
                if (enabled = NetworkServer.active) {
                    projectileController = gameObject.GetComponent<ProjectileController>();
                    projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                }
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        private class IceExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                ProjectileController projectileController = gameObject.GetComponent<ProjectileController>();
                ProjectileDamage projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                int explosionRadius = 10 + projectileController.owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                if (projectileDamage.crit) {
                    explosionRadius *= 2;
                }
                GameObject iceExplosion = Instantiate(AssetReferences.genericDelayBlast, transform.position, Quaternion.identity);
                iceExplosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
                DelayBlast delayBlast = iceExplosion.GetComponent<DelayBlast>();
                if (delayBlast) {
                    delayBlast.position = transform.position;
                    delayBlast.baseDamage = projectileDamage.damage;
                    delayBlast.baseForce = projectileDamage.force;
                    delayBlast.attacker = projectileController.owner;
                    delayBlast.radius = explosionRadius;
                    delayBlast.crit = projectileDamage.crit;
                    delayBlast.procCoefficient = 0.5f;
                    delayBlast.maxTimer = 2f;
                    delayBlast.falloffModel = BlastAttack.FalloffModel.Linear;
                    delayBlast.explosionEffect = AssetReferences.affixWhiteExplosion;
                    delayBlast.delayEffect = AssetReferences.affixWhiteDelayEffect;
                    delayBlast.damageType = DamageType.Freeze2s;
                    TeamFilter teamFilter = iceExplosion.GetComponent<TeamFilter>();
                    if (teamFilter) {
                        teamFilter.teamIndex = projectileController.teamFilter.teamIndex;
                    }
                }
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        private class LightningExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                ProjectileController projectileController = gameObject.GetComponent<ProjectileController>();
                ProjectileDamage projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                int itemCount = projectileController.owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex);
                List<HealthComponent> bouncedObjects = new();
                BullseyeSearch search = new() {
                    filterByLoS = false,
                    maxDistanceFilter = 30 + 3 * itemCount,
                    searchDirection = Vector3.zero,
                    searchOrigin = transform.position,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    teamMaskFilter = TeamMask.allButNeutral,
                };
                search.teamMaskFilter.RemoveTeam(projectileController.teamFilter.teamIndex);
                for (int i = 0 - itemCount; i < 3; ++i) {
                    LightningOrb lightningOrb = new() {
                        attacker = projectileController.owner,
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
                    HurtBox hurtBox = (from v in search.GetResults() where !bouncedObjects.Contains(v.healthComponent) select v).FirstOrDefault();
                    if (hurtBox) {
                        bouncedObjects.Add(hurtBox.healthComponent);
                        lightningOrb.target = hurtBox;
                        OrbManager.instance.AddOrb(lightningOrb);
                    }
                }
            }
        }

        [RequireComponent(typeof(CharacterBody))]
        [RequireComponent(typeof(ProjectileProximityBeamController))]
        private class M551PylonStartAction : MonoBehaviour {

            public void Start() {
                Inventory inventory = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>().inventory;
                if (inventory) {
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                    ProjectileProximityBeamController proximityBeamController = GetComponent<ProjectileProximityBeamController>();
                    proximityBeamController.attackFireCount += itemCount;
                    proximityBeamController.attackRange += M551Pylon.aoe * itemCount;
                    proximityBeamController.bounces += itemCount;
                }
            }
        }
    }
}