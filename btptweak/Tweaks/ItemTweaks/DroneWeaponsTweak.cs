using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class DroneWeaponsTweak : TweakBase<DroneWeaponsTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.DroneWeaponsBehavior.TrySpawnDrone += DroneWeaponsBehavior_TrySpawnDrone;
            On.RoR2.DroneWeaponsBehavior.OnMasterSpawned += DroneWeaponsBehavior_OnMasterSpawned;
            On.RoR2.DroneWeaponsBoostBehavior.OnEnemyHit += DroneWeaponsBoostBehavior_OnEnemyHit;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.DroneCommanderMaster.Load<GameObject>().AddComponent<Deployable>();
        }

        private void DroneWeaponsBoostBehavior_OnEnemyHit(On.RoR2.DroneWeaponsBoostBehavior.orig_OnEnemyHit orig, DroneWeaponsBoostBehavior self, DamageInfo damageInfo, CharacterBody victimBody) {
            if (!damageInfo.procChainMask.HasProc(ProcType.MicroMissile)
                && victimBody.mainHurtBox) {
                var body = self.body;
                if (Util.CheckRoll(10f * self.stack * damageInfo.procCoefficient, body.master)) {
                    var microMissileOrb = new MicroMissileOrb {
                        attacker = body.gameObject,
                        damageColorIndex = DamageColorIndex.Item,
                        damageValue = Util.OnHitProcDamage(damageInfo.damage, body.damage, self.stack),
                        isCrit = damageInfo.crit,
                        origin = self.missileMuzzleTransform ? self.missileMuzzleTransform.position : body.corePosition,
                        procChainMask = damageInfo.procChainMask,
                        procCoefficient = 0.5f,
                        target = victimBody.mainHurtBox,
                        teamIndex = body.teamComponent.teamIndex,
                    };
                    microMissileOrb.procChainMask.AddProc(ProcType.MicroMissile);
                    OrbManager.instance.AddOrb(microMissileOrb);
                }
            }
        }

        private void DroneWeaponsBehavior_TrySpawnDrone(On.RoR2.DroneWeaponsBehavior.orig_TrySpawnDrone orig, DroneWeaponsBehavior self) {
            if (self.body.master.IsDeployableSlotAvailable(DeployableSlot.DroneWeaponsDrone)) {
                self.spawnDelay = 10f;
                var directorSpawnRequest = new DirectorSpawnRequest(self.droneSpawnCard, self.placementRule, self.rng) {
                    summonerBodyObject = self.gameObject,
                    onSpawnedServer = self.OnMasterSpawned,
                    ignoreTeamMemberLimit = false,
                };
                DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            }
        }

        private void DroneWeaponsBehavior_OnMasterSpawned(On.RoR2.DroneWeaponsBehavior.orig_OnMasterSpawned orig, DroneWeaponsBehavior self, SpawnCard.SpawnResult spawnResult) {
            if (spawnResult.success && spawnResult.spawnedInstance.TryGetComponent<Deployable>(out var deployable)) {
                self.body.master.AddDeployable(deployable, DeployableSlot.DroneWeaponsDrone);
            }
        }
    }
}