using BtpTweak.Utils;
using RoR2;
using UnityEngine;

namespace BtpTweak {

    public static class AssetReferences {
        public static readonly GameObject affixWhiteDelayEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/AffixWhiteDelayEffect");
        public static readonly GameObject affixWhiteExplosion = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion");
        public static readonly GameObject bonusMoneyPack = "RoR2/Base/BonusGoldPackOnKill/BonusMoneyPack.prefab".Load<GameObject>();
        public static readonly GameObject electricOrbProjectile = "RoR2/Base/ElectricWorm/ElectricOrbProjectile.prefab".Load<GameObject>();
        public static readonly GameObject fireMeatBallProjectile = "RoR2/Base/FireballsOnHit/FireMeatBall.prefab".Load<GameObject>();
        public static readonly GameObject fireworkPrefab = "RoR2/Base/Firework/FireworkProjectile.prefab".Load<GameObject>();
        public static readonly GameObject genericDelayBlast = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast");
        public static readonly GameObject helperPrefab = LegacyResourcesAPI.Load<GameObject>("SpawnCards/HelperPrefab");
        public static readonly GameObject lunarBlink = "RoR2/Base/moon/MoonExitArenaOrbEffect.prefab".Load<GameObject>();
        public static readonly GameObject magmaOrbProjectile = "RoR2/Base/MagmaWorm/MagmaOrbProjectile.prefab".Load<GameObject>();
        public static readonly GameObject omniRecycleEffect = "RoR2/Base/Recycle/OmniRecycleEffect.prefab".Load<GameObject>();
        public static readonly GameObject stickyBombProjectile = "RoR2/Base/StickyBomb/StickyBomb.prefab".Load<GameObject>();
        public static readonly GameObject molotovSingleProjectile = "RoR2/DLC1/Molotov/MolotovSingleProjectile.prefab".Load<GameObject>();
        public static readonly GameObject molotovProjectileDotZone = "RoR2/DLC1/Molotov/MolotovProjectileDotZone.prefab".Load<GameObject>();
        public static readonly GameObject fireTornado = "RoR2/Base/ElementalRings/FireTornado.prefab".Load<GameObject>();
        public static readonly GameObject elementalRingVoidBlackHole = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ElementalRingVoidBlackHole");
    }
}