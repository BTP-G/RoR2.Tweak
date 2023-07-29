using AncientScepter;
using EntityStates.Merc;
using EntityStates.Toolbot;
using HIFUCaptainTweaks.Skills;
using HIFUEngineerTweaks.Skills;
using HIFULoaderTweaks.Skills;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class SkillHook {
        public static ProjectileDotZone fireRain_;
        public static ProjectileProximityBeamController projectileProximityBeamController;
        public static ItemIndex 权杖;

        public static Dictionary<int, int> 盗贼标记_ = new();

        public static void AddHook() {
            船长();
            磁轨炮手();
            盗贼();
            多功能抢兵();
            工程师();
            工匠();
            雇佣兵();
            女猎人();
            呛鼻毒师();
            虚空恶鬼();
            异教徒();
            指挥官();
            装卸工();
        }

        public static void Init() {
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== Bandit2
            SkillHook.盗贼标记_.Clear();
            //=== Railgunner
            HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = 0.15f;
            //=== Engi
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges;
            pressure.baseMaxStock = PressureMines.charges;
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f;
            Thunderslam.yVelocityCoeff = 0.3f;
            //=== Captain
            HealingBeacon.Healing = 0.1f;
        }

        public static void LevelUp() {
            int upLevel = BtpTweak.玩家等级_ - 1;
            //=== Huntress
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * upLevel);
            //=== Engi
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges + upLevel;
            pressure.baseMaxStock = PressureMines.charges + upLevel;
            //=== Loader
            EntityStates.Loader.BaseSwingChargedFist.velocityDamageCoefficient = 0.3f + 0.01f * upLevel;
            Thunderslam.yVelocityCoeff = 0.3f + 0.01f * upLevel;
            //=== Captain
            HealingBeacon.Healing = 0.1f + 0.01f * upLevel;
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
                bulletAttack.force *= 1 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.Behemoth.itemIndex);
                orig(self, bulletAttack);
            };
            //======
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile += delegate (On.EntityStates.Captain.Weapon.CallAirstrikeBase.orig_ModifyProjectile orig, EntityStates.Captain.Weapon.CallAirstrikeBase self, ref FireProjectileInfo fireProjectileInfo) {
                CaptainAirstrikeAlt2.airstrikePrefab.GetComponent<跟随目标>().speed = self.characterBody.level * self.characterBody.inventory.GetItemCount(权杖);
                orig(self, ref fireProjectileInfo);
            };
        }

        private static void 磁轨炮手() {
            On.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += delegate (On.EntityStates.Railgunner.Weapon.BaseFireSnipe.orig_ModifyBullet orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    CharacterMaster master = self.characterBody.master;
                    int itemCount = master.inventory.GetItemCount(权杖);
                    if (itemCount > 0) {
                        float moneyToDamage = 0.1f * itemCount * master.money;
                        bulletAttack.damage += moneyToDamage;
                        master.money -= (uint)Mathf.Min(moneyToDamage, master.money);
                    }
                }
            };
            //==========
            On.EntityStates.Railgunner.Reload.Waiting.OnEnter += delegate (On.EntityStates.Railgunner.Reload.Waiting.orig_OnEnter orig, EntityStates.Railgunner.Reload.Waiting self) {
                int magazineCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                HIFURailgunnerTweaks.Misc.ScopeAndReload.Damage = 5 + magazineCount;
                HIFURailgunnerTweaks.Misc.ScopeAndReload.ReloadBarPercent = 0.14f + (0.01f * (BtpTweak.玩家等级_ - magazineCount));
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
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (NetworkServer.active) {
                    CharacterBody body = self.characterBody;
                    if (body.isPlayerControlled) {
                        盗贼标记_[body.playerControllerId] = body.GetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex);
                        for (int i = 盗贼标记_[body.playerControllerId] / (3 * BtpTweak.玩家等级_); i > 0; --i) {
                            body.RemoveBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                        }
                    }
                }
            };
            //======
            On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmResetRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmResetRevolver self, BulletAttack bulletAttack) {
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.BossDamageBonus.itemIndex);
                if (itemCount > 0) {
                    bulletAttack.damage += itemCount * self.damageCoefficient * self.damageStat;
                }
                orig(self, bulletAttack);
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
            On.EntityStates.Toolbot.AimGrenade.OnEnter += delegate (On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
                if (self.isAuthority) {
                    self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.StunChanceOnHit.itemIndex);
                }
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.ToolbotDualWieldBase.OnEnter += delegate (On.EntityStates.Toolbot.ToolbotDualWieldBase.orig_OnEnter orig, ToolbotDualWieldBase self) {
                int 权杖层数 = self.characterBody.inventory.GetItemCount(权杖);
                if (权杖层数 == 0 && ToolbotDualWieldBase.bonusBuff.buffIndex != RoR2Content.Buffs.SmallArmorBoost.buffIndex) {
                    ToolbotDualWieldBase.bonusBuff = RoR2Content.Buffs.SmallArmorBoost;
                } else if (权杖层数 == 1) {
                    ToolbotDualWieldBase.bonusBuff = RoR2Content.Buffs.ArmorBoost;
                } else if (权杖层数 == 2) {
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
                        teamMembers[i].body.AddTimedBuff(RoR2Content.Buffs.EngiShield.buffIndex, 6);
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
                        fireBall.meatballCount = BtpTweak.玩家等级_ / 4;
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
                    BtpTweak.logger_.LogError("Evis Hook Error");
                }
            };
            //======
            On.EntityStates.Merc.Evis.OnEnter += delegate (On.EntityStates.Merc.Evis.orig_OnEnter orig, Evis self) {
                int itemCount = self.characterBody.inventory.GetItemCount(权杖);
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
                if (self.firedArrowCount < self.maxArrowCount) {  // 发射剩余箭矢，防止攻速过快箭矢丢失
                    GenericDamageOrb genericDamageOrb = self.CreateArrowOrb();
                    genericDamageOrb.damageValue = self.damageStat * self.orbDamageCoefficient;
                    genericDamageOrb.isCrit = self.isCrit;
                    genericDamageOrb.teamIndex = self.teamComponent.teamIndex;
                    genericDamageOrb.attacker = self.gameObject;
                    genericDamageOrb.procCoefficient = self.orbProcCoefficient;
                    genericDamageOrb.origin = self.childLocator.FindChild(self.muzzleString).position;
                    genericDamageOrb.target = self.initialOrbTarget;
                    while (self.firedArrowCount++ < self.maxArrowCount) {
                        EffectManager.SimpleMuzzleFlash(self.muzzleflashEffectPrefab, self.gameObject, self.muzzleString, true);
                        OrbManager.instance.AddOrb(genericDamageOrb);
                    }
                }
            };
            //======
            On.EntityStates.Huntress.ArrowRain.OnEnter += delegate (On.EntityStates.Huntress.ArrowRain.orig_OnEnter orig, EntityStates.Huntress.ArrowRain self) {
                fireRain_.lifetime = 6 + 3 * self.characterBody.inventory.GetItemCount(权杖);
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
            //======
        }

        private static void 虚空恶鬼() {
            //=== 腐化二技能
            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab").WaitForCompletion();
            ProjectileSimple component = gameObject.GetComponent<ProjectileSimple>();
            component.desiredForwardSpeed = 40;
            component.lifetime = 6.6f;
            component.lifetimeExpiredEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosionCorrupted.prefab").WaitForCompletion();
            ProjectileImpactExplosion component2 = gameObject.GetComponent<ProjectileImpactExplosion>();
            component2.blastRadius = 25;
            RadialForce radialForce = gameObject.AddComponent<RadialForce>();
            radialForce.radius = 25;
            radialForce.damping = 0.5f;
            radialForce.forceMagnitude = -2500;
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
                BuffIndex buffIndex = RoR2Content.Buffs.LunarSecondaryRoot.buffIndex;
                float duration = 3;
                int itemCount = self.characterBody.inventory.GetItemCount(权杖);
                if (itemCount > 0) {
                    buffIndex = AncientScepterMain.perishSongDebuff.buffIndex;
                    duration = 10 * itemCount;
                }
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (characterBody) {
                        if (TeamIndex.Lunar == characterBody.teamComponent.teamIndex) {
                            duration *= 3;
                        }
                        characterBody.AddTimedBuff(buffIndex, duration);
                    }
                }
            };
        }

        private static void 指挥官() {
            On.EntityStates.Commando.CommandoWeapon.FireBarrage.OnEnter += delegate (On.EntityStates.Commando.CommandoWeapon.FireBarrage.orig_OnEnter orig, EntityStates.Commando.CommandoWeapon.FireBarrage self) {
                orig(self);
                int itemCount = self.characterBody.inventory.GetItemCount(权杖);
                if (itemCount > 1) {
                    self.bulletCount *= itemCount;
                }
            };
        }

        private static void 装卸工() {
            GameObject pylon = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderPylon.prefab").WaitForCompletion();
            GameObject grap = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderHook.prefab").WaitForCompletion();
            projectileProximityBeamController = pylon.GetComponent<ProjectileProximityBeamController>();
            ProjectileGrappleController grapC = grap.GetComponent<ProjectileGrappleController>();
            ProjectileSimple projectileSimple = grap.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed *= 2f;
            grapC.maxTravelDistance *= 1.8f;
            //======
            SteppedSkillDef thunder = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Loader/ChargeZapFist.asset").WaitForCompletion();
            thunder.baseRechargeInterval = 3;
            //======
            On.EntityStates.Loader.ThrowPylon.OnEnter += delegate (On.EntityStates.Loader.ThrowPylon.orig_OnEnter orig, EntityStates.Loader.ThrowPylon self) {
                int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                if (itemCount > 1) {
                    projectileProximityBeamController.attackRange = M551Pylon.aoe + 35 * itemCount;
                    projectileProximityBeamController.bounces = M551Pylon.bounces + itemCount;
                }
                orig(self);
            };
            //======
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += delegate (On.EntityStates.Loader.BaseSwingChargedFist.orig_OnEnter orig, EntityStates.Loader.BaseSwingChargedFist self) {
                if (self.isAuthority) {
                    if ((self is EntityStates.Loader.SwingChargedFist)) {
                        self.maxDuration *= 1.2f;
                        if (self.characterBody.inventory.GetItemCount(权杖) > 0) {
                            self.minPunchForce *= 3;
                            self.maxPunchForce *= 3;
                            self.minLungeSpeed *= 2;
                            self.maxLungeSpeed *= 2;
                        }
                    } else {
                        self.maxDuration *= 1.2f;
                        if (self.characterBody.inventory.GetItemCount(权杖) > 0) {
                            self.characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 1);
                            self.minLungeSpeed *= 2;
                            self.maxLungeSpeed *= 2;
                        }
                    }
                }
                orig(self);
            };
        }
    }
}