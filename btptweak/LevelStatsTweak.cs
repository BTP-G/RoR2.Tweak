using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class LevelStatsTweak {

        public static void 角色修改() {
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase += PhaseCounter_GoToNextPhase;
            虚空恶鬼技能修改();
            女猎人弓箭修改();
        }

        private static void 虚空恶鬼技能修改() {
            虚空恶鬼腐化二技能修改();
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.FireProjectiles += FireCorruptDisks_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += FireMegaBlasterBase_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.FixedUpdate += ReadyMegaBlaster_FixedUpdate;
        }

        private static void 女猎人弓箭修改() {
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnEnter += FireSeekingArrow_OnEnter;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit += FireSeekingArrow_OnExit;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.OnLevelUp -= CharacterBody_OnLevelUp;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase -= PhaseCounter_GoToNextPhase;
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter -= FireHandBeam_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.FireProjectiles -= FireCorruptDisks_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter -= ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles -= FireMegaBlasterBase_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.FixedUpdate -= ReadyMegaBlaster_FixedUpdate;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnEnter -= FireSeekingArrow_OnEnter;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.OnExit -= FireSeekingArrow_OnExit;
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
            self.baseDuration = 6.66f;
            orig(self);
        }

        private static void ReadyMegaBlaster_FixedUpdate(On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.orig_FixedUpdate orig, EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster self) {
            orig(self);
            BtpTweak.holdTime += Time.fixedDeltaTime;
        }

        private static void FireMegaBlasterBase_FireProjectiles(On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
            float 充能百分比 = Mathf.Min(BtpTweak.holdTime * self.attackSpeedStat / 66.6f, 1);
            BtpTweak.holdTime = 0;
            self.selfKnockbackForce = 0;
            self.force = 6666 * 充能百分比;
            self.damageCoefficient = 66.66f * 充能百分比;
            orig(self);
        }

        private static void FireCorruptDisks_FireProjectiles(On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
            self.selfKnockbackForce /= 2;
            self.damageCoefficient = 16.66f;
            orig(self);
        }

        private static void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self) {
            orig(self);
            if (self.isPlayerControlled) {
                BtpTweak.玩家角色等级_ = self.level;
                //=== 女猎人
                HIFUHuntressTweaks.Skills.Strafe.damage = 1.6f + BtpTweak.玩家角色等级_ * 0.2f;
                HIFUHuntressTweaks.Skills.Flurry.minArrows = 3 + Mathf.FloorToInt(BtpTweak.玩家角色等级_ / 3);
                HIFUHuntressTweaks.Skills.Flurry.maxArrows = 2 * HIFUHuntressTweaks.Skills.Flurry.minArrows;
                HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * BtpTweak.玩家角色等级_);
                //=== 船长
                HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6 + Mathf.FloorToInt(BtpTweak.玩家角色等级_ / 3f);
                //===
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            if (TeamIndex.Player == self.teamComponent.teamIndex) {
                if (BtpTweak.是否选择造物难度_) {
                    self.levelMaxHealth =
                        BtpTweak.玩家角色等级生命值系数_ *
                        (self.baseMaxHealth <= 440 ? self.baseMaxHealth / 44f : 10f) *
                        (BtpTweak.玩家角色等级_ <= 100 ? BtpTweak.玩家角色等级_ : 100);
                }
            }
            orig(self);
        }

        private static void PhaseCounter_GoToNextPhase(On.RoR2.PhaseCounter.orig_GoToNextPhase orig, PhaseCounter self) {
            if (BtpTweak.是否选择造物难度_ && PhaseCounter.instance?.phase != 0) {
                BtpTweak.玩家角色等级生命值系数_ *= 1.5f;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=130%>击败米斯历克斯第{PhaseCounter.instance?.phase}阶段，奖励最大生命值！</size></color>" });
            }
            orig(self);
        }
    }
}