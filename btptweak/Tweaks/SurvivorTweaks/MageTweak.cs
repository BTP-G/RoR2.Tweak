using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.Mage;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class MageTweak : TweakBase<MageTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float 电击半径 = 25;
        public const float 电击伤害系数 = 0.6f;
        public const int 每次最大电击数 = 10;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Mage.FlyUpState.OnEnter += FlyUpState_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            //===火焰弹===//
            var mageFireFirebolt = SteppedSkillDefPaths.MageBodyFireFirebolt.Load<SteppedSkillDef>();
            mageFireFirebolt.baseRechargeInterval = 0;
            mageFireFirebolt.baseMaxStock = 1;
            //===闪电弹===//
            var mageFireLightningBolt = SteppedSkillDefPaths.MageBodyFireLightningBolt.Load<SteppedSkillDef>();
            mageFireLightningBolt.baseRechargeInterval = 0;
            mageFireLightningBolt.baseMaxStock = 1;
            //===冰枪===//
            GameObjectPaths.MageIceBombProjectile.Load<GameObject>().AddComponent<IceExplosion>();
            SkillDefPaths.MageBodyIceBomb.Load<SkillDef>().mustKeyPress = false;
            //===雷电球===//
            GameObject mageLightningBomb = GameObjectPaths.MageLightningBombProjectile.Load<GameObject>();
            mageLightningBomb.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            var mageProximityBeamController = mageLightningBomb.GetComponent<ProjectileProximityBeamController>();
            mageProximityBeamController.attackFireCount = 每次最大电击数;
            mageProximityBeamController.attackInterval = 0.2f;
            mageProximityBeamController.attackRange = 电击半径;
            mageProximityBeamController.damageCoefficient = 电击伤害系数 * 0.2f;
            mageProximityBeamController.inheritDamageType = true;
            mageProximityBeamController.listClearInterval = 0.2f;
            mageProximityBeamController.procCoefficient = 0.3f;
            SkillDefPaths.MageBodyNovaBomb.Load<SkillDef>().mustKeyPress = false;
            //===角色===//
            var body = RoR2Content.Survivors.Mage.bodyPrefab.GetComponent<CharacterBody>();
            body.baseMoveSpeed = 10f;
            body.baseJumpPower = 20f;
            var motor = RoR2Content.Survivors.Mage.bodyPrefab.GetComponent<CharacterMotor>();
            motor.airControl = 1.25f;
        }

        private void FlyUpState_OnEnter(On.EntityStates.Mage.FlyUpState.orig_OnEnter orig, FlyUpState self) {
            orig(self);
            var body = self.characterBody;
            self.moveSpeedStat = body.baseMoveSpeed * (body.isSprinting ? 1.5f : 1f);
        }

        private class IceExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                var projectileController = gameObject.GetComponent<ProjectileController>();
                var projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                var iceExplosion = Instantiate(AssetReferences.genericDelayBlast, transform.position, Quaternion.identity);
                iceExplosion.transform.localScale = new Vector3(12, 12, 12);
                var delayBlast = iceExplosion.GetComponent<DelayBlast>();
                delayBlast.position = transform.position;
                delayBlast.baseDamage = projectileDamage.damage;
                delayBlast.baseForce = projectileDamage.force;
                delayBlast.attacker = projectileController.owner;
                delayBlast.radius = 12f;
                delayBlast.crit = projectileDamage.crit;
                delayBlast.procCoefficient = 1f;
                delayBlast.maxTimer = 2f;
                delayBlast.falloffModel = BlastAttack.FalloffModel.Linear;
                delayBlast.explosionEffect = AssetReferences.affixWhiteExplosion;
                delayBlast.delayEffect = AssetReferences.affixWhiteDelayEffect;
                delayBlast.damageType = DamageType.Freeze2s;
                delayBlast.teamFilter.teamIndex = projectileController.teamFilter.teamIndex;
            }
        }

        private class MageLightningBombStartAction : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void Start() {
                var owner = GetComponent<ProjectileController>().owner;
                if (owner) {
                    GetComponent<ProjectileProximityBeamController>().attackRange += 3 * owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex);
                }
            }
        }

        private class LightningExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                var projectileController = gameObject.GetComponent<ProjectileController>();
                var projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                int itemCount = projectileController.owner?.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex) ?? 0;
                var bouncedObjects = new List<HealthComponent>();
                var search = new BullseyeSearch() {
                    filterByLoS = false,
                    maxDistanceFilter = 30 + 3 * itemCount,
                    searchDirection = Vector3.zero,
                    searchOrigin = transform.position,
                    sortMode = BullseyeSearch.SortMode.Distance,
                    teamMaskFilter = TeamMask.allButNeutral,
                };
                search.teamMaskFilter.RemoveTeam(projectileController.teamFilter.teamIndex);
                for (int i = 0 - itemCount; i < 3; ++i) {
                    var lightningOrb = new LightningOrb() {
                        attacker = projectileController.owner,
                        bouncedObjects = [],
                        bouncesRemaining = 0,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = projectileDamage.damageType,
                        damageValue = projectileDamage.damage * 0.8f,
                        isCrit = projectileDamage.crit,
                        lightningType = LightningOrb.LightningType.MageLightning,
                        origin = search.searchOrigin,
                        procChainMask = default,
                        procCoefficient = 0.2f,
                        range = search.maxDistanceFilter,
                        teamIndex = projectileController.teamFilter.teamIndex
                    };
                    search.RefreshCandidates();
                    var hurtBox = (from v in search.GetResults() where !bouncedObjects.Contains(v.healthComponent) select v).FirstOrDefault();
                    if (hurtBox) {
                        bouncedObjects.Add(hurtBox.healthComponent);
                        lightningOrb.target = hurtBox;
                        OrbManager.instance.AddOrb(lightningOrb);
                    }
                }
            }
        }
    }
}