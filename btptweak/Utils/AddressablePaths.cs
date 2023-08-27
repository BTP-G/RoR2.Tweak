namespace BtpTweak.Utils {

    public static class AddressableUtils {

        public static T Load<T>(this string path) {
            return UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        }

        public static T LoadComponent<T>(this string path) where T : UnityEngine.Component {
            return path.Load<UnityEngine.GameObject>().GetComponent<T>();
        }

        public static T[] LoadComponents<T>(this string path) where T : UnityEngine.Component {
            return path.Load<UnityEngine.GameObject>().GetComponents<T>();
        }

        public static T LoadComponentInChildren<T>(this string path) where T : UnityEngine.Component {
            return path.Load<UnityEngine.GameObject>().GetComponentInChildren<T>();
        }
    }
}

namespace BtpTweak.Utils.Paths {

    public class RoR2SkillDefs {
        public string AimArrowSnipe = "RoR2/Base/Huntress/AimArrowSnipe.asset";
        public string AncientWispBodyEnrage = "RoR2/Junk/AncientWisp/AncientWispBodyEnrage.asset";
        public string AncientWispBodyRain = "RoR2/Junk/AncientWisp/AncientWispBodyRain.asset";
        public string AncientWispBodyRHCanon = "RoR2/Junk/AncientWisp/AncientWispBodyRHCanon.asset";
        public string ArchWispBodyCannons = "RoR2/Junk/ArchWisp/ArchWispBodyCannons.asset";
        public string Assassin2Hide = "RoR2/DLC1/Assassin2/Assassin2Hide.asset";
        public string Assassin2Shuriken = "RoR2/DLC1/Assassin2/Assassin2Shuriken.asset";
        public string Assassin2Strike = "RoR2/DLC1/Assassin2/Assassin2Strike.asset";
        public string AssassinBodyFireGrenade = "RoR2/Junk/Assassin/AssassinBodyFireGrenade.asset";
        public string AssassinBodyPlaceMine = "RoR2/Junk/Assassin/AssassinBodyPlaceMine.asset";
        public string AssassinBodyUtility = "RoR2/Junk/Assassin/AssassinBodyUtility.asset";
        public string BackupDroneBodyGun = "RoR2/Base/Drones/BackupDroneBodyGun.asset";
        public string BackupDroneOldBodyGun = "RoR2/Junk/BackupDroneOld/BackupDroneOldBodyGun.asset";
        public string Bandit2SerratedShivs = "RoR2/Base/Bandit2/Bandit2SerratedShivs.asset";
        public string BanditBodyCloak = "RoR2/Junk/Bandit/BanditBodyCloak.asset";
        public string BanditBodyFireShotgun = "RoR2/Junk/Bandit/BanditBodyFireShotgun.asset";
        public string BanditBodyGrenade = "RoR2/Junk/Bandit/BanditBodyGrenade.asset";
        public string BanditBodyLightsOut = "RoR2/Junk/Bandit/BanditBodyLightsOut.asset";
        public string BeetleBodyHeadbutt = "RoR2/Base/Beetle/BeetleBodyHeadbutt.asset";
        public string BeetleBodySleep = "RoR2/Base/Beetle/BeetleBodySleep.asset";
        public string BeetleGuardBodyDefenseUp = "RoR2/Base/Beetle/BeetleGuardBodyDefenseUp.asset";
        public string BeetleGuardBodyGroundSlam = "RoR2/Base/Beetle/BeetleGuardBodyGroundSlam.asset";
        public string BeetleGuardBodySunder = "RoR2/Base/Beetle/BeetleGuardBodySunder.asset";
        public string BeetleQueen2BodySpawnWards = "RoR2/Base/Beetle/BeetleQueen2BodySpawnWards.asset";
        public string BeetleQueen2BodySpit = "RoR2/Base/Beetle/BeetleQueen2BodySpit.asset";
        public string BeetleQueen2BodySummonEggs = "RoR2/Base/Beetle/BeetleQueen2BodySummonEggs.asset";
        public string BeginOvercharge = "RoR2/Junk/Loader/BeginOvercharge.asset";
        public string BellBodyBellBlast = "RoR2/Base/Bell/BellBodyBellBlast.asset";
        public string BisonBodyCharge = "RoR2/Base/Bison/BisonBodyCharge.asset";
        public string BisonBodyHeadbutt = "RoR2/Base/Bison/BisonBodyHeadbutt.asset";
        public string BomberBodyFireBarrage = "RoR2/Junk/Bomber/BomberBodyFireBarrage.asset";
        public string BomberBodyFireRocket = "RoR2/Junk/Bomber/BomberBodyFireRocket.asset";
        public string BomberBodyPaintMicroMissiles = "RoR2/Junk/Bomber/BomberBodyPaintMicroMissiles.asset";
        public string BomberBodyRoll = "RoR2/Junk/Bomber/BomberBodyRoll.asset";
        public string CallAirstrike = "RoR2/Base/Captain/CallAirstrike.asset";
        public string CallAirstrikeAlt = "RoR2/Base/Captain/CallAirstrikeAlt.asset";
        public string CallSupplyDropDefense = "RoR2/Junk/Captain/CallSupplyDropDefense.asset";
        public string CallSupplyDropEquipmentRestock = "RoR2/Base/Captain/CallSupplyDropEquipmentRestock.asset";
        public string CallSupplyDropHacking = "RoR2/Base/Captain/CallSupplyDropHacking.asset";
        public string CallSupplyDropHealing = "RoR2/Base/Captain/CallSupplyDropHealing.asset";
        public string CallSupplyDropShocking = "RoR2/Base/Captain/CallSupplyDropShocking.asset";
        public string CaptainCancelDummy = "RoR2/Base/Captain/CaptainCancelDummy.asset";
        public string CaptainShotgun = "RoR2/Base/Captain/CaptainShotgun.asset";
        public string CaptainSkillDisconnected = "RoR2/Base/Captain/CaptainSkillDisconnected.asset";
        public string CaptainSkillUsedUp = "RoR2/Base/Captain/CaptainSkillUsedUp.asset";
        public string CaptainTazer = "RoR2/Base/Captain/CaptainTazer.asset";
        public string ChargeGoldLaser = "RoR2/Base/Titan/ChargeGoldLaser.asset";
        public string ChargeMegaBlaster = "RoR2/DLC1/VoidSurvivor/ChargeMegaBlaster.asset";
        public string ChargeWindblade = "RoR2/Base/Vulture/ChargeWindblade.asset";
        public string ClayBodyLeap = "RoR2/Junk/ClayMan/ClayBodyLeap.asset";
        public string ClayBodySwipeForward = "RoR2/Junk/ClayMan/ClayBodySwipeForward.asset";
        public string ClayBossBodyChargeBombardment = "RoR2/Base/ClayBoss/ClayBossBodyCharge Bombardment.asset";
        public string ClayBossBodyRecover = "RoR2/Base/ClayBoss/ClayBossBodyRecover.asset";
        public string ClayBossBodyTarball = "RoR2/Base/ClayBoss/ClayBossBodyTarball.asset";
        public string ClayBruiserBodyGun = "RoR2/Base/ClayBruiser/ClayBruiserBodyGun.asset";
        public string ClayBruiserBodyKnockback = "RoR2/Base/ClayBruiser/ClayBruiserBodyKnockback.asset";
        public string CommandoBodyBarrage = "RoR2/Base/Commando/CommandoBodyBarrage.asset";
        public string CommandoBodyFireFMJ = "RoR2/Base/Commando/CommandoBodyFireFMJ.asset";
        public string CommandoBodyFireShotgunBlast = "RoR2/Base/Commando/CommandoBodyFireShotgunBlast.asset";
        public string CommandoBodyRoll = "RoR2/Base/Commando/CommandoBodyRoll.asset";
        public string CommandoBodySweepBarrage = "RoR2/Junk/Commando/CommandoBodySweepBarrage.asset";
        public string CommandoPerformanceTestBodyBarrage = "RoR2/Junk/CommandoPerformanceTest/CommandoPerformanceTestBodyBarrage.asset";
        public string CommandoPerformanceTestBodyFireFMJ = "RoR2/Junk/CommandoPerformanceTest/CommandoPerformanceTestBodyFireFMJ.asset";
        public string CommandoPerformanceTestBodyFirePistol = "RoR2/Junk/CommandoPerformanceTest/CommandoPerformanceTestBodyFirePistol.asset";
        public string CommandoPerformanceTestBodyRoll = "RoR2/Junk/CommandoPerformanceTest/CommandoPerformanceTestBodyRoll.asset";
        public string CommandoSlide = "RoR2/Base/Commando/CommandoSlide.asset";
        public string CrocoBite = "RoR2/Base/Croco/CrocoBite.asset";
        public string CrocoChainableLeap = "RoR2/Base/Croco/CrocoChainableLeap.asset";
        public string CrocoDisease = "RoR2/Base/Croco/CrocoDisease.asset";
        public string CrocoLeap = "RoR2/Base/Croco/CrocoLeap.asset";
        public string CrocoPassiveBlight = "RoR2/Base/Croco/CrocoPassiveBlight.asset";
        public string CrocoPassivePoison = "RoR2/Base/Croco/CrocoPassivePoison.asset";
        public string CrocoSpit = "RoR2/Base/Croco/CrocoSpit.asset";
        public string CrushHealth = "RoR2/DLC1/VoidSurvivor/CrushHealth.asset";
        public string DeployMinion = "RoR2/Base/RoboBallBoss/DeployMinion.asset";
        public string Drone1BodyGun = "RoR2/Base/Drones/Drone1BodyGun.asset";
        public string Drone2BodyHealingBeam = "RoR2/Base/Drones/Drone2BodyHealingBeam.asset";
        public string ElectricWormBodyBlink = "RoR2/Base/ElectricWorm/ElectricWormBodyBlink.asset";
        public string ElectricWormBodyStanceSwitch = "RoR2/Base/ElectricWorm/ElectricWormBodyStanceSwitch.asset";
        public string EnableEyebeam = "RoR2/Junk/RoboBallBoss/EnableEyebeam.asset";
        public string EnforcerBodyFireBarrage = "RoR2/Junk/Enforcer/EnforcerBodyFireBarrage.asset";
        public string EnforcerBodyFireFMJ = "RoR2/Junk/Enforcer/EnforcerBodyFireFMJ.asset";
        public string EnforcerBodyFirePistol = "RoR2/Junk/Enforcer/EnforcerBodyFirePistol.asset";
        public string EnforcerBodyRoll = "RoR2/Junk/Enforcer/EnforcerBodyRoll.asset";
        public string EngiBeamTurretBodyTurret = "RoR2/Junk/Engi/EngiBeamTurretBodyTurret.asset";
        public string EngiBodyFireGrenade = "RoR2/Base/Engi/EngiBodyFireGrenade.asset";
        public string EngiBodyPlaceBubbleShield = "RoR2/Base/Engi/EngiBodyPlaceBubbleShield.asset";
        public string EngiBodyPlaceMine = "RoR2/Base/Engi/EngiBodyPlaceMine.asset";
        public string EngiBodyPlaceSpiderMine = "RoR2/Base/Engi/EngiBodyPlaceSpiderMine.asset";
        public string EngiBodyPlaceTurret = "RoR2/Base/Engi/EngiBodyPlaceTurret.asset";
        public string EngiBodyPlaceWalkerTurret = "RoR2/Base/Engi/EngiBodyPlaceWalkerTurret.asset";
        public string EngiCancelTargetingDummy = "RoR2/Base/Engi/EngiCancelTargetingDummy.asset";
        public string EngiConfirmTargetDummy = "RoR2/Base/Engi/EngiConfirmTargetDummy.asset";
        public string EngiHarpoons = "RoR2/Base/Engi/EngiHarpoons.asset";
        public string EngiTurretBodyTurret = "RoR2/Base/Engi/EngiTurretBodyTurret.asset";
        public string EngiTurretFireBeam = "RoR2/Base/Engi/EngiTurretFireBeam.asset";
        public string EyeBlast = "RoR2/Base/RoboBallBoss/EyeBlast.asset";
        public string FindItem = "RoR2/Base/Scav/FindItem.asset";
        public string FireArrowSnipe = "RoR2/Base/Huntress/FireArrowSnipe.asset";
        public string FireConstructBeam = "RoR2/DLC1/MajorAndMinorConstruct/FireConstructBeam.asset";
        public string FireCorruptBeam = "RoR2/DLC1/VoidSurvivor/FireCorruptBeam.asset";
        public string FireCorruptDisk = "RoR2/DLC1/VoidSurvivor/FireCorruptDisk.asset";
        public string FireCrabBlackCannon = "RoR2/DLC1/VoidMegaCrab/FireCrabBlackCannon.asset";
        public string FireCrabWhiteCannon = "RoR2/DLC1/VoidMegaCrab/FireCrabWhiteCannon.asset";
        public string FireDelayKnockup = "RoR2/Base/RoboBallBoss/FireDelayKnockup.asset";
        public string FireExploderShards = "RoR2/Base/LunarExploder/FireExploderShards.asset";
        public string FireEyeBeam = "RoR2/Base/RoboBallBoss/FireEyeBeam.asset";
        public string FireGoldFist = "RoR2/Base/Titan/FireGoldFist.asset";
        public string FireHandBeam = "RoR2/DLC1/VoidSurvivor/FireHandBeam.asset";
        public string FireHook = "RoR2/Base/Loader/FireHook.asset";
        public string FireLunarShards = "RoR2/Base/Brother/FireLunarShards.asset";
        public string FireLunarShardsHurt = "RoR2/Base/Brother/FireLunarShardsHurt.asset";
        public string FireNullifier = "RoR2/Base/Nullifier/FireNullifier.asset";
        public string FireRandomProjectiles = "RoR2/Base/BrotherHaunt/FireRandomProjectiles.asset";
        public string FireSolarFlares = "RoR2/Base/ArtifactShell/FireSolarFlares.asset";
        public string FireTrackingLaser = "RoR2/DLC1/MajorAndMinorConstruct/FireTrackingLaser.asset";
        public string FireVoidMissiles = "RoR2/DLC1/VoidMegaCrab/FireVoidMissiles.asset";
        public string FireYankHook = "RoR2/Base/Loader/FireYankHook.asset";
        public string FistSlam = "RoR2/Base/Brother/FistSlam.asset";
        public string FlameDroneBodyFlamethrower = "RoR2/Base/Drones/FlameDroneBodyFlamethrower.asset";
        public string FlyToLand = "RoR2/Base/Vulture/FlyToLand.asset";
        public string GolemBodyClap = "RoR2/Base/Golem/GolemBodyClap.asset";
        public string GolemBodyLaser = "RoR2/Base/Golem/GolemBodyLaser.asset";
        public string GrandParentChannelSun = "RoR2/Base/Grandparent/GrandParentChannelSun.asset";
        public string GrandParentSecondary = "RoR2/Base/Grandparent/GrandParentSecondary.asset";
        public string GravekeeperBodyBarrage = "RoR2/Base/Gravekeeper/GravekeeperBodyBarrage.asset";
        public string GravekeeperBodyPrepHook = "RoR2/Base/Gravekeeper/GravekeeperBodyPrepHook.asset";
        public string GreaterWispBodyCannons = "RoR2/Base/GreaterWisp/GreaterWispBodyCannons.asset";
        public string GupSpikes = "RoR2/DLC1/Gup/GupSpikes.asset";
        public string HANDBodyFireWinch = "RoR2/Junk/HAND/HANDBodyFire Winch.asset";
        public string HANDBodyFullSwing = "RoR2/Junk/HAND/HANDBodyFullSwing.asset";
        public string HANDBodyOverclock = "RoR2/Junk/HAND/HANDBodyOverclock.asset";
        public string HANDBodySlam = "RoR2/Junk/HAND/HANDBodySlam.asset";
        public string HereticDefaultAbility = "RoR2/Base/Heretic/HereticDefaultAbility.asset";
        public string HermitCrabBodyBurrowMortar = "RoR2/Base/HermitCrab/HermitCrabBodyBurrowMortar.asset";
        public string HuntressBodyArrowRain = "RoR2/Base/Huntress/HuntressBodyArrowRain.asset";
        public string HuntressBodyBlink = "RoR2/Base/Huntress/HuntressBodyBlink.asset";
        public string HuntressBodyMiniBlink = "RoR2/Base/Huntress/HuntressBodyMiniBlink.asset";
        public string ImpBodyBlink = "RoR2/Base/Imp/ImpBodyBlink.asset";
        public string ImpBodyDoubleSlash = "RoR2/Base/Imp/ImpBodyDoubleSlash.asset";
        public string ImpBossBodyBlink = "RoR2/Base/ImpBoss/ImpBossBodyBlink.asset";
        public string ImpBossBodyFireVoidspikes = "RoR2/Base/ImpBoss/ImpBossBodyFireVoidspikes.asset";
        public string ImpBossBodyGroundPound = "RoR2/Base/ImpBoss/ImpBossBodyGroundPound.asset";
        public string JellyfishBodyNova = "RoR2/Base/Jellyfish/JellyfishBodyNova.asset";
        public string LarvaLeap = "RoR2/DLC1/AcidLarva/LarvaLeap.asset";
        public string LemurianBodyBite = "RoR2/Base/Lemurian/LemurianBodyBite.asset";
        public string LemurianBodyFireball = "RoR2/Base/Lemurian/LemurianBodyFireball.asset";
        public string LemurianBruiserBodyPrimary = "RoR2/Base/LemurianBruiser/LemurianBruiserBodyPrimary.asset";
        public string LemurianBruiserBodySecondary = "RoR2/Base/LemurianBruiser/LemurianBruiserBodySecondary.asset";
        public string LockOn = "RoR2/Base/Spectator/LockOn.asset";
        public string LunarClawSpecialReplacement = "RoR2/Junk/LunarSkillReplacements/LunarClawSpecialReplacement.asset";
        public string LunarGolemBodyShield = "RoR2/Base/LunarGolem/LunarGolemBodyShield.asset";
        public string LunarGolemBodyTwinShot = "RoR2/Base/LunarGolem/LunarGolemBodyTwinShot.asset";
        public string LunarWispBodyMiniguns = "RoR2/Base/LunarWisp/LunarWispBodyMiniguns.asset";
        public string LunarWispBodySeekingBomb = "RoR2/Base/LunarWisp/LunarWispBodySeekingBomb.asset";
        public string MageBodyFlamethrower = "RoR2/Base/Mage/MageBodyFlamethrower.asset";
        public string MageBodyFlyUp = "RoR2/Base/Mage/MageBodyFlyUp.asset";
        public string MageBodyIceBomb = "RoR2/Base/Mage/MageBodyIceBomb.asset";
        public string MageBodyNovaBomb = "RoR2/Base/Mage/MageBodyNovaBomb.asset";
        public string MageBodyWall = "RoR2/Base/Mage/MageBodyWall.asset";
        public string MagmaWormBodyBlink = "RoR2/Base/MagmaWorm/MagmaWormBodyBlink.asset";
        public string MagmaWormBodyStanceSwitch = "RoR2/Base/MagmaWorm/MagmaWormBodyStanceSwitch.asset";
        public string MagmaWormBodySteerAtTarget = "RoR2/Base/MagmaWorm/MagmaWormBodySteerAtTarget.asset";
        public string MegaDroneBodyGun = "RoR2/Base/Drones/MegaDroneBodyGun.asset";
        public string MegaDroneBodyRocket = "RoR2/Base/Drones/MegaDroneBodyRocket.asset";
        public string MercBodyEvis = "RoR2/Base/Merc/MercBodyEvis.asset";
        public string MercBodyEvisProjectile = "RoR2/Base/Merc/MercBodyEvisProjectile.asset";
        public string MercBodyFocusedAssault = "RoR2/Base/Merc/MercBodyFocusedAssault.asset";
        public string MercBodyGroundLight = "RoR2/Junk/Merc/MercBodyGroundLight.asset";
        public string MercBodyUppercut = "RoR2/Base/Merc/MercBodyUppercut.asset";
        public string MercBodyWhirlwind = "RoR2/Base/Merc/MercBodyWhirlwind.asset";
        public string MiniMushroomSporeGrenade = "RoR2/Base/MiniMushroom/MiniMushroomSporeGrenade.asset";
        public string MissileDroneBodyGun = "RoR2/Base/Drones/MissileDroneBodyGun.asset";
        public string PaladinBodyBarrierUp = "RoR2/Junk/Paladin/PaladinBodyBarrierUp.asset";
        public string PaladinBodyLeap = "RoR2/Junk/Paladin/PaladinBodyLeap.asset";
        public string PaladinBodyRocket = "RoR2/Junk/Paladin/PaladinBodyRocket.asset";
        public string ParentLoomingPresence = "RoR2/Base/Parent/ParentLoomingPresence.asset";
        public string PortalFist = "RoR2/Junk/GrandParent/PortalFist.asset";
        public string PotMobile2BodyFireCannon = "RoR2/Junk/PotMobile2/PotMobile2BodyFireCannon.asset";
        public string PotMobileBodyFireCannon = "RoR2/Junk/PotMobile/PotMobileBodyFireCannon.asset";
        public string PrepEnergyCannon = "RoR2/Base/Scav/PrepEnergyCannon.asset";
        public string PrepSack = "RoR2/Base/Scav/PrepSack.asset";
        public string RaidCrabEyeMissiles = "RoR2/DLC1/VoidRaidCrab/RaidCrabEyeMissiles.asset";
        public string RaidCrabMultiBeam = "RoR2/DLC1/VoidRaidCrab/RaidCrabMultiBeam.asset";
        public string RaidCrabSpinBeam = "RoR2/DLC1/VoidRaidCrab/RaidCrabSpinBeam.asset";
        public string RaidCrabVacuumAttack = "RoR2/DLC1/VoidRaidCrab/RaidCrabVacuumAttack.asset";
        public string RailgunnerBodyActiveReload = "RoR2/DLC1/Railgunner/RailgunnerBodyActiveReload.asset";
        public string RailgunnerBodyFireMineBlinding = "RoR2/DLC1/Railgunner/RailgunnerBodyFireMineBlinding.asset";
        public string RailgunnerBodyFireMineConcussive = "RoR2/DLC1/Railgunner/RailgunnerBodyFireMineConcussive.asset";
        public string RaiseShield = "RoR2/DLC1/MajorAndMinorConstruct/RaiseShield.asset";
        public string ResetRevolver = "RoR2/Base/Bandit2/ResetRevolver.asset";
        public string ShopkeeperBodyKickFromShop = "RoR2/Base/Shopkeeper/ShopkeeperBodyKickFromShop.asset";
        public string SkullRevolver = "RoR2/Base/Bandit2/SkullRevolver.asset";
        public string SlashBlade = "RoR2/Base/Bandit2/SlashBlade.asset";
        public string SniperBodyFireFMJ = "RoR2/Junk/Sniper/SniperBodyFireFMJ.asset";
        public string SniperBodyFireRifle = "RoR2/Junk/Sniper/SniperBodyFireRifle.asset";
        public string SniperBodyReload = "RoR2/Junk/Sniper/SniperBodyReload.asset";
        public string SniperBodyRoll = "RoR2/Junk/Sniper/SniperBodyRoll.asset";
        public string Spit = "RoR2/DLC1/FlyingVermin/Spit.asset";
        public string SprintBash = "RoR2/Base/Brother/SprintBash.asset";
        public string SquidTurretBodyTurret = "RoR2/Base/Squid/SquidTurretBodyTurret.asset";
        public string StartHealBeam = "RoR2/Base/Drones/StartHealBeam.asset";
        public string SuperEyeblast = "RoR2/Base/RoboBallBoss/SuperEyeblast.asset";
        public string SuperFireDelayKnockup = "RoR2/Base/RoboBallBoss/SuperFireDelayKnockup.asset";
        public string SwitchStance = "RoR2/DLC1/MajorAndMinorConstruct/SwitchStance.asset";
        public string ThrowBarrel = "RoR2/DLC1/ClayGrenadier/ThrowBarrel.asset";
        public string ThrowGrenade = "RoR2/Base/Commando/ThrowGrenade.asset";
        public string ThrowPylon = "RoR2/Base/Loader/ThrowPylon.asset";
        public string ThrowSmokebomb = "RoR2/Base/Bandit2/ThrowSmokebomb.asset";
        public string ThrowStickyGrenade = "RoR2/Junk/Commando/ThrowStickyGrenade.asset";
        public string TitanBodyFist = "RoR2/Base/Titan/TitanBodyFist.asset";
        public string TitanBodyLaser = "RoR2/Base/Titan/TitanBodyLaser.asset";
        public string TitanBodyRechargeRocks = "RoR2/Base/Titan/TitanBodyRechargeRocks.asset";
        public string ToolbotBodyStunDrone = "RoR2/Base/Toolbot/ToolbotBodyStunDrone.asset";
        public string ToolbotBodySwap = "RoR2/Base/Toolbot/ToolbotBodySwap.asset";
        public string ToolbotBodyToolbotDash = "RoR2/Base/Toolbot/ToolbotBodyToolbotDash.asset";
        public string ToolbotCancelDualWield = "RoR2/Base/Toolbot/ToolbotCancelDualWield.asset";
        public string ToolbotDualWield = "RoR2/Base/Toolbot/ToolbotDualWield.asset";
        public string TreebotBodyAimMortar2 = "RoR2/Base/Treebot/TreebotBodyAimMortar2.asset";
        public string TreebotBodyAimMortarRain = "RoR2/Base/Treebot/TreebotBodyAimMortarRain.asset";
        public string TreebotBodyFireFlower2 = "RoR2/Base/Treebot/TreebotBodyFireFlower2.asset";
        public string TreebotBodyFireFruitSeed = "RoR2/Base/Treebot/TreebotBodyFireFruitSeed.asset";
        public string TreebotBodyFireSyringe = "RoR2/Base/Treebot/TreebotBodyFireSyringe.asset";
        public string TreebotBodyPlantSonicBoom = "RoR2/Base/Treebot/TreebotBodyPlantSonicBoom.asset";
        public string TreebotBodySonicBoom = "RoR2/Base/Treebot/TreebotBodySonicBoom.asset";
        public string TreebotBodySonicPull = "RoR2/Junk/Treebot/TreebotBodySonicPull.asset";
        public string Turret1BodyTurret = "RoR2/Base/Drones/Turret1BodyTurret.asset";
        public string UrchinTurretBodyTurret = "RoR2/Base/ElitePoison/UrchinTurretBodyTurret.asset";
        public string VagrantBodyChargeMegaNova = "RoR2/Base/Vagrant/VagrantBodyChargeMegaNova.asset";
        public string VagrantBodyJellyBarrage = "RoR2/Base/Vagrant/VagrantBodyJellyBarrage.asset";
        public string VagrantBodyTrackingBomb = "RoR2/Base/Vagrant/VagrantBodyTrackingBomb.asset";
        public string VoidBarnacleFire = "RoR2/DLC1/VoidBarnacle/VoidBarnacleFire.asset";
        public string VoidBlinkDown = "RoR2/DLC1/VoidSurvivor/VoidBlinkDown.asset";
        public string VoidBlinkUp = "RoR2/DLC1/VoidSurvivor/VoidBlinkUp.asset";
        public string VoidInfestorLeap = "RoR2/DLC1/EliteVoid/VoidInfestorLeap.asset";
        public string VoidJailerChargeCapture = "RoR2/DLC1/VoidJailer/VoidJailerChargeCapture.asset";
        public string VoidJailerChargeFire22 = "RoR2/DLC1/VoidJailer/VoidJailerChargeFire.asset";
        public string WeaponSlam = "RoR2/Base/Brother/WeaponSlam.asset";
        public string WispBodyFireEmber = "RoR2/Base/Wisp/WispBodyFireEmber.asset";
    }

