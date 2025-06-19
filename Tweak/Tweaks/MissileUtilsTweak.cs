using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class MissileUtilsTweak : TweakBase<MissileUtilsTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.MissileUtils.FireMissile_Vector3_CharacterBody_ProcChainMask_GameObject_float_bool_GameObject_DamageColorIndex_Vector3_float_bool += MissileUtils_FireMissile;
        }

        private void MissileUtils_FireMissile(On.RoR2.MissileUtils.orig_FireMissile_Vector3_CharacterBody_ProcChainMask_GameObject_float_bool_GameObject_DamageColorIndex_Vector3_float_bool orig, Vector3 position, CharacterBody attackerBody, ProcChainMask procChainMask, GameObject victim, float missileDamage, bool isCrit, GameObject projectilePrefab, DamageColorIndex damageColorIndex, Vector3 initialDirection, float force, bool addMissileProc) {
            if (addMissileProc) {
                procChainMask.AddProc(ProcType.Missile);
            }
            var fireProjectileInfo = new FireProjectileInfo {
                crit = isCrit,
                damage = missileDamage,
                damageColorIndex = damageColorIndex,
                force = force,
                owner = attackerBody.gameObject,
                position = position,
                procChainMask = procChainMask,
                projectilePrefab = projectilePrefab,
                target = victim,
            };
            var itemCount = attackerBody.inventory ? attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile.itemIndex) : 0;
            if (itemCount > 0) {
                fireProjectileInfo.damage *= (itemCount + 1) * 0.5f;
                var axis = attackerBody.inputBank ? attackerBody.inputBank.aimDirection : attackerBody.corePosition;
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(45f, axis) * initialDirection);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(-45f, axis) * initialDirection);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(initialDirection);
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }
    }
}