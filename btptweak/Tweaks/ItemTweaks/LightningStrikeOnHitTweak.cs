using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Threading.Tasks;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class LightningStrikeOnHitTweak : TweakBase<LightningStrikeOnHitTweak> {
        public const int BasePercentChance = 10;
        public const float 半数 = 9f;
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
                ilcursor.EmitDelegate(async (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                    if (itemCount == 0) {
                        return;
                    }
                    var simpleOrbInfo = default(SimpleOrbInfo);
                    var result = 0f;
                    await Task.Run(() => {
                        if (!damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit) && victimBody.mainHurtBox && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                            simpleOrbInfo = new SimpleOrbInfo {
                                attacker = damageInfo.attacker,
                                target = victimBody.mainHurtBox,
                                isCrit = damageInfo.crit,
                                procChainMask = damageInfo.procChainMask,
                            };
                            simpleOrbInfo.procChainMask.AddYellowProcs();
                            result = Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount);
                        }
                    });
                    if (result > 0f) {
                        (victimBody.GetComponent<SimpleLightningStrikeOrbPool>()
                        ?? victimBody.AddComponent<SimpleLightningStrikeOrbPool>()).AddOrb(simpleOrbInfo, result);
                    }
                });
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("LightningStrikeOnHit :: Hook Failed!");
            }
        }
    }
}