    public class RoR2CharacterSpawnCards {
        public string cscAcidLarva = "RoR2/DLC1/AcidLarva/cscAcidLarva.asset";
        public string cscArchWisp = "RoR2/Junk/ArchWisp/cscArchWisp.asset";
        public string cscAssassin2 = "RoR2/DLC1/Assassin2/cscAssassin2.asset";
        public string cscBackupDrone = "RoR2/Base/Drones/cscBackupDrone.asset";
        public string cscBeetle = "RoR2/Base/Beetle/cscBeetle.asset";
        public string cscBeetleCrystal = "RoR2/Junk/BeetleCrystal/cscBeetleCrystal.asset";
        public string cscBeetleGuard = "RoR2/Base/Beetle/cscBeetleGuard.asset";
        public string cscBeetleGuardAlly = "RoR2/Base/BeetleGland/cscBeetleGuardAlly.asset";
        public string cscBeetleGuardCrystal = "RoR2/Junk/BeetleGuardCrystal/cscBeetleGuardCrystal.asset";
        public string cscBeetleGuardSulfur = "RoR2/Base/Beetle/cscBeetleGuardSulfur.asset";
        public string cscBeetleQueen = "RoR2/Base/Beetle/cscBeetleQueen.asset";
        public string cscBeetleQueenSulfur = "RoR2/Base/Beetle/cscBeetleQueenSulfur.asset";
        public string cscBeetleSulfur = "RoR2/Base/Beetle/cscBeetleSulfur.asset";
        public string cscBell = "RoR2/Base/Bell/cscBell.asset";
        public string cscBison = "RoR2/Base/Bison/cscBison.asset";
        public string cscBrother = "RoR2/Base/Brother/cscBrother.asset";
        public string cscBrotherGlass = "RoR2/Junk/BrotherGlass/cscBrotherGlass.asset";
        public string cscBrotherHurt = "RoR2/Base/Brother/cscBrotherHurt.asset";
        public string cscBrotherIT = "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/cscBrotherIT.asset";
        public string cscClayBoss = "RoR2/Base/ClayBoss/cscClayBoss.asset";
        public string cscClayBruiser = "RoR2/Base/ClayBruiser/cscClayBruiser.asset";
        public string cscClayGrenadier = "RoR2/DLC1/ClayGrenadier/cscClayGrenadier.asset";
        public string cscDrone1 = "RoR2/Base/Drones/cscDrone1.asset";
        public string cscDrone2 = "RoR2/Base/Drones/cscDrone2.asset";
        public string cscDroneCommander = "RoR2/DLC1/DroneCommander/cscDroneCommander.asset";
        public string cscElectricWorm = "RoR2/Base/ElectricWorm/cscElectricWorm.asset";
        public string cscFlyingVermin = "RoR2/DLC1/FlyingVermin/cscFlyingVermin.asset";
        public string cscFlyingVerminSnowy = "RoR2/DLC1/FlyingVermin/cscFlyingVerminSnowy.asset";
        public string cscGeepBody = "RoR2/DLC1/Gup/cscGeepBody.asset";
        public string cscGipBody = "RoR2/DLC1/Gup/cscGipBody.asset";
        public string cscGolem = "RoR2/Base/Golem/cscGolem.asset";
        public string cscGolemNature = "RoR2/Base/Golem/cscGolemNature.asset";
        public string cscGolemSandy = "RoR2/Base/Golem/cscGolemSandy.asset";
        public string cscGolemSnowy = "RoR2/Base/Golem/cscGolemSnowy.asset";
        public string cscGrandparent = "RoR2/Base/Grandparent/cscGrandparent.asset";
        public string cscGravekeeper = "RoR2/Base/Gravekeeper/cscGravekeeper.asset";
        public string cscGreaterWisp = "RoR2/Base/GreaterWisp/cscGreaterWisp.asset";
        public string cscGupBody = "RoR2/DLC1/Gup/cscGupBody.asset";
        public string cscHermitCrab = "RoR2/Base/HermitCrab/cscHermitCrab.asset";
        public string cscImp = "RoR2/Base/Imp/cscImp.asset";
        public string cscImpBoss = "RoR2/Base/ImpBoss/cscImpBoss.asset";
        public string cscJellyfish = "RoR2/Base/Jellyfish/cscJellyfish.asset";
        public string cscLemurian = "RoR2/Base/Lemurian/cscLemurian.asset";
        public string cscLemurianBruiser = "RoR2/Base/LemurianBruiser/cscLemurianBruiser.asset";
        public string cscLesserWisp = "RoR2/Base/Wisp/cscLesserWisp.asset";
        public string cscLunarExploder = "RoR2/Base/LunarExploder/cscLunarExploder.asset";
        public string cscLunarGolem = "RoR2/Base/LunarGolem/cscLunarGolem.asset";
        public string cscLunarWisp = "RoR2/Base/LunarWisp/cscLunarWisp.asset";
        public string cscMagmaWorm = "RoR2/Base/MagmaWorm/cscMagmaWorm.asset";
        public string cscMajorConstruct = "RoR2/DLC1/MajorAndMinorConstruct/cscMajorConstruct.asset";
        public string cscMegaConstruct = "RoR2/DLC1/MajorAndMinorConstruct/cscMegaConstruct.asset";
        public string cscMegaDrone = "RoR2/Base/Drones/cscMegaDrone.asset";
        public string cscMiniMushroom = "RoR2/Base/MiniMushroom/cscMiniMushroom.asset";
        public string cscMiniVoidRaidCrabBase = "RoR2/DLC1/VoidRaidCrab/cscMiniVoidRaidCrabBase.asset";
        public string cscMiniVoidRaidCrabPhase1 = "RoR2/DLC1/VoidRaidCrab/cscMiniVoidRaidCrabPhase1.asset";
        public string cscMiniVoidRaidCrabPhase2 = "RoR2/DLC1/VoidRaidCrab/cscMiniVoidRaidCrabPhase2.asset";
        public string cscMiniVoidRaidCrabPhase3 = "RoR2/DLC1/VoidRaidCrab/cscMiniVoidRaidCrabPhase3.asset";
        public string cscMinorConstruct = "RoR2/DLC1/MajorAndMinorConstruct/cscMinorConstruct.asset";
        public string cscMinorConstructAttachable = "RoR2/DLC1/MajorAndMinorConstruct/cscMinorConstructAttachable.asset";
        public string cscMinorConstructOnKill = "RoR2/DLC1/MajorAndMinorConstruct/cscMinorConstructOnKill.asset";
        public string cscNullifier = "RoR2/Base/Nullifier/cscNullifier.asset";
        public string cscNullifierAlly = "RoR2/Base/Nullifier/cscNullifierAlly.asset";
        public string cscParent = "RoR2/Base/Parent/cscParent.asset";
        public string cscParentPod = "RoR2/Junk/Incubator/cscParentPod.asset";
        public string cscRoboBallBoss = "RoR2/Base/RoboBallBoss/cscRoboBallBoss.asset";
        public string cscRoboBallGreenBuddy = "RoR2/Base/RoboBallBuddy/cscRoboBallGreenBuddy.asset";
        public string cscRoboBallMini = "RoR2/Base/RoboBallBoss/cscRoboBallMini.asset";
        public string cscRoboBallRedBuddy = "RoR2/Base/RoboBallBuddy/cscRoboBallRedBuddy.asset";
        public string cscScav = "RoR2/Base/Scav/cscScav.asset";
        public string cscScavBoss = "RoR2/Base/Scav/cscScavBoss.asset";
        public string cscSquidTurret = "RoR2/Base/Squid/cscSquidTurret.asset";
        public string cscSuperRoboBallBoss = "RoR2/Base/RoboBallBoss/cscSuperRoboBallBoss.asset";
        public string cscTitanBlackBeach = "RoR2/Base/Titan/cscTitanBlackBeach.asset";
        public string cscTitanDampCave = "RoR2/Base/Titan/cscTitanDampCave.asset";
        public string cscTitanGold = "RoR2/Base/Titan/cscTitanGold.asset";
        public string cscTitanGoldAlly = "RoR2/Base/TitanGoldDuringTP/cscTitanGoldAlly.asset";
        public string cscTitanGolemPlains = "RoR2/Base/Titan/cscTitanGolemPlains.asset";
        public string cscTitanGooLake = "RoR2/Base/Titan/cscTitanGooLake.asset";
        public string cscVagrant = "RoR2/Base/Vagrant/cscVagrant.asset";
        public string cscVermin = "RoR2/DLC1/Vermin/cscVermin.asset";
        public string cscVerminSnowy = "RoR2/DLC1/Vermin/cscVerminSnowy.asset";
        public string cscVoidBarnacle = "RoR2/DLC1/VoidBarnacle/cscVoidBarnacle.asset";
        public string cscVoidBarnacleAlly = "RoR2/DLC1/VoidBarnacle/cscVoidBarnacleAlly.asset";
        public string cscVoidBarnacleNoCast = "RoR2/DLC1/VoidBarnacle/cscVoidBarnacleNoCast.asset";
        public string cscVoidInfestor = "RoR2/DLC1/EliteVoid/cscVoidInfestor.asset";
        public string cscVoidJailer = "RoR2/DLC1/VoidJailer/cscVoidJailer.asset";
        public string cscVoidJailerAlly = "RoR2/DLC1/VoidJailer/cscVoidJailerAlly.asset";
        public string cscVoidMegaCrab = "RoR2/DLC1/VoidMegaCrab/cscVoidMegaCrab.asset";
        public string cscVoidMegaCrabAlly = "RoR2/DLC1/VoidMegaCrab/cscVoidMegaCrabAlly.asset";
        public string cscVoidRaidCrab = "RoR2/DLC1/VoidRaidCrab/cscVoidRaidCrab.asset";
        public string cscVoidRaidCrabJoint = "RoR2/DLC1/VoidRaidCrab/cscVoidRaidCrabJoint.asset";
        public string cscVulture = "RoR2/Base/Vulture/cscVulture.asset";
    }

