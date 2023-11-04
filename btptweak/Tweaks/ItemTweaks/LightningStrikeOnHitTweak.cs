using BtpTweak.OrbPools;
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
                        (victimBody.GetComponent<SimpleLightningStrikeOrbPool>() ?? victimBody.gameObject.AddComponent<SimpleLightningStrikeOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                            attacker = damageInfo.attacker,
                            damageColorIndex = DamageColorIndex.Item,
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 3 * itemCount),
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.5f,
                            target = victimBody.mainHurtBox,
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("LightningStrikeOnHit :: Hook Failed!");
            }
        }
    }
}