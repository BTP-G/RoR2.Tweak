using BtpTweak.MissilePools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileTweak : TweakBase<MissileTweak> {
        public const float BasePercnetChance = 10f;
        public const int DamageCoefficient = 2;

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Missile")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, GameObject victim, CharacterMaster attackerMaster) => {
                    if (itemCount > 0 && Util.CheckRoll(100f * (itemCount / (itemCount + 3f)) * damageInfo.procCoefficient, attackerMaster)) {
                        (damageInfo.attacker.GetComponent<AtgMissileMK_1Pool>() ?? damageInfo.attacker.AddComponent<AtgMissileMK_1Pool>()).AddMissile(
                            Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                            damageInfo.crit,
                            victim,
                            damageInfo.procChainMask);
                    }
                    int fireworkCount = attackerMaster.inventory.GetItemCount(RoR2Content.Items.Firework);
                    if (fireworkCount > 0 && Util.CheckRoll(FireworkTweak.PercentChance * fireworkCount * damageInfo.procCoefficient, attackerMaster)) {
                        (damageInfo.attacker.GetComponent<FireworkPool>() ?? damageInfo.attacker.AddComponent<FireworkPool>()).AddMissile(
                            Util.OnHitProcDamage(damageInfo.damage, 0, FireworkTweak.BaseDamageCoefficent),
                            damageInfo.crit,
                            victim,
                            damageInfo.procChainMask);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Missile & Firework :: FireHook Failed!");
            }
        }
    }
}