    public class RoR2CaptainOrbitalSkillDefs {
        public string PrepAirstrike = "RoR2/Base/Captain/PrepAirstrike.asset";
        public string PrepAirstrikeAlt = "RoR2/Base/Captain/PrepAirstrikeAlt.asset";
    }

    public class RoR2CaptainSupplyDropSkillDefs {
        public string PrepSupplyDrop = "RoR2/Base/Captain/PrepSupplyDrop.asset";
    }

    public class RoR2SteppedSkillDefs {
        public string BigPunch = "RoR2/Junk/Loader/BigPunch.asset";
        public string ChargeFist = "RoR2/Base/Loader/ChargeFist.asset";
        public string ChargeZapFist = "RoR2/Base/Loader/ChargeZapFist.asset";
        public string CommandoBodyFirePistol = "RoR2/Base/Commando/CommandoBodyFirePistol.asset";
        public string CrocoSlash5 = "RoR2/Base/Croco/CrocoSlash.asset";
        public string GrandParentGroundSwipe = "RoR2/Base/Grandparent/GrandParentGroundSwipe.asset";
        public string GroundSlam = "RoR2/Base/Loader/GroundSlam.asset";
        public string MageBodyFireFirebolt = "RoR2/Base/Mage/MageBodyFireFirebolt.asset";
        public string MageBodyFireIceBolt = "RoR2/Junk/Mage/MageBodyFireIceBolt.asset";
        public string MageBodyFireLightningBolt = "RoR2/Base/Mage/MageBodyFireLightningBolt.asset";
        public string MercGroundLight2 = "RoR2/Base/Merc/MercGroundLight2.asset";
        public string SwingFist = "RoR2/Base/Loader/SwingFist.asset";
        public string SwingMelee = "RoR2/DLC1/VoidSurvivor/SwingMelee.asset";
        public string TongueLash = "RoR2/DLC1/Vermin/TongueLash.asset";
    }

