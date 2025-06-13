using BTP.RoR2Plugin.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ThornsTweak : TweakBase<ThornsTweak>, IOnModLoadBehavior {
        public const int BaseRadius = 20;
        public const int StackRadius = 10;
        public const float BaseDamageCoefficient = 2f;
        public const float StackDamageCoefficient = 2f;
        public const float Interval = 0.1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After, c => c.MatchLdfld<HealthComponent.ItemCounts>("thorns"))) {
                cursor.GotoNext(MoveType.After, c => c.MatchLdfld<HealthComponent.ItemCounts>("thorns"))
                      .Emit(OpCodes.Ldloc, 12)
                      .EmitDelegate((int itemCount, DamageReport damageReport) => {
                          if (itemCount > 0 && damageReport.damageDealt > 0) {
                              var damageInfo = damageReport.damageInfo;
                              if (!damageInfo.procChainMask.HasProc(ProcType.Thorns)) {
                                  var simpleOrbInfo = new OrbPoolKey {
                                      attackerBody = damageReport.victimBody,
                                      target = damageReport.attackerBody?.mainHurtBox,
                                      procChainMask = damageInfo.procChainMask,
                                      isCrit = damageInfo.crit,
                                      通用浮点数 = itemCount - 1f,
                                  };
                                  ThornsOrbPool.RentPool(damageReport.victim.gameObject).AddOrb(simpleOrbInfo, damageReport);
                              }
                          }
                      });
                cursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("Thorns :: Hook Failed!");
            }
        }
    }
}