using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ProjectileTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.Projectile.MissileController.FixedUpdate += MissileController_FixedUpdate;
        }

        public override void Load() {
            base.Load();
            var gameObject = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/DaggerProjectile");
            gameObject.GetComponent<ProjectileController>().procCoefficient = 0.33f;
            gameObject.GetComponent<ProjectileSimple>().lifetime = 10f;
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<ProjectileController>().procCoefficient = 0.5f;
            AssetReferences.fireMeatBallProjectile.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.5f;
            AssetReferences.stickyBombProjectile.RemoveComponent<LoopSound>();
            var missileController = GlobalEventManager.CommonAssets.missilePrefab.GetComponent<MissileController>();
            missileController.maxVelocity *= 2f;
            missileController.acceleration *= 10f;
            missileController.maxSeekDistance = float.MaxValue;
            missileController.delayTimer = 0.2f;
            missileController.turbulence = 0;
            var missileController2 = AssetReferences.fireworkPrefab.GetComponent<MissileController>();
            missileController2.maxVelocity = missileController.maxVelocity;
            missileController2.acceleration = missileController.acceleration;
            missileController2.delayTimer = missileController.delayTimer;
            missileController2.maxSeekDistance = missileController.maxSeekDistance;
            missileController2.turbulence = missileController.turbulence;
        }

        private void MissileController_FixedUpdate(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 0))) {
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.EmitDelegate((Vector3 target, MissileController missileController) => {
                    missileController.transform.rotation = Util.QuaternionSafeLookRotation(target);
                    return Vector3.zero;
                });
            } else {
                Main.Logger.LogError("MissileController Hook Failed!");
            }
        }
    }
}