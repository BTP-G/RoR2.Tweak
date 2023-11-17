using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class InfusionTweak : TweakBase<InfusionTweak> {
        public const float 基础生命值占比 = 0.1f;

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Infusion")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 17);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate((int itemCount, DamageReport damageReport, CharacterBody attackerBody, Inventory inventory, Vector3 pos) => {
                    int teamItemCount = Util.GetItemCountForTeam(damageReport.attackerTeamIndex, RoR2Content.Items.Infusion.itemIndex, true, false);
                    if (teamItemCount > 0) {
                        if (inventory.infusionBonus < (uint)(attackerBody.level * attackerBody.baseMaxHealth * 基础生命值占比 * teamItemCount)) {
                            InfusionOrb infusionOrb = new() {
                                origin = pos,
                                target = attackerBody.mainHurtBox,
                                maxHpValue = teamItemCount
                            };
                            OrbManager.instance.AddOrb(infusionOrb);
                        }
                        var ownerBody = damageReport.attackerOwnerMaster?.GetBody();
                        if (ownerBody?.mainHurtBox && ownerBody.inventory.infusionBonus < (uint)(ownerBody.level * ownerBody.levelMaxHealth * teamItemCount)) {
                            InfusionOrb infusionOrb = new() {
                                origin = pos,
                                target = ownerBody.mainHurtBox,
                                maxHpValue = teamItemCount
                            };
                            OrbManager.instance.AddOrb(infusionOrb);
                        }
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Infusion :: Hook Failed!");
            }
        }
    }
}