    public class RoR2InteractableSpawnCards {
        public string iscBarrel1 = "RoR2/Base/Barrel1/iscBarrel1.asset";
        public string iscBrokenDrone1 = "RoR2/Base/Drones/iscBrokenDrone1.asset";
        public string iscBrokenDrone2 = "RoR2/Base/Drones/iscBrokenDrone2.asset";
        public string iscBrokenEmergencyDrone = "RoR2/Base/Drones/iscBrokenEmergencyDrone.asset";
        public string iscBrokenEquipmentDrone = "RoR2/Base/Drones/iscBrokenEquipmentDrone.asset";
        public string iscBrokenFlameDrone = "RoR2/Base/Drones/iscBrokenFlameDrone.asset";
        public string iscBrokenMegaDrone = "RoR2/Base/Drones/iscBrokenMegaDrone.asset";
        public string iscBrokenMissileDrone = "RoR2/Base/Drones/iscBrokenMissileDrone.asset";
        public string iscBrokenTurret1 = "RoR2/Base/Drones/iscBrokenTurret1.asset";
        public string iscCasinoChest = "RoR2/Base/CasinoChest/iscCasinoChest.asset";
        public string iscCategoryChest2Damage = "RoR2/DLC1/CategoryChest2/iscCategoryChest2Damage.asset";
        public string iscCategoryChest2Healing = "RoR2/DLC1/CategoryChest2/iscCategoryChest2Healing.asset";
        public string iscCategoryChest2Utility = "RoR2/DLC1/CategoryChest2/iscCategoryChest2Utility.asset";
        public string iscCategoryChestDamage = "RoR2/Base/CategoryChest/iscCategoryChestDamage.asset";
        public string iscCategoryChestHealing = "RoR2/Base/CategoryChest/iscCategoryChestHealing.asset";
        public string iscCategoryChestUtility = "RoR2/Base/CategoryChest/iscCategoryChestUtility.asset";
        public string iscChest1 = "RoR2/Base/Chest1/iscChest1.asset";
        public string iscChest1Stealthed = "RoR2/Base/Chest1StealthedVariant/iscChest1Stealthed.asset";
        public string iscChest2 = "RoR2/Base/Chest2/iscChest2.asset";
        public string iscDeepVoidPortal = "RoR2/DLC1/DeepVoidPortal/iscDeepVoidPortal.asset";
        public string iscDeepVoidPortalBattery = "RoR2/DLC1/DeepVoidPortalBattery/iscDeepVoidPortalBattery.asset";
        public string iscDuplicator = "RoR2/Base/Duplicator/iscDuplicator.asset";
        public string iscDuplicatorLarge = "RoR2/Base/DuplicatorLarge/iscDuplicatorLarge.asset";
        public string iscDuplicatorMilitary = "RoR2/Base/DuplicatorMilitary/iscDuplicatorMilitary.asset";
        public string iscDuplicatorWild = "RoR2/Base/DuplicatorWild/iscDuplicatorWild.asset";
        public string iscEquipmentBarrel = "RoR2/Base/EquipmentBarrel/iscEquipmentBarrel.asset";
        public string iscFreeChest = "RoR2/DLC1/FreeChest/iscFreeChest.asset";
        public string iscGauntletEntrance = "RoR2/DLC1/gauntlets/iscGauntletEntrance.asset";
        public string iscGoldChest = "RoR2/Base/GoldChest/iscGoldChest.asset";
        public string iscGoldshoresBeacon = "RoR2/Base/goldshores/iscGoldshoresBeacon.asset";
        public string iscGoldshoresPortal = "RoR2/Base/PortalGoldshores/iscGoldshoresPortal.asset";
        public string iscInfiniteTowerPortal = "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/iscInfiniteTowerPortal.asset";
        public string iscInfiniteTowerSafeWard = "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/iscInfiniteTowerSafeWard.asset";
        public string iscInfiniteTowerSafeWardAwaitingInteraction = "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/iscInfiniteTowerSafeWardAwaitingInteraction.asset";
        public string iscLockbox = "RoR2/Junk/TreasureCache/iscLockbox.asset";
        public string iscLockboxVoid = "RoR2/DLC1/TreasureCacheVoid/iscLockboxVoid.asset";
        public string iscLunarChest = "RoR2/Base/LunarChest/iscLunarChest.asset";
        public string iscLunarTeleporter = "RoR2/Base/Teleporters/iscLunarTeleporter.asset";
        public string iscMSPortal = "RoR2/Base/PortalMS/iscMSPortal.asset";
        public string iscRadarTower = "RoR2/Base/RadarTower/iscRadarTower.asset";
        public string iscScavBackpack = "RoR2/Base/Scav/iscScavBackpack.asset";
        public string iscScavLunarBackpack = "RoR2/Base/Scav/iscScavLunarBackpack.asset";
        public string iscScrapper = "RoR2/Base/Scrapper/iscScrapper.asset";
        public string iscShopPortal = "RoR2/Base/PortalShop/iscShopPortal.asset";
        public string iscShrineBlood = "RoR2/Base/ShrineBlood/iscShrineBlood.asset";
        public string iscShrineBloodSandy = "RoR2/Base/ShrineBlood/iscShrineBloodSandy.asset";
        public string iscShrineBloodSnowy = "RoR2/Base/ShrineBlood/iscShrineBloodSnowy.asset";
        public string iscShrineBoss = "RoR2/Base/ShrineBoss/iscShrineBoss.asset";
        public string iscShrineBossSandy = "RoR2/Base/ShrineBoss/iscShrineBossSandy.asset";
        public string iscShrineBossSnowy = "RoR2/Base/ShrineBoss/iscShrineBossSnowy.asset";
        public string iscShrineChance = "RoR2/Base/ShrineChance/iscShrineChance.asset";
        public string iscShrineChanceSandy = "RoR2/Base/ShrineChance/iscShrineChanceSandy.asset";
        public string iscShrineChanceSnowy = "RoR2/Base/ShrineChance/iscShrineChanceSnowy.asset";
        public string iscShrineCleanse = "RoR2/Base/ShrineCleanse/iscShrineCleanse.asset";
        public string iscShrineCleanseSandy = "RoR2/Base/ShrineCleanse/iscShrineCleanseSandy.asset";
        public string iscShrineCleanseSnowy = "RoR2/Base/ShrineCleanse/iscShrineCleanseSnowy.asset";
        public string iscShrineCombat = "RoR2/Base/ShrineCombat/iscShrineCombat.asset";
        public string iscShrineCombatSandy = "RoR2/Base/ShrineCombat/iscShrineCombatSandy.asset";
        public string iscShrineCombatSnowy = "RoR2/Base/ShrineCombat/iscShrineCombatSnowy.asset";
        public string iscShrineGoldshoresAccess = "RoR2/Base/ShrineGoldshoresAccess/iscShrineGoldshoresAccess.asset";
        public string iscShrineHealing = "RoR2/Base/ShrineHealing/iscShrineHealing.asset";
        public string iscShrineRestack = "RoR2/Base/ShrineRestack/iscShrineRestack.asset";
        public string iscShrineRestackSandy = "RoR2/Base/ShrineRestack/iscShrineRestackSandy.asset";
        public string iscShrineRestackSnowy = "RoR2/Base/ShrineRestack/iscShrineRestackSnowy.asset";
        public string iscSquidTurret = "RoR2/Junk/SquidTurret/iscSquidTurret.asset";
        public string iscTeleporter = "RoR2/Base/Teleporters/iscTeleporter.asset";
        public string iscTripleShop = "RoR2/Base/TripleShop/iscTripleShop.asset";
        public string iscTripleShopEquipment = "RoR2/Base/TripleShopEquipment/iscTripleShopEquipment.asset";
        public string iscTripleShopLarge = "RoR2/Base/TripleShopLarge/iscTripleShopLarge.asset";
        public string iscVoidCamp = "RoR2/DLC1/VoidCamp/iscVoidCamp.asset";
        public string iscVoidChest = "RoR2/DLC1/VoidChest/iscVoidChest.asset";
        public string iscVoidChestSacrificeOn = "RoR2/DLC1/VoidChest/iscVoidChestSacrificeOn.asset";
        public string iscVoidCoinBarrel = "RoR2/DLC1/VoidCoinBarrel/iscVoidCoinBarrel.asset";
        public string iscVoidOutroPortal = "RoR2/DLC1/VoidOutroPortal/iscVoidOutroPortal.asset";
        public string iscVoidPortal = "RoR2/DLC1/PortalVoid/iscVoidPortal.asset";
        public string iscVoidRaidSafeWard = "RoR2/DLC1/VoidRaidCrab/iscVoidRaidSafeWard.asset";
        public string iscVoidSuppressor = "RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset";
        public string iscVoidTriple = "RoR2/DLC1/VoidTriple/iscVoidTriple.asset";
    }

