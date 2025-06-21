using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class RandomEquipmentTriggerTweak : ModComponent, IModLoadMessageHandler {

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.EquipmentSlot.OnEquipmentExecuted += EquipmentSlot_OnEquipmentExecuted;
            CharacterBody.onBodyInventoryChangedGlobal += CharacterBody_onBodyInventoryChangedGlobal;
        }

        private void CharacterBody_onBodyInventoryChangedGlobal(CharacterBody body) {
            if (NetworkServer.active) {
                body.AddItemBehavior<RandomEquipmentTriggerBehavior>(body.inventory.GetItemCount(DLC1Content.Items.RandomEquipmentTrigger.itemIndex));
            }
        }

        private void EquipmentSlot_OnEquipmentExecuted(ILContext il) {
            var cursor = new ILCursor(il);
            cursor.GotoNext(MoveType.After,
                            x => x.MatchLdsfld(typeof(DLC1Content.Items), "RandomEquipmentTrigger"),
                            x => x.MatchCallvirt<Inventory>("GetItemCount"))
                  .Emit(OpCodes.Pop)
                  .Emit(OpCodes.Ldc_I4_0);
        }

        public class RandomEquipmentTriggerBehavior : CharacterBody.ItemBehavior {
            public const float transformInterval = 30f;
            public const int transformCountPerStack = 3;
            private readonly Xoroshiro128Plus transformRng = new(Run.instance.runRNG.nextUlong);
            private float transformTimer = 0f;

            private void FixedUpdate() {
                if ((transformTimer += Time.fixedDeltaTime) > transformInterval) {
                    var inventory = body.inventory;
                    var oldIndex = transformRng.NextElementUniform(inventory.itemAcquisitionOrder);
                    var pickupIndices = PickupTransmutationManager.GetAvailableGroupFromPickupIndex(PickupCatalog.FindPickupIndex(oldIndex));
                    if (pickupIndices == null || pickupIndices.Length == 0) {
                        return;
                    }
                    var newIndex = PickupCatalog.GetPickupDef(transformRng.NextElementUniform(pickupIndices))?.itemIndex ?? ItemIndex.None;
                    if (newIndex == ItemIndex.None) {
                        return;
                    }
                    var transformCount = Mathf.Min(inventory.GetItemCount(oldIndex), transformCountPerStack * stack);
                    inventory.RemoveItem(oldIndex, transformCount);
                    inventory.GiveItem(newIndex, transformCount);
                    transformTimer = 0;
                    CharacterMasterNotificationQueue.SendTransformNotification(body.master, oldIndex, newIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
                    var effectData = new EffectData {
                        origin = body.corePosition,
                    };
                    effectData.SetNetworkedObjectReference(gameObject);
                    EffectManager.SpawnEffect(AssetReferences.randomEquipmentTriggerProcEffect, effectData, true);
                }
            }
        }
    }
}