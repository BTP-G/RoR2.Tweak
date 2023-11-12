using BtpTweak.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningTweak : TweakBase<ChainLightningTweak> {
        public const int BasePercentChance = 25;
        public const int StackPercentChance = 5;
        public const float BaseDamageCoefficient = 0.6f;
        public const int BaseRadius = 18;
        public const int StackRadius = 3;
        public const int Bounces = 3;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
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
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && Util.CheckRoll((BasePercentChance + StackPercentChance * (itemCount - 1)) * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<LightningOrbPool>() ?? victim.AddComponent<LightningOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient),
                            isCrit = damageInfo.crit,
                            bouncesRemaining = Bounces,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            bouncedObjects = new List<HealthComponent> { victim.GetComponent<HealthComponent>() },
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.2f,
                            lightningType = LightningOrb.LightningType.Ukulele,
                            damageColorIndex = DamageColorIndex.Item,
                            range = BaseRadius + StackRadius * (itemCount - 1)
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightning :: Hook Failed!");
            }
        }
    }
}