    public class RoR2HuntressTrackingSkillDefs {
        public string FireFlurrySeekingArrow = "RoR2/Base/Huntress/FireFlurrySeekingArrow.asset";
        public string HuntressBodyFireSeekingArrow = "RoR2/Base/Huntress/HuntressBodyFireSeekingArrow.asset";
        public string HuntressBodyGlaive = "RoR2/Base/Huntress/HuntressBodyGlaive.asset";
    }

    public class RoR2MercDashSkillDefs {
        public string MercBodyAssaulter = "RoR2/Base/Merc/MercBodyAssaulter.asset";
    }

    public class RoR2MultiCharacterSpawnCards {
        public string cscScavLunar = "RoR2/Base/ScavLunar/cscScavLunar.asset";
    }

    public class RoR2ToolbotWeaponSkillDefs {
        public string ToolbotBodyFireBuzzsaw = "RoR2/Base/Toolbot/ToolbotBodyFireBuzzsaw.asset";
        public string ToolbotBodyFireGrenadeLauncher = "RoR2/Base/Toolbot/ToolbotBodyFireGrenadeLauncher.asset";
        public string ToolbotBodyFireNailgun = "RoR2/Base/Toolbot/ToolbotBodyFireNailgun.asset";
        public string ToolbotBodyFireSpear = "RoR2/Base/Toolbot/ToolbotBodyFireSpear.asset";
    }

