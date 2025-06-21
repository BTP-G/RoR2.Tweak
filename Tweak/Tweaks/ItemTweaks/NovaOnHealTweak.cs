using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class NovaOnHealTweak : TweakBase<NovaOnHealTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BaseDamageCoefficient = 1;
        public const float Interval = 0.1f;
        public const float BaseRadius = 66.6f;
        public const float BaseThresholdFraction = 0.1f;
        public const float 半数 = 9f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Items.NovaOnHeal.TryApplyTag(ItemTag.AIBlacklist);
        }

        private void HealthComponent_ServerFixedUpdate(ILContext il) {
            var iLCursor = new ILCursor(il);
            if (iLCursor.TryGotoNext(MoveType.Before,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdfld<HealthComponent>("devilOrbTimer"),
                                     x => x.MatchLdcR4(0f))) {
                iLCursor.Index += 2;
                iLCursor.Emit(OpCodes.Ldarg_0)
                        .EmitDelegate((float devilOrbTimer, HealthComponent healthComponent) => {
                            if (devilOrbTimer <= 0) {
                                healthComponent.devilOrbTimer = Interval;
                                var novaOnHealCount = healthComponent.itemCounts.novaOnHeal;
                                var damageValue = Util2.CloseTo(novaOnHealCount > 0 ? novaOnHealCount : 1, 9f, healthComponent.fullCombinedHealth);
                                if (healthComponent.devilOrbHealPool >= damageValue) {
                                    var body = healthComponent.body;
                                    var devilOrb = new DevilOrb {
                                        attacker = healthComponent.gameObject,
                                        damageColorIndex = DamageColorIndex.Poison,
                                        damageValue = damageValue,
                                        effectType = DevilOrb.EffectType.Skull,
                                        origin = body.aimOrigin,
                                        procChainMask = default,
                                        procCoefficient = 0.5f,
                                        scale = 1,
                                        teamIndex = body.teamComponent.teamIndex,
                                    };
                                    if (devilOrb.target = devilOrb.PickNextTarget(devilOrb.origin, BaseRadius)) {
                                        devilOrb.procChainMask.AddProc(ProcType.HealNova);
                                        devilOrb.isCrit = body.RollCrit();
                                        OrbManager.instance.AddOrb(devilOrb);
                                        healthComponent.devilOrbHealPool -= damageValue;
                                    }
                                }
                            }
                        });
                iLCursor.Emit(OpCodes.Ldc_R4, 1f);
            } else {
                LogExtensions.LogError("NovaOnHeal Hook Failed!");
            }
        }
    }
}