using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BtpTweak {

    public static class AssetReferences {
        public static readonly GameObject affixWhiteDelayEffect = GameObjectPaths.AffixWhiteDelayEffect.Load<GameObject>();
        public static readonly GameObject affixWhiteExplosion = GameObjectPaths.AffixWhiteExplosion.Load<GameObject>();
        public static readonly GameObject bonusMoneyPack = GameObjectPaths.BonusMoneyPack.Load<GameObject>();
        public static readonly GameObject brotherBodyPrefab = GameObjectPaths.BrotherBody.Load<GameObject>();
        public static readonly GameObject brotherUltLineProjectileStatic = GameObjectPaths.BrotherUltLineProjectileStatic.Load<GameObject>();
        public static readonly GameObject critspark = GameObjectPaths.Critspark.Load<GameObject>();
        public static readonly GameObject critsparkHeavy = GameObjectPaths.CritsparkHeavy.Load<GameObject>();
        public static readonly GameObject electricOrbProjectile = GameObjectPaths.ElectricOrbProjectile.Load<GameObject>();
        public static readonly GameObject elementalRingVoidBlackHole = GameObjectPaths.ElementalRingVoidBlackHole.Load<GameObject>();
        public static readonly GameObject fireMeatBallProjectile = GameObjectPaths.FireMeatBall.Load<GameObject>();
        public static readonly GameObject fireTornado = GameObjectPaths.FireTornado.Load<GameObject>();
        public static readonly GameObject fireworkLauncher = GameObjectPaths.FireworkLauncher.Load<GameObject>();
        public static readonly GameObject fireworkPrefab = GameObjectPaths.FireworkProjectile.Load<GameObject>();
        public static readonly GameObject fractureImpactEffect = GameObjectPaths.FractureImpactEffect.Load<GameObject>();
        public static readonly GameObject genericDelayBlast = GameObjectPaths.GenericDelayBlast.Load<GameObject>();
        public static readonly GameObject helperPrefab = GameObjectPaths.DirectorSpawnProbeHelperPrefab.Load<GameObject>();
        public static readonly GameObject iceRingExplosion = GameObjectPaths.IceRingExplosion.Load<GameObject>();
        public static readonly GameObject lightningStake = GameObjectPaths.LightningStake.Load<GameObject>();
        public static readonly GameObject lunarMissilePrefab = GameObjectPaths.LunarMissileProjectile.Load<GameObject>();
        public static readonly GameObject magmaOrbProjectile = GameObjectPaths.MagmaOrbProjectile.Load<GameObject>();
        public static readonly GameObject molotovClusterProjectile = GameObjectPaths.MolotovClusterProjectile.Load<GameObject>();
        public static readonly GameObject molotovProjectileDotZone = GameObjectPaths.MolotovProjectileDotZone.Load<GameObject>();
        public static readonly GameObject molotovSingleProjectile = GameObjectPaths.MolotovSingleProjectile.Load<GameObject>();
        public static readonly GameObject moonExitArenaOrbEffect = GameObjectPaths.MoonExitArenaOrbEffect.Load<GameObject>();
        public static readonly GameObject omniExplosionVFXQuick = GameObjectPaths.OmniExplosionVFXQuick.Load<GameObject>();
        public static readonly GameObject omniRecycleEffect = GameObjectPaths.OmniRecycleEffect.Load<GameObject>();
        public static readonly GameObject shrineUseEffect = GameObjectPaths.ShrineUseEffect.Load<GameObject>();
        public static readonly GameObject stickyBombProjectile = GameObjectPaths.StickyBomb1.Load<GameObject>();
        public static readonly GameObject teamWarCryActivation = GameObjectPaths.TeamWarCryActivation.Load<GameObject>();
        public static readonly GameObject coinImpact = GameObjectPaths.CoinImpact.Load<GameObject>();
        public static readonly GameObject titanGoldPreFistProjectile = GameObjectPaths.TitanGoldPreFistProjectile.Load<GameObject>();
        public static readonly GameObject randomEquipmentTriggerProcEffect = GameObjectPaths.RandomEquipmentTriggerProcEffect.Load<GameObject>();
        public static readonly Material helscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarWisp/matLunarWispFlames.mat").WaitForCompletion();
        public static readonly Material moonscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/moon2/matSkyboxMoon.mat").WaitForCompletion();
        public static readonly Material stormscourgeMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Mage/matMageCalibrateLightning.mat").WaitForCompletion();
    }
}