    public class RoR2LunarDetonatorSkills {
        public string LunarDetonatorSpecialReplacement = "RoR2/Base/LunarSkillReplacements/LunarDetonatorSpecialReplacement.asset";
    }

    public class RoR2SceneDefs {
        public string aitest = "RoR2/Junk/ai_test/ai_test.asset";
        public string ancientloft = "RoR2/DLC1/ancientloft/ancientloft.asset";
        public string arena = "RoR2/Base/arena/arena.asset";
        public string artifactworld = "RoR2/Base/artifactworld/artifactworld.asset";
        public string bazaar = "RoR2/Base/bazaar/bazaar.asset";
        public string blackbeach = "RoR2/Base/blackbeach/blackbeach.asset";
        public string blackbeach2 = "RoR2/Base/blackbeach2/blackbeach2.asset";
        public string crystalworld = "RoR2/Base/crystalworld/crystalworld.asset";
        public string dampcavesimple = "RoR2/Base/dampcavesimple/dampcavesimple.asset";
        public string eclipseworld = "RoR2/Base/eclipseworld/eclipseworld.asset";
        public string foggyswamp = "RoR2/Base/foggyswamp/foggyswamp.asset";
        public string frozenwall = "RoR2/Base/frozenwall/frozenwall.asset";
        public string goldshores = "RoR2/Base/goldshores/goldshores.asset";
        public string golemplains = "RoR2/Base/golemplains/golemplains.asset";
        public string golemplains2 = "RoR2/Base/golemplains2/golemplains2.asset";
        public string goolake = "RoR2/Base/goolake/goolake.asset";
        public string infinitetowerworld = "RoR2/DLC1/infinitetowerworld/infinitetowerworld.asset";
        public string intro = "RoR2/Base/intro/intro.asset";
        public string itancientloft = "RoR2/DLC1/itancientloft/itancientloft.asset";
        public string itdampcave = "RoR2/DLC1/itdampcave/itdampcave.asset";
        public string itfrozenwall = "RoR2/DLC1/itfrozenwall/itfrozenwall.asset";
        public string itgolemplains = "RoR2/DLC1/itgolemplains/itgolemplains.asset";
        public string itgoolake = "RoR2/DLC1/itgoolake/itgoolake.asset";
        public string itmoon = "RoR2/DLC1/itmoon/itmoon.asset";
        public string itskymeadow = "RoR2/DLC1/itskymeadow/itskymeadow.asset";
        public string limbo = "RoR2/Base/limbo/limbo.asset";
        public string loadingbasic = "RoR2/Base/loadingbasic/loadingbasic.asset";
        public string lobby = "RoR2/Base/lobby/lobby.asset";
        public string logbook = "RoR2/Base/logbook/logbook.asset";
        public string moon = "RoR2/Base/moon/moon.asset";
        public string moon2 = "RoR2/Base/moon2/moon2.asset";
        public string mysteryspace = "RoR2/Base/mysteryspace/mysteryspace.asset";
        public string outro = "RoR2/Base/outro/outro.asset";
        public string PromoRailGunner = "RoR2/DLC1/PromoRailGunner.asset";
        public string PromoVoidSurvivor = "RoR2/DLC1/PromoVoidSurvivor.asset";
        public string rootjungle = "RoR2/Base/rootjungle/rootjungle.asset";
        public string satellitescene = "RoR2/Junk/satellitescene/satellitescene.asset";
        public string shipgraveyard = "RoR2/Base/shipgraveyard/shipgraveyard.asset";
        public string skymeadow = "RoR2/Base/skymeadow/skymeadow.asset";
        public string snowyforest = "RoR2/DLC1/snowyforest/snowyforest.asset";
        public string splash = "RoR2/Base/splash/splash.asset";
        public string sulfurpools = "RoR2/DLC1/sulfurpools/sulfurpools.asset";
        public string testscene = "RoR2/Junk/testscene/testscene.asset";
        public string title = "RoR2/Base/title/title.asset";
        public string voidoutro = "RoR2/DLC1/voidoutro/voidoutro.asset";
        public string voidraid = "RoR2/DLC1/voidraid/voidraid.asset";
        public string voidstage = "RoR2/DLC1/voidstage/voidstage.asset";
        public string wispgraveyard = "RoR2/Base/wispgraveyard/wispgraveyard.asset";
    }

