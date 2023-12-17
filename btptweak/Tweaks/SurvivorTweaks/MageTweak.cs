using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class MageTweak : TweakBase<MageTweak>, IOnRoR2LoadedBehavior {
        public const float 电击半径 = 25;
        public const float 电击伤害系数 = 0.6f;
        public const int 每次最大电击数 = 10;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            SteppedSkillDef mageFireFirebolt = SteppedSkillDefPaths.MageBodyFireFirebolt.Load<SteppedSkillDef>();
            mageFireFirebolt.baseRechargeInterval = 0;
            mageFireFirebolt.baseMaxStock = 1;
            SteppedSkillDef mageFireLightningBolt = SteppedSkillDefPaths.MageBodyFireLightningBolt.Load<SteppedSkillDef>();
            mageFireLightningBolt.baseRechargeInterval = 0;
            mageFireLightningBolt.baseMaxStock = 1;
            GameObjectPaths.MageIceBombProjectile.Load<GameObject>().AddComponent<IceExplosion>();
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
            SkillDefPaths.MageBodyIceBomb.Load<SkillDef>().mustKeyPress = false;
            SkillDefPaths.MageBodyNovaBomb.Load<SkillDef>().mustKeyPress = false;
        }

        [RequireComponent(typeof(ProjectileController))]
        private class IceExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                ProjectileController projectileController = gameObject.GetComponent<ProjectileController>();
                ProjectileDamage projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                GameObject iceExplosion = Instantiate(AssetReferences.genericDelayBlast, transform.position, Quaternion.identity);
                iceExplosion.transform.localScale = new Vector3(12, 12, 12);
                DelayBlast delayBlast = iceExplosion.GetComponent<DelayBlast>();
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
                iceExplosion.GetComponent<TeamFilter>().teamIndex = projectileController.teamFilter.teamIndex;
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileProximityBeamController))]
        private class MageLightningBombStartAction : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void Start() {
                var inventory = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>().inventory;
                if (inventory) {
                    GetComponent<ProjectileProximityBeamController>().attackRange += 3 * inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex);
                }
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileDamage))]
        private class LightningExplosion : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void OnDestroy() {
                ProjectileController projectileController = gameObject.GetComponent<ProjectileController>();
                ProjectileDamage projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                int itemCount = projectileController.owner?.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ChainLightning.itemIndex) ?? 0;
                List<HealthComponent> bouncedObjects = [];
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
                    HurtBox hurtBox = (from v in search.GetResults() where !bouncedObjects.Contains(v.healthComponent) select v).FirstOrDefault();
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