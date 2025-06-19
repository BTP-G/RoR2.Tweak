using RoR2;
using RoR2.Items;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class IcicleTweak : TweakBase<IcicleTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float DamageCoefficient = 3f;
        public const float DamageCoefficientPerIcicle = 1f;
        public const float IcicleDuration = 3;
        public const float BaseRadius = 6;
        public const float RadiusPerIcicle = 3;
        public const int BaseIcicleMin = 1;
        public const int StacIcicleMin = 1;
        public const int BaseIcicleMax = 6;
        public const int StackicicleMax = 3;

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.Items.IcicleBodyBehavior.OnEnable += IcicleBodyBehavior_OnEnable;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var icicleAuraController = IcicleBodyBehavior.icicleAuraPrefab.GetComponent<IcicleAuraController>();
            icicleAuraController.icicleDuration = IcicleDuration;  // 1.5
            icicleAuraController.icicleDamageCoefficientPerTick = DamageCoefficient * icicleAuraController.baseIcicleAttackInterval;  // 3
            icicleAuraController.icicleDamageCoefficientPerTickPerIcicle = DamageCoefficientPerIcicle * icicleAuraController.baseIcicleAttackInterval;  // 0
            icicleAuraController.baseIcicleMax = BaseIcicleMax;  //  6
            icicleAuraController.icicleMaxPerStack = StackicicleMax;  // 6
            icicleAuraController.icicleCountOnFirstKill = 0;  // 0
            icicleAuraController.icicleBaseRadius = BaseRadius;  // 6
            icicleAuraController.icicleRadiusPerIcicle = RadiusPerIcicle;  // 2
        }

        private void IcicleBodyBehavior_OnEnable(On.RoR2.Items.IcicleBodyBehavior.orig_OnEnable orig, IcicleBodyBehavior self) {
            orig(self);
            self.icicleAura.icicleLifetimes.Add(float.MaxValue);
            self.icicleAura.icicleCountOnFirstKill = BaseIcicleMin + StacIcicleMin * (self.body.inventory.GetItemCount(RoR2Content.Items.Icicle.itemIndex) - 1) - 1;
            void onInventoryChanged() {
                if (!self) {
                    return;
                }
                if (self.icicleAura) {
                    self.icicleAura.icicleCountOnFirstKill = BaseIcicleMin + StacIcicleMin * (self.body.inventory.GetItemCount(RoR2Content.Items.Icicle.itemIndex) - 1) - 1;
                    return;
                }
                if (self.body) {
                    self.body.onInventoryChanged -= onInventoryChanged;
                }
            }
            self.body.onInventoryChanged += onInventoryChanged;
        }
    }
}