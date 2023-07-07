using EntityStates.Toolbot;
using HIFUEngineerTweaks.Skills;
using HIFUMercenaryTweaks.Skills;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class SkillHook {
        //private static bool hasTweakbonusBlastForce = false;

        public static void AddHook() {
            技能冷却();
            船长();
            磁轨炮手();
            盗贼();
            多功能抢兵();
            工程师();
            工匠();
            雇佣兵();
            女猎人();
            虚空恶鬼();
            异教徒();
            指挥官();
            装卸工();
        }

        public static void Init() {
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.8f;
            HIFUHuntressTweaks.Skills.Flurry.damage = 1.2f;
            HIFUHuntressTweaks.Skills.Flurry.minArrows = 3;
            HIFUHuntressTweaks.Skills.Flurry.maxArrows = 6;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== 船 长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6;
            //=== 盗贼
            BtpTweak.盗贼标记_.Clear();
            //=== 导弹无人机
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseMaxHealth = 40;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseDamage = 11f;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseMoveSpeed = 14f;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.projectilePrefab.GetComponent<ProjectileImpactExplosion>().bonusBlastForce = Vector3.zero;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.damageCoefficient = 2;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.baseFireInterval *= 0.75f;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.PerformAutoCalculateLevelStats();
            //=== 磁轨炮手
            HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f;
            //=== 工程师
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges;
            pressure.baseMaxStock = PressureMines.charges;
        }

        public static void LevelUp() {
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.5f + BtpTweak.玩家等级_ * 0.3f;
            HIFUHuntressTweaks.Skills.Flurry.minArrows = 3 + BtpTweak.玩家等级_ / 6;
            HIFUHuntressTweaks.Skills.Flurry.maxArrows = 2 * HIFUHuntressTweaks.Skills.Flurry.minArrows;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * BtpTweak.玩家等级_);
            //=== 船长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6 + BtpTweak.玩家等级_ / 6;
            //=== 工程师
            SkillDef spider = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset").WaitForCompletion();
            SkillDef pressure = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Engi/EngiBodyPlaceMine.asset").WaitForCompletion();
            spider.baseMaxStock = SpiderMines.charges + BtpTweak.玩家等级_ - 1;
            pressure.baseMaxStock = PressureMines.charges + BtpTweak.玩家等级_ - 1;
        }

        private static void 技能冷却() => On.RoR2.GenericSkill.RecalculateFinalRechargeInterval += delegate (On.RoR2.GenericSkill.orig_RecalculateFinalRechargeInterval orig, GenericSkill self) {
            self.finalRechargeInterval = Mathf.Max(0, self.baseRechargeInterval * self.cooldownScale - self.flatCooldownReduction);
        };

        private static void 船长() {
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += delegate (On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, BulletAttack bulletAttack) {
                bulletAttack.force /= HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount;
                orig(self, bulletAttack);
            };
        }

        private static void 磁轨炮手() {
            On.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += delegate (On.EntityStates.Railgunner.Weapon.BaseFireSnipe.orig_ModifyBullet orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (self.isAuthority && self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    if (self.outer.commonComponents.characterBody.inventory.GetItemCount(AncientScepter.AncientScepterItem.instance.ItemDef) > 0) {
                        float moneyToDamage = Mathf.Min(self.outer.commonComponents.characterBody.master.money * (0.01f * (self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine) + 1))
                                                        , self.outer.commonComponents.characterBody.master.money);
                        bulletAttack.damage += moneyToDamage;
                        self.outer.commonComponents.characterBody.master.money -= (uint)moneyToDamage;
                    }
                }
            };
            //==========
            On.EntityStates.Railgunner.Reload.Waiting.OnEnter += delegate (On.EntityStates.Railgunner.Reload.Waiting.orig_OnEnter orig, EntityStates.Railgunner.Reload.Waiting self) {
                int magazineCount = self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine);
                HRGT.Misc.ScopeAndReload.Damage = 5 + (0.5f * magazineCount);
                HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f + (0.01f * (self.characterBody.level - 1 - magazineCount));
                orig(self);
            };
            //==========
            On.EntityStates.Railgunner.Scope.BaseWindUp.OnEnter += delegate (On.EntityStates.Railgunner.Scope.BaseWindUp.orig_OnEnter orig, EntityStates.Railgunner.Scope.BaseWindUp self) {
                HurtBox.sniperTargetRadius = 1 + 0.5f * self.outer.commonComponents.characterBody.inventory.GetItemCount(DLC1Content.Items.CritDamage);
                HurtBox.sniperTargetRadiusSqr = HurtBox.sniperTargetRadius * HurtBox.sniperTargetRadius;
                orig(self);
            };
            //==========
            On.RoR2.Projectile.SlowDownProjectiles.Start += delegate (On.RoR2.Projectile.SlowDownProjectiles.orig_Start orig, SlowDownProjectiles self) {
                orig(self);
                if (self.name.StartsWith("Rail")) {
                    self.slowDownCoefficient = 0.03f;
                }
            };
        }

        private static void 盗贼() {
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                if (self.isAuthority) {
                    BtpTweak.盗贼标记_[self.characterBody.playerControllerId] = self.GetBuffCount(RoR2Content.Buffs.BanditSkull);
                    self.characterBody.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, BtpTweak.盗贼标记_[self.characterBody.playerControllerId] -= BtpTweak.盗贼标记_[self.characterBody.playerControllerId] / (3 * BtpTweak.玩家等级_));
                }
            };
        }

        private static void 多功能抢兵() {
            On.EntityStates.Toolbot.BaseNailgunState.PullCurrentStats += delegate (On.EntityStates.Toolbot.BaseNailgunState.orig_PullCurrentStats orig, BaseNailgunState self) {
                if (self.isAuthority) {
                    BaseNailgunState.damageCoefficient = Mathf.Min(2.1f, 0.77f + self.fireNumber * 0.001f);
                }
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.NailgunFinalBurst.OnEnter += delegate (On.EntityStates.Toolbot.NailgunFinalBurst.orig_OnEnter orig, NailgunFinalBurst self) {
                if (self.isAuthority) {
                    BaseNailgunState.damageCoefficient += 0.07f * self.characterBody.level;
                }
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.FireBuzzsaw.FixedUpdate += delegate (On.EntityStates.Toolbot.FireBuzzsaw.orig_FixedUpdate orig, FireBuzzsaw self) {
                orig(self);
                if (self.isAuthority) {
                    self.attack.damage += (Time.fixedDeltaTime * self.damageStat) / (1 + self.fixedAge);
                }
            };
            On.EntityStates.Toolbot.AimGrenade.OnEnter += delegate (On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
                if (self.isAuthority) {
                    BtpTweak.logger_.LogInfo("self.detonationRadius ============== " + self.detonationRadius);
                    self.detonationRadius *= 2;
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
                        teamMembers[i].body?.AddTimedBuff(RoR2Content.Buffs.EngiShield.buffIndex, 6);
                    }
                }
            };
        }

        private static void 工匠() {
            SteppedSkillDef mageFireFirebolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireFirebolt.asset").WaitForCompletion();
            SteppedSkillDef mageFireLightningBolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireLightningBolt.asset").WaitForCompletion();
            mageFireFirebolt.baseRechargeInterval = 1f;
            mageFireLightningBolt.baseRechargeInterval = 1f;
            On.EntityStates.Mage.Weapon.BaseThrowBombState.OnEnter += delegate (On.EntityStates.Mage.Weapon.BaseThrowBombState.orig_OnEnter orig, EntityStates.Mage.Weapon.BaseThrowBombState self) {
                orig(self);
                if (self is EntityStates.Mage.Weapon.ThrowNovabomb) {
                    Meatball meatball = self.projectilePrefab.GetComponent<Meatball>();
                    if (meatball) {
                        return;
                    } else {
                        self.projectilePrefab.AddComponent<Meatball>();
                    }
                } else if (self is EntityStates.Mage.Weapon.ThrowIcebomb) {
                    IceExplosion iceExplosion = self.projectilePrefab.GetComponent<IceExplosion>();
                    if (iceExplosion) {
                        iceExplosion.explosionRadius = 6 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
                    } else {
                        self.projectilePrefab.AddComponent<IceExplosion>();
                    }
                }
            };
        }

        private static void 雇佣兵() {
            On.EntityStates.Merc.Evis.OnEnter += delegate (On.EntityStates.Merc.Evis.orig_OnEnter orig, EntityStates.Merc.Evis self) {
                orig(self);
                Eviscerate.instance.ignoreAllies = false;
            };
            On.EntityStates.Merc.Evis.SearchForTarget += delegate (On.EntityStates.Merc.Evis.orig_SearchForTarget orig, EntityStates.Merc.Evis self) {
                BullseyeSearch bullseyeSearch = new BullseyeSearch {
                    searchOrigin = self.transform.position,
                    searchDirection = self.inputBank.aimDirection,
                    maxDistanceFilter = EntityStates.Merc.Evis.maxRadius,
                    teamMaskFilter = TeamMask.AllExcept(self.GetTeam()),
                    sortMode = BullseyeSearch.SortMode.Angle
                };
                bullseyeSearch.RefreshCandidates();
                bullseyeSearch.FilterOutGameObject(self.gameObject);
                HurtBox result = bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
                if (result != null && self.isAuthority) {
                    EntityStates.Merc.Evis.damageCoefficient += 0.013f;
                }
                return result;
            };
            On.EntityStates.Merc.Evis.OnExit += delegate (On.EntityStates.Merc.Evis.orig_OnExit orig, EntityStates.Merc.Evis self) {
                orig(self);
                Eviscerate.instance.ignoreAllies = true;
            };
            On.EntityStates.Merc.WhirlwindBase.OnEnter += delegate (On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, EntityStates.Merc.WhirlwindBase self) {
                orig(self);
                self.selfForceMagnitude = 0;
                self.moveSpeedBonusCoefficient = 36 / self.moveSpeedStat;
            };
            On.EntityStates.Merc.WhirlwindBase.FixedUpdate += delegate (On.EntityStates.Merc.WhirlwindBase.orig_FixedUpdate orig, EntityStates.Merc.WhirlwindBase self) {
                orig(self);
                if (self is EntityStates.Merc.WhirlwindGround) {
                    self.characterMotor.velocity.y = -10;
                } else {
                    self.characterMotor.velocity.y = 0;
                }
            };
            On.EntityStates.Merc.Weapon.GroundLight2.PlayAnimation += delegate (On.EntityStates.Merc.Weapon.GroundLight2.orig_PlayAnimation orig, EntityStates.Merc.Weapon.GroundLight2 self) {
                if (self.duration < 0.22f * self.baseDuration) {
                    self.duration = 0.22f * self.baseDuration;
                }
                orig(self);
            };
        }

        private static void 女猎人() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += delegate (On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnExit orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
                orig(self);
                if (NetworkServer.active && self.firedArrowCount < self.maxArrowCount) {  // 发射剩余箭矢，防止攻速过快箭矢丢失
                    GenericDamageOrb genericDamageOrb = self.CreateArrowOrb();
                    genericDamageOrb.damageValue = self.characterBody.damage * self.orbDamageCoefficient;
                    genericDamageOrb.isCrit = self.isCrit;
                    genericDamageOrb.teamIndex = self.teamComponent.teamIndex;
                    genericDamageOrb.attacker = self.gameObject;
                    genericDamageOrb.procCoefficient = self.orbProcCoefficient;
                    HurtBox hurtBox = self.initialOrbTarget;
                    if (hurtBox) {
                        while (self.firedArrowCount++ < self.maxArrowCount) {
                            genericDamageOrb.origin = self.childLocator.FindChild(self.muzzleString).position;
                            genericDamageOrb.target = hurtBox;
                            OrbManager.instance.AddOrb(genericDamageOrb);
                        }
                    }
                }
            };
        }

        private static void 虚空恶鬼() {
            //=== 腐化二技能
            GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab").WaitForCompletion();
            ProjectileSimple component = gameObject.GetComponent<ProjectileSimple>();
            component.desiredForwardSpeed = 44;
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
                if (self.isAuthority) {
                    self.damageCoefficient = 3.6f;
                }
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
                self.baseDuration = 4;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += delegate (On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
                if (self.isAuthority) {
                    if (self is EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBig) {
                        self.force = 4444;
                        self.damageCoefficient = 44.44f;
                    } else {
                        self.force = 666;
                        self.damageCoefficient = 6.66f;
                    }
                }
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
                if (self.isAuthority) {
                    self.damageCoefficient = 25;
                }
                self.selfKnockbackForce = 0;
                orig(self);
            };
        }

        private static void 异教徒() {
            //==========
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += delegate (On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
                orig(self);
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (characterBody) {
                        if (TeamIndex.Lunar == characterBody.teamComponent.teamIndex) {
                            characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot.buffIndex, 9);
                        } else {
                            characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot.buffIndex, 3);
                        }
                    }
                }
            };
        }

        private static void 指挥官() {
        }

        private static void 装卸工() {
        }
    }
}