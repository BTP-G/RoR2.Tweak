using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExplodeOnDeathTweak : TweakBase<ExplodeOnDeathTweak> {
        public const int DamageCoefficient = 3;
        public const float BaseRadius = 12;

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("ExplodeOnDeath")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_2);
                ilcursor.EmitDelegate((int itemCount, DamageReport damageReport, CharacterBody victimBody) => {
                    if (itemCount > 0) {
                        GameObject explodeOnDeath = Object.Instantiate(GlobalEventManager.CommonAssets.explodeOnDeathPrefab, victimBody.corePosition, Quaternion.identity);
                        explodeOnDeath.GetComponent<TeamFilter>().teamIndex = damageReport.attackerTeamIndex;
                        DelayBlast delayBlast = explodeOnDeath.GetComponent<DelayBlast>();
                        delayBlast.attacker = damageReport.attacker;
                        delayBlast.baseDamage = Util.OnKillProcDamage(damageReport.attackerBody.damage, DamageCoefficient * itemCount);
                        delayBlast.crit = damageReport.attackerBody.RollCrit();
                        delayBlast.position = damageReport.damageInfo.position;
                        delayBlast.procCoefficient = damageReport.damageInfo.procCoefficient;
                        delayBlast.radius = BaseRadius + 1.2f * victimBody.bestFitRadius;
                        NetworkServer.Spawn(explodeOnDeath);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ExplodeOnDeath :: Hook Failed!");
            }
        }
    }
}