    public class RoR2MasterSpawnSlotSkillDefs {
        public string SpawnMinorConstructs = "RoR2/DLC1/MajorAndMinorConstruct/SpawnMinorConstructs.asset";
    }

    public class RoR2RailgunSkillDefs {
        public string RailgunnerBodyChargeSnipeCryo = "RoR2/DLC1/Railgunner/RailgunnerBodyChargeSnipeCryo.asset";
        public string RailgunnerBodyChargeSnipeSuper = "RoR2/DLC1/Railgunner/RailgunnerBodyChargeSnipeSuper.asset";
        public string RailgunnerBodyFirePistol = "RoR2/DLC1/Railgunner/RailgunnerBodyFirePistol.asset";
        public string RailgunnerBodyFireSnipeCryo = "RoR2/DLC1/Railgunner/RailgunnerBodyFireSnipeCryo.asset";
        public string RailgunnerBodyFireSnipeHeavy = "RoR2/DLC1/Railgunner/RailgunnerBodyFireSnipeHeavy.asset";
        public string RailgunnerBodyFireSnipeLight = "RoR2/DLC1/Railgunner/RailgunnerBodyFireSnipeLight.asset";
        public string RailgunnerBodyFireSnipeSuper = "RoR2/DLC1/Railgunner/RailgunnerBodyFireSnipeSuper.asset";
        public string RailgunnerBodyScopeHeavy = "RoR2/DLC1/Railgunner/RailgunnerBodyScopeHeavy.asset";
        public string RailgunnerBodyScopeLight = "RoR2/DLC1/Railgunner/RailgunnerBodyScopeLight.asset";
    }

