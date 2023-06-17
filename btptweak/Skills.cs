using EntityStates.Toolbot;
using R2API;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace Btp {

    internal class Skills {

        public static void 技能调整() {
            技能冷却();
            指挥官();
            女猎人();
            工匠();
            盗贼();
            多功能抢兵();
            船长();
            磁轨炮手();
            虚空恶鬼();
            异教徒();
        }

        private static void 指挥官() {
        }

        private static void 技能冷却() => On.RoR2.GenericSkill.RecalculateFinalRechargeInterval += delegate (On.RoR2.GenericSkill.orig_RecalculateFinalRechargeInterval orig, GenericSkill self) {
            self.finalRechargeInterval = Mathf.Max(0, self.baseRechargeInterval * self.cooldownScale - self.flatCooldownReduction);
        };

        public static void RunInit() {
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.8f;
            HIFUHuntressTweaks.Skills.Flurry.damage = 1.2f;
            HIFUHuntressTweaks.Skills.Flurry.minArrows = 3;
            HIFUHuntressTweaks.Skills.Flurry.maxArrows = 6;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60;
            //=== 船 长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6;
            //=== 盗贼
            BtpTweak.盗贼标记_ = 0;
            //=== 导弹无人机
            MissileDroneSurvivor.MsIsleEntityStates.NukeAbility.projectilePrefabNuke.GetComponent<ProjectileImpactExplosion>().blastRadius = 35;
            //=== 工程师
            HIFUEngineerTweaks.Skills.PressureMines.charges = 4;
            HIFUEngineerTweaks.Skills.SpiderMines.charges = 2;
            //=== 磁轨炮手
            HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f;
        }

        public static void 按等级重新调整技能() {
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.5f + BtpTweak.玩家等级_ * 0.3f;
            HIFUHuntressTweaks.Skills.Flurry.minArrows = 3 + BtpTweak.玩家等级_ / 3;
            HIFUHuntressTweaks.Skills.Flurry.maxArrows = 2 * HIFUHuntressTweaks.Skills.Flurry.minArrows;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * BtpTweak.玩家等级_);
            //=== 船长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6 + BtpTweak.玩家等级_ / 3;
            //=== 导弹无人机
            MissileDroneSurvivor.MsIsleEntityStates.NukeAbility.projectilePrefabNuke.GetComponent<ProjectileImpactExplosion>().blastRadius = 34 + BtpTweak.玩家等级_;
            //=== 工程师
            HIFUEngineerTweaks.Skills.PressureMines.charges = 4 + BtpTweak.玩家等级_;
            HIFUEngineerTweaks.Skills.SpiderMines.charges = 2 + BtpTweak.玩家等级_;
            //=== 磁轨炮手
        }

        private static void 女猎人() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += delegate (On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnExit orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
                if (NetworkServer.active) {
                    while (self.firedArrowCount++ < self.maxArrowCount) {  // 发射剩余箭矢，防止攻速过快箭矢丢失
                        GenericDamageOrb genericDamageOrb = self.CreateArrowOrb();
                        genericDamageOrb.damageValue = self.characterBody.damage * self.orbDamageCoefficient;
                        genericDamageOrb.isCrit = self.isCrit;
                        genericDamageOrb.teamIndex = self.teamComponent.teamIndex;
                        genericDamageOrb.attacker = self.gameObject;
                        genericDamageOrb.procCoefficient = self.orbProcCoefficient;
                        HurtBox hurtBox = self.initialOrbTarget;
                        if (hurtBox) {
                            Transform transform = self.childLocator.FindChild(self.muzzleString);
                            genericDamageOrb.origin = transform.position;
                            genericDamageOrb.target = hurtBox;
                            OrbManager.instance.AddOrb(genericDamageOrb);
                        }
                    }
                }
                orig(self);
            };
        }

        private static void 工匠() {
            SkillDef mageFlamethrower = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Mage/MageBodyFlamethrower.asset").WaitForCompletion();
            SteppedSkillDef mageFireFirebolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireFirebolt.asset").WaitForCompletion();
            SteppedSkillDef mageFireLightningBolt = Addressables.LoadAssetAsync<SteppedSkillDef>("RoR2/Base/Mage/MageBodyFireLightningBolt.asset").WaitForCompletion();
            mageFlamethrower.baseRechargeInterval = 0;
            mageFireFirebolt.baseRechargeInterval *= 0.5f;
            mageFireLightningBolt.baseRechargeInterval *= 0.5f;
            GameObject mageNovaBomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
            mageNovaBomb.AddComponent<Meatball>();
            GameObject mageiceBomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIceBombProjectile.prefab").WaitForCompletion();
            mageiceBomb.AddComponent<IceExplosion>();
        }

        private static void 盗贼() {
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += delegate (On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver self, BulletAttack bulletAttack) {
                orig(self, bulletAttack);
                BtpTweak.盗贼标记_ = self.GetBuffCount(RoR2Content.Buffs.BanditSkull);
                self.characterBody.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, BtpTweak.盗贼标记_ -= BtpTweak.盗贼标记_ / (3 * BtpTweak.玩家等级_));
            };
        }

        private static void 多功能抢兵() {
            On.EntityStates.Toolbot.BaseNailgunState.PullCurrentStats += delegate (On.EntityStates.Toolbot.BaseNailgunState.orig_PullCurrentStats orig, BaseNailgunState self) {
                BaseNailgunState.damageCoefficient = Mathf.Min(2.1f, 0.77f + self.fireNumber * 0.001f);
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.NailgunFinalBurst.OnEnter += delegate (On.EntityStates.Toolbot.NailgunFinalBurst.orig_OnEnter orig, NailgunFinalBurst self) {
                BaseNailgunState.damageCoefficient += BtpTweak.玩家等级_ * 0.7f;
                orig(self);
            };
            //==========
            On.EntityStates.Toolbot.FireSpear.OnEnter += delegate (On.EntityStates.Toolbot.FireSpear.orig_OnEnter orig, FireSpear self) {
                self.damageCoefficient += BtpTweak.玩家等级_ / 7;
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
                if (self is EntityStates.Railgunner.Weapon.FireSnipeSuper) {
                    if (self.outer.commonComponents.characterBody.skillLocator.special.skillDef.skillName.EndsWith("Scepter")) {
                        float moneyToDamage = self.outer.commonComponents.characterBody.master.money * ((self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine) + 1) / 100.0f);
                        bulletAttack.damage += moneyToDamage;
                        self.outer.commonComponents.characterBody.master.money -= (uint)moneyToDamage;
                    }
                }
            };
            //==========
            On.EntityStates.Railgunner.Reload.Reloading.OnEnter += delegate (On.EntityStates.Railgunner.Reload.Reloading.orig_OnEnter orig, EntityStates.Railgunner.Reload.Reloading self) {
                int magazineCount = self.outer.commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine);
                HRGT.Misc.ScopeAndReload.Damage = 5 + (magazineCount * 0.5f);
                HRGT.Misc.ScopeAndReload.ReloadBarPercent = 0.15f + ((BtpTweak.玩家等级_ - magazineCount) * 0.01f);
                orig(self);
            };
        }

        private static void 虚空恶鬼() {
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self) {
                self.damageCoefficient = 4.44f;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += delegate (On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
                BtpTweak.虚空恶鬼二技能充能时间_ = 0;
                self.baseDuration = 4;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.FixedUpdate += delegate (On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_FixedUpdate orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
                BtpTweak.虚空恶鬼二技能充能时间_ += Time.fixedDeltaTime;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += delegate (On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
                float 充能百分比 = Mathf.Min(BtpTweak.虚空恶鬼二技能充能时间_ * self.attackSpeedStat * 0.25f, 1);
                self.selfKnockbackForce = 0;
                self.force = 4444 * 充能百分比;
                self.damageCoefficient = 44.44f * 充能百分比;
                orig(self);
            };
            //==========
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.FireProjectiles += delegate (On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
                self.selfKnockbackForce *= 0.5f;
                self.damageCoefficient = 22.22f;
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
            ContentAddition.AddEntityState<AncientScepter.HereticPerishSong>(out _);
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