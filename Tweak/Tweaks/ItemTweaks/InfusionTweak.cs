using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class InfusionTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float 基础生命值占比 = 0.2f;

        public void Handle() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            RoR2Content.Items.Infusion.TryApplyTag(ItemTag.CannotCopy);
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Infusion")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Pop)
                        .Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc, 15)
                        .EmitDelegate((DamageReport damageReport, CharacterBody attackerBody) => {
                            var teamItemCount = Util.GetItemCountForTeam(damageReport.attackerTeamIndex, RoR2Content.Items.Infusion.itemIndex, true, false);
                            if (teamItemCount > 0) {
                                if (attackerBody.mainHurtBox && attackerBody.inventory.infusionBonus < (uint)(attackerBody.level * attackerBody.baseMaxHealth * 基础生命值占比 * teamItemCount)) {
                                    var infusionOrb = new InfusionOrb() {
                                        origin = damageReport.damageInfo.position,
                                        target = attackerBody.mainHurtBox,
                                        maxHpValue = teamItemCount,
                                    };
                                    OrbManager.instance.AddOrb(infusionOrb);
                                }
                                var ownerBody = damageReport.attackerOwnerMaster?.GetBody();
                                if (ownerBody && ownerBody.mainHurtBox && ownerBody.inventory.infusionBonus < (uint)(ownerBody.level * ownerBody.baseMaxHealth * 基础生命值占比 * teamItemCount)) {
                                    var infusionOrb = new InfusionOrb() {
                                        origin = damageReport.damageInfo.position,
                                        target = ownerBody.mainHurtBox,
                                        maxHpValue = teamItemCount,
                                    };
                                    OrbManager.instance.AddOrb(infusionOrb);
                                }
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("Infusion :: Hook Failed!");
            }
        }
    }
}