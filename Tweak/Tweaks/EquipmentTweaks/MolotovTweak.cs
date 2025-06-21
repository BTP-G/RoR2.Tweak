using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class MolotovTweak : ModComponent, IModLoadMessageHandler {
        public const float DamageBonusCoefficient = 0.1666f;

        void IModLoadMessageHandler.Handle() {
            On.RoR2.EquipmentSlot.FireMolotov += EquipmentSlot_FireMolotov;
        }

        private bool EquipmentSlot_FireMolotov(On.RoR2.EquipmentSlot.orig_FireMolotov orig, EquipmentSlot self) {
            var aimRay = self.GetAimRay();
            ProjectileManager.instance.FireProjectile(AssetReferences.molotovClusterProjectile,
                aimRay.origin,
                Quaternion.LookRotation(aimRay.direction),
                self.gameObject,
                self.characterBody.damage * (0.1666f * self.inventory.GetItemCount(RoR2Content.Items.IgniteOnKill.itemIndex) + 1),
                0f,
                self.characterBody.RollCrit());
            return true;
        }
    }
}