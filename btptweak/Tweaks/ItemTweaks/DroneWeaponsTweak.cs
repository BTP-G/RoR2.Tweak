using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class DroneWeaponsTweak : TweakBase<DroneWeaponsTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.DroneWeaponsBehavior.TrySpawnDrone += DroneWeaponsBehavior_TrySpawnDrone;
            On.RoR2.DroneWeaponsBehavior.OnMasterSpawned += DroneWeaponsBehavior_OnMasterSpawned;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.DroneCommanderMaster.Load<GameObject>().AddComponent<Deployable>();
        }

        private void DroneWeaponsBehavior_TrySpawnDrone(On.RoR2.DroneWeaponsBehavior.orig_TrySpawnDrone orig, DroneWeaponsBehavior self) {
            if (self.body.master.IsDeployableLimited(DeployableSlot.DroneWeaponsDrone)
                || self.body.HasBuff(BtpContent.Buffs.DroneCommanderSpawnCooldown)) {
                return;
            }
            self.spawnDelay = 1f;
            var directorSpawnRequest = new DirectorSpawnRequest(self.droneSpawnCard, self.placementRule, self.rng) {
                summonerBodyObject = self.gameObject,
                onSpawnedServer = self.OnMasterSpawned
            };
            DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
        }

        private void DroneWeaponsBehavior_OnMasterSpawned(On.RoR2.DroneWeaponsBehavior.orig_OnMasterSpawned orig, DroneWeaponsBehavior self, SpawnCard.SpawnResult spawnResult) {
            if ((self.hasSpawnedDrone = spawnResult.success) && spawnResult.spawnedInstance.TryGetComponent<CharacterMaster>(out var master)) {
                if (spawnResult.spawnedInstance.TryGetComponent<Deployable>(out var deployable)) {
                    self.body.master.AddDeployable(deployable, DeployableSlot.DroneWeaponsDrone);
                }
                master.onBodyDeath.AddListener(() => {
                    if (self) {
                        self.hasSpawnedDrone = false;
                        self.body.AddTimedBuff(BtpContent.Buffs.DroneCommanderSpawnCooldown, 60f / self.stack);
                    }
                });
                master.inventory.AddItemsFrom(self.body.inventory);
                master.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 60);
                master.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
                master.inventory.ResetItem(DLC1Content.Items.ExtraLifeVoid.itemIndex);
                master.inventory.ResetItem(RoR2Content.Items.UseAmbientLevel.itemIndex);
            }
        }
    }
}