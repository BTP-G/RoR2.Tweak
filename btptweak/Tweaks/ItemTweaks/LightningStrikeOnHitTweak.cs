using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class LightningStrikeOnHitTweak : TweakBase<LightningStrikeOnHitTweak> {
        public const int BasePercentChance = 10;
        public const int DamageCoefficient = 3;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("LightningStrikeOnHit")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit) && victimBody.mainHurtBox && Util.CheckRoll(100f * (itemCount / (itemCount + 9f)) * damageInfo.procCoefficient, attackerMaster)) {
                        var simpleOrbInfo = new SimpleOrbInfo {
                            attacker = damageInfo.attacker,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                        };
                        simpleOrbInfo.procChainMask.AddYellowProcs();
                        (victimBody.GetComponent<SimpleLightningStrikeOrbPool>()
                        ?? victimBody.AddComponent<SimpleLightningStrikeOrbPool>()).AddOrb(simpleOrbInfo,
                                                                                           DamageCoefficient * damageInfo.damage * itemCount);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("LightningStrikeOnHit :: Hook Failed!");
            }
        }
    }
}