using EntityStates.Toolbot;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BtpTweak {

    internal class SkillHook {

        //private static bool hasTweakbonusBlastForce = false;
        public static float iceExplosionRadius = 1;

        public static void AddHook() {
            技能冷却();
            指挥官();
            女猎人();
            工匠();
            盗贼();
            装卸工();
            多功能抢兵();
            船长();
            磁轨炮手();
            虚空恶鬼();
            异教徒();
        }

        private static void 技能冷却() => On.RoR2.GenericSkill.RecalculateFinalRechargeInterval += delegate (On.RoR2.GenericSkill.orig_RecalculateFinalRechargeInterval orig, GenericSkill self) {
            self.finalRechargeInterval = Mathf.Max(0, self.baseRechargeInterval * self.cooldownScale - self.flatCooldownReduction);
        };

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
            MissileDroneSurvivor.MsIsleEntityStates.NukeAbility.projectilePrefabNuke.GetComponent<ProjectileImpactExplosion>().blastRadius = 30;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseMaxHealth = 40;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseDamage = 11f;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.baseMoveSpeed = 18f;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.projectilePrefab.GetComponent<ProjectileImpactExplosion>().bonusBlastForce = Vector3.zero;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.damageCoefficient = 2;
            MissileDroneSurvivor.MsIsleEntityStates.MissileBarrage.baseFireInterval *= 0.75f;
            MissileDroneSurvivor.MissileDroneMod.bodyComponent.PerformAutoCalculateLevelStats();
            //=== 磁轨炮手
            HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f;
            //=== 工匠
            iceExplosionRadius = 1;
            //=== 多功能枪兵
        }

        public static void LevelUp() {
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.5f + BtpTweak.玩家等级_ * 0.3f;
            HIFUHuntressTweaks.Skills.Flurry.minArrows = 3 + BtpTweak.玩家等级_ / 3;
            HIFUHuntressTweaks.Skills.Flurry.maxArrows = 2 * HIFUHuntressTweaks.Skills.Flurry.minArrows;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * BtpTweak.玩家等级_);
            //=== 船长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6 + BtpTweak.玩家等级_ / 3;
            //=== 工匠
            iceExplosionRadius = BtpTweak.玩家等级_ > 30 ? 30 : BtpTweak.玩家等级_;
        }

        private static void 指挥官() {
        }

        private static void 女猎人() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += delegate (On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnExit orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
                orig(self);
                if (self.isAuthority && self.firedArrowCount < self.maxArrowCount) {  // 发射剩余箭矢，防止攻速过快箭矢丢失
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

        private static void 工匠() {
            SkillDef mageFlamethrower = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Mage/MageBodyFlamethrower.asset").WaitForCompletion();
            SteppedSkillDef mageFireFirebolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireFirebolt.asset").WaitForCompletion();
            SteppedSkillDef mageFireLightningBolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireLightningBolt.asset").WaitForCompletion();
            mageFlamethrower.baseRechargeInterval = 0;
            mageFireFirebolt.baseRechargeInterval *= 0.5f;
            mageFireLightningBolt.baseRechargeInterval *= 0.5f;
            //GameObject mageNovaBomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
            //mageNovaBomb.AddComponent<Meatball>();
            //GameObject mageiceBomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIceBombProjectile.prefab").WaitForCompletion();
            //mageiceBomb.AddComponent<IceExplosion>();
            On.EntityStates.Mage.Weapon.BaseThrowBombState.OnEnter += delegate (On.EntityStates.Mage.Weapon.BaseThrowBombState.orig_OnEnter orig, EntityStates.Mage.Weapon.BaseThrowBombState self) {
                orig(self);
                if (self is EntityStates.Mage.Weapon.ThrowNovabomb) {
                    if (self.projectilePrefab.GetComponent<Meatball>() == null) {
                        self.projectilePrefab.AddComponent<Meatball>();
                    }
                } else if (self is EntityStates.Mage.Weapon.ThrowIcebomb) {
                    if (self.projectilePrefab.GetComponent<IceExplosion>() == null) {
                        self.projectilePrefab.AddComponent<IceExplosion>();
                    }
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

        private static void 装卸工() {
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
            On.EntityStates.Toolbot.FireSpear.OnEnter += delegate (On.EntityStates.Toolbot.FireSpear.orig_OnEnter orig, FireSpear self) {
                if (self.isAuthority) {
                    self.damageCoefficient += BtpTweak.玩家等级_ / 7;
                }
                orig(self);
            };
            On.EntityStates.Toolbot.FireBuzzsaw.FixedUpdate += delegate (On.EntityStates.Toolbot.FireBuzzsaw.orig_FixedUpdate orig, FireBuzzsaw self) {
                orig(self);
                if (self.isAuthority) {
                    self.attack.damage += (Time.fixedDeltaTime * self.damageStat) / (1 + self.fixedAge);
                }
            };
            On.EntityStates.Toolbot.AimGrenade.OnEnter += delegate (On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
                if (self.isAuthority) {
                    self.projectilePrefab.GetComponent<ProjectileImpactExplosion>().bonusBlastForce = -2500 * Vector3.one;
                }
                orig(self);
            };
        }

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
                    if (self.outer.commonComponents.characterBody.skillLocator.special.skillDef.skillName.EndsWith("Scepter")) {
                        float moneyToDamage = Mathf.Min(self.outer.commonComponents.characterBody.master.money * (0.01f * (self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine) + 1))
                                                        , self.outer.commonComponents.characterBody.master.money);
                        bulletAttack.damage += moneyToDamage;
                        self.outer.commonComponents.characterBody.master.money -= (uint)moneyToDamage;
                    }
                }
            };
            //==========
            On.EntityStates.Railgunner.Reload.Reloading.OnEnter += delegate (On.EntityStates.Railgunner.Reload.Reloading.orig_OnEnter orig, EntityStates.Railgunner.Reload.Reloading self) {
                int magazineCount = self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine);
                HRGT.Misc.ScopeAndReload.Damage = 5 + (magazineCount * 0.5f);
                HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f + ((self.characterBody.level - magazineCount) * 0.01f);
                orig(self);
            };
        }

        private static void 虚空恶鬼() {
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
                self.selfKnockbackForce *= 0.5f;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.FireProjectiles += delegate (On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
                if (self.isAuthority) {
                    self.damageCoefficient = 25;
                }
                self.selfKnockbackForce = 0;
                orig(self);
            };
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
    }
}