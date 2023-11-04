using BtpTweak.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ProjectileTweak : TweakBase<ProjectileTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
        }

        public void Load() {
            var daggerProjectile = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/DaggerProjectile");
            daggerProjectile.GetComponent<ProjectileController>().procCoefficient = 0.33f;
            daggerProjectile.GetComponent<ProjectileSimple>().lifetime = 10f;
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<ProjectileController>().procCoefficient = 0.5f;
            AssetReferences.fireworkPrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.fireworkPrefab.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.2f;
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.2f;
            AssetReferences.stickyBombProjectile.RemoveComponent<LoopSound>();
            AssetReferences.fireMeatBallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.fireMeatBallProjectile.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.5f;
            var missileController = GlobalEventManager.CommonAssets.missilePrefab.GetComponent<MissileController>();
            //missileController.maxVelocity *= 2f;
            //missileController.acceleration *= 10f;
            missileController.maxSeekDistance = float.MaxValue;
            //missileController.delayTimer = 0.2f;
            //missileController.turbulence = 0;
            var missileController2 = AssetReferences.fireworkPrefab.GetComponent<MissileController>();
            missileController2.maxVelocity = missileController.maxVelocity;
            missileController2.acceleration = missileController.acceleration;
            missileController2.delayTimer = missileController.delayTimer;
            missileController2.maxSeekDistance = missileController.maxSeekDistance;
            missileController2.turbulence = missileController.turbulence;
            AssetReferences.fireTornado.GetComponent<ProjectileOverlapAttack>().overlapProcCoefficient = 0.1f;
            AssetReferences.fireTornado.GetComponent<ProjectileSimple>().lifetime = 3f;
            AssetReferences.molotovProjectileDotZone.AddComponent<MolotovDotZoneStartAction>();
            "RoR2/Base/DeathProjectile/DeathProjectile.prefab".LoadComponent<ProjectileController>().procCoefficient = 0;
            var delayBlast0 = GlobalEventManager.CommonAssets.explodeOnDeathPrefab.GetComponent<DelayBlast>();
            delayBlast0.baseForce = 1000f;
            delayBlast0.bonusForce = Vector3.up * 1000f;
            delayBlast0.damageColorIndex = DamageColorIndex.Item;
            delayBlast0.damageType = DamageType.AOE;
            delayBlast0.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            delayBlast0.inflictor = null;
            delayBlast0.maxTimer = 0.5f;
            var delayBlast1 = HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab.GetComponent<DelayBlast>();
            delayBlast1.baseForce = 666f;
            delayBlast1.damageColorIndex = DamageColorIndex.Void;
            delayBlast1.damageType = DamageType.AOE;
            delayBlast1.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            delayBlast1.inflictor = null;
            delayBlast1.maxTimer = 0f;
            var delayBlast2 = GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect.GetComponent<DelayBlast>();
            delayBlast2.baseForce = 0f;
            delayBlast2.bonusForce = Vector3.zero;
            delayBlast2.damageColorIndex = DamageColorIndex.Item;
            delayBlast2.damageType = DamageType.AOE | DamageType.BleedOnHit;
            delayBlast2.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            delayBlast2.inflictor = null;
            delayBlast2.maxTimer = 0.1f;
        }

        private class MolotovDotZoneStartAction : MonoBehaviour {
            private float radiusScale = 1;
            private float timer = 0;

            private void Start() {
                int itemCount = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.IgniteOnKill) ?? 0;
                if (itemCount > 0) {
                    radiusScale += 0.5f * itemCount;
                    GetComponent<ProjectileDotZone>().lifetime *= radiusScale;
                }
            }

            private void Update() {
                timer += Time.deltaTime;
                float currentRadius = 0.5f + radiusScale * timer / (timer + radiusScale);
                transform.localScale = new Vector3(currentRadius, currentRadius, currentRadius);
            }
        }
    }
}