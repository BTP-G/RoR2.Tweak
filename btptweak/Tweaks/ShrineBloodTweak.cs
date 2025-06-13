using BTP.RoR2Plugin.Utils;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks {

    internal class ShrineBloodTweak : TweakBase<ShrineBloodTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.ShrineBloodBehavior.AddShrineStack += ShrineBloodBehavior_AddShrineStack;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteraction_onTeleporterBeginChargingGlobal;
        }

        private void ShrineBloodBehavior_AddShrineStack(On.RoR2.ShrineBloodBehavior.orig_AddShrineStack orig, ShrineBloodBehavior self, Interactor interactor) {
            if (!NetworkServer.active) {
                Debug.LogWarning("[Server] function 'System.Void RoR2.ShrineBloodBehavior::AddShrineStack(RoR2.Interactor)' called on client");
                return;
            }
            self.waitingForRefresh = true;
            self.refreshTimer = 2f;
            if (++self.purchaseCount >= self.maxPurchaseCount) {
                self.symbolTransform.gameObject.SetActive(false);
            }
            EffectManager.SpawnEffect(AssetReferences.shrineUseEffect, new EffectData {
                origin = self.transform.position,
                rotation = Quaternion.identity,
                scale = 1f,
                color = Color.red
            }, true);
            var rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong);
            var random = rng.RangeInt(0, 26);
            PickupIndex pickupIndex;
            if (random < 15) {
                pickupIndex = rng.NextElementUniform(Run.instance.availableTier1DropList);
            } else if (random < 20) {
                pickupIndex = rng.NextElementUniform(Run.instance.availableEquipmentDropList);
            } else if (random < 25) {
                pickupIndex = rng.NextElementUniform(Run.instance.availableTier2DropList);
            } else {
                pickupIndex = rng.NextElementUniform(Run.instance.availableTier3DropList);
            }
            if (pickupIndex == PickupIndex.none) {
                ChatMessage.Send("供奉后什么也没有发生。".ToShrine());
                return;
            }
            PickupDropletController.CreatePickupDroplet(pickupIndex, interactor.transform.position, Vector3.up * 30f);
            var body = interactor.GetComponent<CharacterBody>();
            if (body) {
                var permanentCurseBuffCount = random * (1 + Run.instance.stageClearCount);
                for (int i = 0; i < permanentCurseBuffCount; ++i) {
                    body.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex);
                }
                ChatMessage.Send($"{body.GetColoredUserName()}感觉到一阵灼热的疼痛，获得了奖励和{permanentCurseBuffCount}层诅咒。".ToShrine());
            }
        }

        private void TeleporterInteraction_onTeleporterBeginChargingGlobal(TeleporterInteraction teleporterInteraction) {
            InstanceTracker.GetInstancesList<PurchaseInteraction>()?.ForEach(interaction => {
                if (interaction.TryGetComponent<ShrineBloodBehavior>(out var shrineBlood)) {
                    shrineBlood.maxPurchaseCount = 0;
                    shrineBlood.symbolTransform.gameObject.SetActive(false);
                    interaction.SetAvailable(false);
                }
            });
        }
    }
}