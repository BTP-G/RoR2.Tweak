using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
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
            var daggerProjectile = GameObjectPaths.DaggerProjectile.Load<GameObject>();
            daggerProjectile.GetComponent<ProjectileController>().procCoefficient = 0.33f;
            daggerProjectile.GetComponent<ProjectileSimple>().lifetime = 10f;
            AssetReferences.molotovProjectileDotZone.AddComponent<MolotovDotZoneStartAction>();
            GameObjectPaths.DeathProjectile18.LoadComponent<ProjectileController>().procCoefficient = 0;
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