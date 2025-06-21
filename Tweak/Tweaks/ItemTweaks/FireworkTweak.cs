using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class FireworkTweak : ModComponent, IRoR2LoadedMessageHandler {
        public const int BaseDamageCoefficient = 1;
        public const int FireCount = 6;

        void IRoR2LoadedMessageHandler.Handle() {
            AssetReferences.fireworkPrefab.Asset.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.fireworkPrefab.Asset.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.2f;
            AssetReferences.fireworkPrefab.Asset.GetComponent<QuaternionPID>().gain *= 100;
            var missileController = AssetReferences.fireworkPrefab.Asset.GetComponent<MissileController>();
            missileController.acceleration *= 2f;
            missileController.delayTimer *= 0.5f;
            missileController.maxSeekDistance = float.MaxValue;
            missileController.turbulence = 0;
        }

        private void EquipmentSlot_onServerEquipmentActivated(EquipmentSlot equipmentSlot, EquipmentIndex equipmentIndex) {
            var itemCount = equipmentSlot.inventory.GetItemCount(RoR2Content.Items.Firework.itemIndex);
            if (itemCount > 0) {
                var body = equipmentSlot.characterBody;
                if (!body) {
                    return;
                }
                var fireworkLauncher = body.gameObject.GetComponentInChildren<FireworkLauncher>();
                if (fireworkLauncher) {
                    fireworkLauncher.remaining += FireCount * itemCount;
                } else {
                    fireworkLauncher = Object.Instantiate(AssetReferences.fireworkLauncher.Asset, body.corePosition, Quaternion.identity).GetComponent<FireworkLauncher>();
                    fireworkLauncher.transform.parent = body.gameObject.transform;
                    fireworkLauncher.owner = body.gameObject;
                    fireworkLauncher.launchInterval = 0.125f;
                    fireworkLauncher.remaining = FireCount * itemCount;
                }
            }
        }

        private void GlobalEventManager_OnInteractionBegin(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(MoveType.After,
                                   x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Firework")),
                                   x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("Firework :: Hook Failed!");
            }
        }
    }
}