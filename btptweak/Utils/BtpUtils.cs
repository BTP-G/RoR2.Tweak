using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Utils {

    internal static class BtpUtils {

        public static void SpawnVoidDeathBomb(in Vector3 position) => ProjectileManager.instance.FireProjectile(new FireProjectileInfo() {
            projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
            position = position,
            rotation = Quaternion.identity,
        });

        public static float 简单逼近(float 基数, float 半数, float 目标) => 目标 * 基数 / (基数 + 半数);

    }
}