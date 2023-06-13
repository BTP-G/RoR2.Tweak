using R2API;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class Skills {

        public static void 技能调整() {
            虚空恶鬼技能修改();
            女猎人弓箭修改();
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += Squawk_OnEnter;
            ContentAddition.AddEntityState<AncientScepter.HereticPerishSong>(out _);
            On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.ModifyBullet += FireSidearmSkullRevolver_ModifyBullet;
        }

        private static void Squawk_OnEnter(On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
            orig(self);
            foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                if (characterBody) {
                    if (TeamIndex.Lunar == characterBody.teamComponent.teamIndex) {
                        characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 9);
                    } else {
                        characterBody.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 3);
                    }
                }
            }
        }

        private static void FireSidearmSkullRevolver_ModifyBullet(On.EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver.orig_ModifyBullet orig, EntityStates.Bandit2.Weapon.FireSidearmSkullRevolver self, BulletAttack bulletAttack) {
            orig(self, bulletAttack);
            BtpTweak.banditSkullCount_ = self.GetBuffCount(RoR2Content.Buffs.BanditSkull);
            self.characterBody.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, BtpTweak.banditSkullCount_ -= BtpTweak.banditSkullCount_ / (5 * BtpTweak.玩家角色等级_));
        }

        private static void 虚空恶鬼技能修改() {
            虚空恶鬼腐化二技能修改();
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.FireProjectiles += FireCorruptDisks_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.FixedUpdate += ChargeMegaBlaster_FixedUpdate;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += FireMegaBlasterBase_FireProjectiles;
        }

        private static void 虚空恶鬼腐化二技能修改() {
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

        private static void FireHandBeam_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self) {
            self.damageCoefficient = 4.44f;
            orig(self);
        }

        private static void ChargeMegaBlaster_OnEnter(On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
            BtpTweak.megaBlasterChargedTime_ = 0;
            self.baseDuration = 4;
            orig(self);
        }

        private static void ChargeMegaBlaster_FixedUpdate(On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_FixedUpdate orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
            BtpTweak.megaBlasterChargedTime_ += Time.fixedDeltaTime;
            orig(self);
        }

        private static void FireMegaBlasterBase_FireProjectiles(On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
            float 充能百分比 = Mathf.Min(BtpTweak.megaBlasterChargedTime_ * self.attackSpeedStat * 0.25f, 1);
            self.selfKnockbackForce = 0;
            self.force = 4444 * 充能百分比;
            self.damageCoefficient = 44.44f * 充能百分比;
            orig(self);
        }

        private static void FireCorruptDisks_FireProjectiles(On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
            self.selfKnockbackForce *= 0.5f;
            self.damageCoefficient = 22.22f;
            orig(self);
        }

        private static void 女猎人弓箭修改() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnEnter += FireSeekingArrow_OnEnter;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += FireSeekingArrow_OnExit;
        }

        private static void FireSeekingArrow_OnEnter(On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnEnter orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
            orig(self);
            self.arrowReloadDuration /= (self.isCrit ? HIFUHuntressTweaks.Skills.Flurry.maxArrows : HIFUHuntressTweaks.Skills.Flurry.minArrows);
        }

        private static void FireSeekingArrow_OnExit(On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_OnExit orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
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
        }
    }
}