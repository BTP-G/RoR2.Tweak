using BtpTweak.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ThornsTweak : TweakBase<ThornsTweak>, IOnModLoadBehavior {
        public const int BaseRadius = 25;
        public const int StackRadius = 25;
        public const float BaseDamageCoefficient = 2.5f;
        public const float StackDamageCoefficient = 2.5f;
        public const float Interval = 0.1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   c => c.MatchRet(),
                                   c => c.MatchLdloc(6),
                                   c => c.MatchLdcR4(0f),
                                   c => c.Match(OpCodes.Ble_Un))) {
                cursor.Index += 2;
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldloc, 11);
                cursor.EmitDelegate((float damageDealt, HealthComponent healthComponent, DamageReport damageReport) => {
                    if (healthComponent.itemCounts.thorns > 0 && damageDealt > 0) {
                        var damageInfo = damageReport.damageInfo;
                        if (!damageInfo.procChainMask.HasProc(ProcType.Thorns)) {
                            var simpleOrbInfo = new OrbPoolKey {
                                attackerBody = healthComponent.body,
                                target = damageReport.attackerBody?.mainHurtBox,
                                procChainMask = damageInfo.procChainMask,
                                isCrit = damageInfo.crit,
                                通用浮点数 = healthComponent.itemCounts.thorns - 1f,
                            };
                            ThornsOrbPool.RentPool(healthComponent.gameObject).AddOrb(simpleOrbInfo, damageReport);
                        }
                    }
                });
                cursor.Emit(OpCodes.Ldc_R4, 0f);
            } else {
                Main.Logger.LogError("Thorns :: Hook Failed!");
            }
        }
    }
}