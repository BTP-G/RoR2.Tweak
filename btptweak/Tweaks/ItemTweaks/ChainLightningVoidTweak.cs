using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningVoidTweak : TweakBase<ChainLightningVoidTweak> {
        public const int BasePercentChance = 20;
        public const float 半数 = 4;
        public const float DamageCoefficient = 0.3f;
        public const int TotalStrikes = 3;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("ChainLightningVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && victimBody.mainHurtBox && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                        var voidLightningOrb = new VoidLightningOrb() {
                            origin = damageInfo.position,
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                            isCrit = damageInfo.crit,
                            totalStrikes = TotalStrikes,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.2f,
                            damageColorIndex = DamageColorIndex.Void,
                            secondsPerStrike = 0.1f,
                            target = victimBody.mainHurtBox
                        };
                        voidLightningOrb.procChainMask.AddWhiteProcs();
                        voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                        OrbManager.instance.AddOrb(voidLightningOrb);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
            }
        }
    }
}