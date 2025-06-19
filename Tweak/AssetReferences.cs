using BTP.RoR2Plugin.Utils;
using R2API.AddressReferencedAssets;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BTP.RoR2Plugin {

    public static class AssetReferences {
        public static readonly AddressReferencedPrefab affixWhiteDelayEffect = GameObjectPaths.AffixWhiteDelayEffect;

        public static readonly AddressReferencedPrefab affixWhiteExplosion = GameObjectPaths.AffixWhiteExplosion;

        public static readonly AddressReferencedPrefab bonusMoneyPack = GameObjectPaths.BonusMoneyPack;

        public static readonly AddressReferencedPrefab brotherBodyPrefab = GameObjectPaths.BrotherBody;

        public static readonly AddressReferencedPrefab brotherUltLineProjectileStatic = GameObjectPaths.BrotherUltLineProjectileStatic;

        public static readonly AddressReferencedPrefab critspark = GameObjectPaths.Critspark;

        public static readonly AddressReferencedPrefab critsparkHeavy = GameObjectPaths.CritsparkHeavy;

        public static readonly AddressReferencedPrefab electricOrbProjectile = GameObjectPaths.ElectricOrbProjectile;

        public static readonly AddressReferencedPrefab elementalRingVoidBlackHole = GameObjectPaths.ElementalRingVoidBlackHole;

        public static readonly AddressReferencedPrefab fireMeatBallProjectile = GameObjectPaths.FireMeatBall;

        public static readonly AddressReferencedPrefab fireTornado = GameObjectPaths.FireTornado;

        public static readonly AddressReferencedPrefab fireworkLauncher = GameObjectPaths.FireworkLauncher;

        public static readonly AddressReferencedPrefab fireworkPrefab = GameObjectPaths.FireworkProjectile;

        public static readonly AddressReferencedPrefab fractureImpactEffect = GameObjectPaths.FractureImpactEffect;

        public static readonly AddressReferencedPrefab genericDelayBlast = GameObjectPaths.GenericDelayBlast;

        public static readonly AddressReferencedPrefab helperPrefab = GameObjectPaths.DirectorSpawnProbeHelperPrefab;

        public static readonly AddressReferencedPrefab iceRingExplosion = GameObjectPaths.IceRingExplosion;

        public static readonly AddressReferencedPrefab lightningStake = GameObjectPaths.LightningStake;

        public static readonly AddressReferencedPrefab lunarMissilePrefab = GameObjectPaths.LunarMissileProjectile;

        public static readonly AddressReferencedPrefab magmaOrbProjectile = GameObjectPaths.MagmaOrbProjectile;

        public static readonly AddressReferencedPrefab molotovClusterProjectile = GameObjectPaths.MolotovClusterProjectile;

        public static readonly AddressReferencedPrefab molotovProjectileDotZone = GameObjectPaths.MolotovProjectileDotZone;

        public static readonly AddressReferencedPrefab molotovSingleProjectile = GameObjectPaths.MolotovSingleProjectile;

        public static readonly AddressReferencedPrefab moonExitArenaOrbEffect = GameObjectPaths.MoonExitArenaOrbEffect;

        public static readonly AddressReferencedPrefab omniExplosionVFXQuick = GameObjectPaths.OmniExplosionVFXQuick;

        public static readonly AddressReferencedPrefab omniRecycleEffect = GameObjectPaths.OmniRecycleEffect;

        public static readonly AddressReferencedPrefab shrineUseEffect = GameObjectPaths.ShrineUseEffect;

        public static readonly AddressReferencedPrefab stickyBombProjectile = GameObjectPaths.StickyBomb1;

        public static readonly AddressReferencedPrefab teamWarCryActivation = GameObjectPaths.TeamWarCryActivation;

        public static readonly AddressReferencedPrefab coinImpact = GameObjectPaths.CoinImpact;

        public static readonly AddressReferencedPrefab titanGoldPreFistProjectile = GameObjectPaths.TitanGoldPreFistProjectile;

        public static readonly AddressReferencedPrefab randomEquipmentTriggerProcEffect = GameObjectPaths.RandomEquipmentTriggerProcEffect;

        public static readonly Material helscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarWisp/matLunarWispFlames.mat").WaitForCompletion();

        public static readonly Material moonscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/moon2/matSkyboxMoon.mat").WaitForCompletion();

        public static readonly Material stormscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Mage/matMageCalibrateLightning.mat").WaitForCompletion();

        public static readonly GameObject stunAndPierceBoomerang = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StunAndPierceBoomerang");

        public static void Init() {
        }
    }
}