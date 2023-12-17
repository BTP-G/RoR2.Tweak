using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class NovaOnHealTweak : TweakBase<NovaOnHealTweak>, IOnModLoadBehavior {
        public const float BaseDamageCoefficient = 1;
        public const float Interval = 0.1f;
        public const float BaseRadius = 66.6f;
        public const float BaseThresholdFraction = 0.1f;
        public const float 半数 = 9f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        private void HealthComponent_ServerFixedUpdate(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(MoveType.Before,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdfld<HealthComponent>("devilOrbTimer"),
                                     x => x.MatchLdcR4(0f))) {
                iLCursor.Index += 2;
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.EmitDelegate((float devilOrbTimer, HealthComponent healthComponent) => {
                    if (devilOrbTimer <= 0) {
                        healthComponent.devilOrbTimer = Interval;
                        var novaOnHealCount = healthComponent.itemCounts.novaOnHeal;
                        var damageValue = BtpUtils.简单逼近(novaOnHealCount > 0 ? novaOnHealCount : 1, 9f, healthComponent.fullCombinedHealth);
                        if (healthComponent.devilOrbHealPool >= damageValue) {
                            healthComponent.devilOrbHealPool -= damageValue;
                            var body = healthComponent.body;
                            var devilOrb = new DevilOrb {
                                attacker = body.gameObject,
                                damageColorIndex = DamageColorIndex.Poison,
                                damageValue = damageValue,
                                effectType = DevilOrb.EffectType.Skull,
                                origin = body.aimOriginTransform.position,
                                procChainMask = default,
                                procCoefficient = 0.5f,
                                scale = 1,
                                teamIndex = body.teamComponent.teamIndex,
                            };
                            if (devilOrb.target = devilOrb.PickNextTarget(devilOrb.origin, BaseRadius)) {
                                devilOrb.procChainMask.AddProc(ProcType.HealNova);
                                devilOrb.isCrit = body.RollCrit();
                                OrbManager.instance.AddOrb(devilOrb);
                            }
                        }
                    }
                });
                iLCursor.Emit(OpCodes.Ldc_R4, 1f);
            } else {
                Main.Logger.LogError("NovaOnHeal Hook Failed!");
            }
        }
    }
}