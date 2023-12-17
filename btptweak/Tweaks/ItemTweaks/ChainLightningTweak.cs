using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningTweak : TweakBase<ChainLightningTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 0.6f;
        public const float 半数 = 4;
        public const int BasePercentChance = 20;
        public const int BaseRadius = 18;
        public const int Bounces = 2;
        public const int StackRadius = 3;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("ChainLightning")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) => {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                        var simpleOrbInfo = new SimpleOrbInfo {
                            attacker = damageInfo.attacker,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                        };
                        simpleOrbInfo.procChainMask.AddGreenProcs();
                        (victim.GetComponent<LightningOrbPool>()
                        ?? victim.AddComponent<LightningOrbPool>()).AddOrb(simpleOrbInfo,
                                                                           Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                                                                           itemCount,
                                                                           attackerMaster.teamIndex);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightning :: Hook Failed!");
            }
        }
    }
}