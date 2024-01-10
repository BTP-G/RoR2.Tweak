using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class CaptainTweak : TweakBase<CaptainTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float CallAirstrikeAltDamageCoefficient = 500f;

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.Achievements.Captain.CaptainSupplyDropFinaleAchievement.CaptainSupplyDropFinaleServerAchievement.DoesDamageQualify += CaptainSupplyDropFinaleServerAchievement_DoesDamageQualify;
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile += CallAirstrikeBase_ModifyProjectile;
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += FireCaptainShotgun_ModifyBullet;
            On.EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState.OnEnter += BaseCaptainSupplyDropState_OnEnter;
            On.EntityStates.CaptainSupplyDrop.HealZoneMainState.OnEnter += HealZoneMainState_OnEnter;
            EntityStateConfigurationPaths.EntityStatesCaptainWeaponCallAirstrikeAlt.Load<EntityStateConfiguration>().Set("damageCoefficient", CallAirstrikeAltDamageCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObject captainTazer = GameObjectPaths.CaptainTazer31.Load<GameObject>();
            captainTazer.AddComponent<ChainLightning>();
            captainTazer.GetComponent<ProjectileSimple>().lifetime = 6f;
            ProjectileImpactExplosion impactExplosion = captainTazer.GetComponent<ProjectileImpactExplosion>();
            impactExplosion.lifetimeAfterImpact = 6f;
            impactExplosion.lifetime = 6f;
            SkillDef prepAirstrikeAlt = CaptainOrbitalSkillDefPaths.PrepAirstrikeAlt.Load<SkillDef>();
            prepAirstrikeAlt.baseRechargeInterval = 10;
            prepAirstrikeAlt.baseMaxStock = 4;
            prepAirstrikeAlt.requiredStock = 4;
            prepAirstrikeAlt.stockToConsume = 4;
            EntityStates.CaptainSupplyDrop.ShockZoneMainState.shockRadius = 20f;
            SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("CaptainSupplyDropDepleted")).baseMaxStock = 1;
            RoR2Content.Survivors.Captain.bodyPrefab.GetComponent<CharacterBody>().baseMoveSpeed = 8;
            foreach (var sceneDef in SceneCatalog.allSceneDefs) {
                sceneDef.blockOrbitalSkills = false;
            }
        }

        private bool CaptainSupplyDropFinaleServerAchievement_DoesDamageQualify(On.RoR2.Achievements.Captain.CaptainSupplyDropFinaleAchievement.CaptainSupplyDropFinaleServerAchievement.orig_DoesDamageQualify orig, RoR2.Achievements.BaseServerAchievement self, DamageReport damageReport) {
            return damageReport.damageInfo.inflictor
                && damageReport.damageInfo.inflictor.TryGetComponent<GenericDisplayNameProvider>(out var component)
                && component.displayToken != null
                && component.displayToken.StartsWith("CAPTAIN_SUPPLY_");
        }

        private void BaseCaptainSupplyDropState_OnEnter(On.EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState.orig_OnEnter orig, EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState self) {
            orig(self);
            if (self is EntityStates.CaptainSupplyDrop.EquipmentRestockMainState) {
                self.energyComponent.chargeRate = 0.5f * TeamManager.instance.GetTeamLevel(self.teamFilter.teamIndex);
            }
        }

        private void CallAirstrikeBase_ModifyProjectile(On.EntityStates.Captain.Weapon.CallAirstrikeBase.orig_ModifyProjectile orig, EntityStates.Captain.Weapon.CallAirstrikeBase self, ref FireProjectileInfo fireProjectileInfo) {
            orig(self, ref fireProjectileInfo);
            int itemCount = self.characterBody.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine.itemIndex);
            if (itemCount > 0) {
                fireProjectileInfo.useFuseOverride = true;
                fireProjectileInfo.fuseOverride = fireProjectileInfo.projectilePrefab.GetComponent<ProjectileImpactExplosion>().lifetime * Mathf.Pow(0.666f, itemCount);
            }
        }

        private void FireCaptainShotgun_ModifyBullet(On.EntityStates.Captain.Weapon.FireCaptainShotgun.orig_ModifyBullet orig, EntityStates.Captain.Weapon.FireCaptainShotgun self, BulletAttack bulletAttack) {
            orig(self, bulletAttack);
            bulletAttack.force *= 1 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.Behemoth.itemIndex);
        }

        private void HealZoneMainState_OnEnter(On.EntityStates.CaptainSupplyDrop.HealZoneMainState.orig_OnEnter orig, EntityStates.CaptainSupplyDrop.HealZoneMainState self) {
            orig(self);
            if (NetworkServer.active) {
                var healingWard = self.healZoneInstance.GetComponent<HealingWard>();
                float upLevel = TeamManager.instance.GetTeamLevel(self.teamFilter.teamIndex) - 1;
                healingWard.Networkradius += upLevel;
                healingWard.healFraction += 0.01f * upLevel * healingWard.interval;
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileDamage))]
        private class ChainLightning : MonoBehaviour, IProjectileImpactBehavior {
            private ProjectileController projectileController;
            private ProjectileDamage projectileDamage;

            public void OnProjectileImpact(ProjectileImpactInfo impactInfo) {
                var lightningOrb = new LightningOrb() {
                    attacker = projectileController.owner,
                    bouncedObjects = [],
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
                    targetsToFindPerBounce = 2,
                    teamIndex = projectileController.teamFilter.teamIndex
                };
                if (lightningOrb.target = lightningOrb.PickNextTarget(impactInfo.estimatedPointOfImpact)) {
                    lightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
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
    }
}