    public class RoR2PassiveItemSkillDefs {
        public string RailgunnerBodyPassiveConvertCrit = "RoR2/DLC1/Railgunner/RailgunnerBodyPassiveConvertCrit.asset";
        public string VoidSurvivorPassive = "RoR2/DLC1/VoidSurvivor/VoidSurvivorPassive.asset";
    }

    public class RoR2VoidRaidCrabBodySkillDefs {
        public string RaidCrabChannelGauntlet = "RoR2/DLC1/VoidRaidCrab/RaidCrabChannelGauntlet.asset";
        public string RaidCrabFinalStand = "RoR2/DLC1/VoidRaidCrab/RaidCrabFinalStand.asset";
        public string RaidCrabGravityBump = "RoR2/DLC1/VoidRaidCrab/RaidCrabGravityBump.asset";
        public string RaidCrabWardWipe = "RoR2/DLC1/VoidRaidCrab/RaidCrabWardWipe.asset";
    }

    public class RoR2VoidSurvivorSkillDefs {
        public string CrushCorruption = "RoR2/DLC1/VoidSurvivor/CrushCorruption.asset";
    }

    public class RoR2SpawnCards {
        public string scVoidCampGrass = "RoR2/DLC1/VoidCamp/scVoidCampGrass.asset";
        public string scVoidCampKelp = "RoR2/DLC1/VoidCamp/scVoidCampKelp.asset";
        public string scVoidCampTallGrassCluster1 = "RoR2/DLC1/VoidCamp/scVoidCampTallGrassCluster1.asset";
        public string scVoidCampTallGrassCluster2 = "RoR2/DLC1/VoidCamp/scVoidCampTallGrassCluster2.asset";
        public string scVoidCampTallGrassCluster3 = "RoR2/DLC1/VoidCamp/scVoidCampTallGrassCluster3.asset";
        public string scVoidCampXYZ = "RoR2/DLC1/VoidCamp/scVoidCampXYZ.asset";
        public string scVoidCampXYZOpen = "RoR2/DLC1/VoidCamp/scVoidCampXYZOpen.asset";
    }

    public class RoR2FreeChestDropTables {
        public string dtFreeChest = "RoR2/DLC1/FreeChest/dtFreeChest.asset";
    }

    public class RoR2EngiMineDeployerSkills {
        public string ThrowMineDeployer = "RoR2/Junk/Engi/ThrowMineDeployer.asset";
    }
}