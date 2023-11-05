using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class CaptainTweak : TweakBase<CaptainTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.Captain.Weapon.CallAirstrikeAlt.OnExit += CallAirstrikeAlt_OnExit;
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile += CallAirstrikeBase_ModifyProjectile;
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet += FireCaptainShotgun_ModifyBullet;
            On.EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState.OnEnter += BaseCaptainSupplyDropState_OnEnter;
            On.EntityStates.CaptainSupplyDrop.HealZoneMainState.OnEnter += HealZoneMainState_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.Captain.Weapon.CallAirstrikeAlt.OnExit -= CallAirstrikeAlt_OnExit;
            On.EntityStates.Captain.Weapon.CallAirstrikeBase.ModifyProjectile -= CallAirstrikeBase_ModifyProjectile;
            On.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet -= FireCaptainShotgun_ModifyBullet;
            On.EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState.OnEnter -= BaseCaptainSupplyDropState_OnEnter;
            On.EntityStates.CaptainSupplyDrop.HealZoneMainState.OnEnter -= HealZoneMainState_OnEnter;
        }

        public void Load() {
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
            CaptainSupplyDropSkillDefPaths.PrepSupplyDrop.Load<SkillDef>().baseRechargeInterval = 0.5f;
            EntityStates.CaptainSupplyDrop.ShockZoneMainState.shockRadius = 20;
            RoR2Content.Survivors.Captain.bodyPrefab.GetComponent<CharacterBody>().baseMoveSpeed = 8;
            foreach (var sceneDef in SceneCatalog.allSceneDefs) {
                sceneDef.blockOrbitalSkills = false;
            }
        }

        private void BaseCaptainSupplyDropState_OnEnter(On.EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState.orig_OnEnter orig, EntityStates.CaptainSupplyDrop.BaseCaptainSupplyDropState self) {
            orig(self);
            if (self is EntityStates.CaptainSupplyDrop.EquipmentRestockMainState) {
                self.energyComponent.chargeRate = 0.5f * TeamManager.instance.GetTeamLevel(self.teamFilter.teamIndex);
            }
        }

        private void CallAirstrikeAlt_OnExit(On.EntityStates.Captain.Weapon.CallAirstrikeAlt.orig_OnExit orig, EntityStates.Captain.Weapon.CallAirstrikeAlt self) {
            self.damageCoefficient = 500f;
            orig(self);
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
            HealingWard healingWard = self.healZoneInstance?.GetComponent<HealingWard>();
            if (healingWard != null) {
                uint level = TeamManager.instance.GetTeamLevel(self.teamFilter.teamIndex);
                healingWard.Networkradius += level;
                healingWard.healFraction *= 0.1f * level;
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileDamage))]
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
    }
}