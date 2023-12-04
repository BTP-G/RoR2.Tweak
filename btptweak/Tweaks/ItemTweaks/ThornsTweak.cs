using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ThornsTweak : TweakBase<ThornsTweak> {
        public const int Radius = 10;
        public const float BaseDamageCoefficient = 1.5f;

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
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
                            var simpleOrbInfo = new SimpleOrbInfo {
                                attacker = healthComponent.gameObject,
                                target = damageReport.attackerBody?.mainHurtBox,
                                procChainMask = damageInfo.procChainMask,
                                isCrit = damageInfo.crit,
                            };
                            (healthComponent.GetComponent<ThornsOrbPool>()
                            ?? healthComponent.AddComponent<ThornsOrbPool>()).AddOrb(simpleOrbInfo, damageReport);
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