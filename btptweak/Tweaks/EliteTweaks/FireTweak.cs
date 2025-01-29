using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using TPDespair.ZetAspects;

namespace BtpTweak.Tweaks.EliteTweaks {

    internal class FireTweak : TweakBase<FireTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 0.2f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
            BetterEvents.OnHitEnemy += BetterEvents_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var curosr = new ILCursor(il);
            curosr.GotoNext(i => i.MatchStloc(32));
            curosr.Emit(OpCodes.Ldarg_1)
                  .Emit(OpCodes.Ldloc_1)
                  .EmitDelegate((DamageInfo damageInfo, CharacterBody attackerBody) => damageInfo.crit ? attackerBody.critMultiplier : 1f);
            curosr.Emit(OpCodes.Mul);
        }

        private void BetterEvents_OnHitEnemy(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) {
            if (!attackerBody.TryGetAspectStackMagnitude(RoR2Content.Buffs.AffixRed.buffIndex, out var stack)) {
                return;
            }
            var damageCoefficient = (Configuration.AspectRedBaseBurnDamage.Value + Configuration.AspectRedStackBurnDamage.Value * (stack - 1f));
            var dotInfo = new InflictDotInfo {
                attackerObject = attackerBody.gameObject,
                damageMultiplier = damageCoefficient * stack,
                dotIndex = DotController.DotIndex.Burn,
                duration = Configuration.AspectRedBurnDuration.Value,
                victimObject = victimBody.gameObject,
                totalDamage = Configuration.AspectRedUseBase.Value
                    ? attackerBody.damage * damageCoefficient
                    : Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient * (damageInfo.crit ? attackerBody.critMultiplier : 1f)),
            };
            if (attackerBody.teamComponent.teamIndex != TeamIndex.Player) {
                dotInfo.totalDamage *= Configuration.AspectRedMonsterDamageMult.Value;
            }
            StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.inventory, ref dotInfo);
            DotController.InflictDot(ref dotInfo);
        }
    }
}