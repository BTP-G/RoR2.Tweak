using RoR2.Projectile;

namespace BtpTweak.ProjectileFountains {

    public class StickyBombFountain : ProjectileFountain {

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            info.useFuseOverride = false;
            info.speedOverride *= 0.33f;
        }
    }
}