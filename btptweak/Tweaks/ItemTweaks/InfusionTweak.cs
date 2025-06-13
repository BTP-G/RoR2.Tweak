using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class InfusionTweak : TweakBase<InfusionTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float 基础生命值占比 = 0.1f;

        public void OnModLoad() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
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
                            int teamItemCount = Util.GetItemCountForTeam(damageReport.attackerTeamIndex, RoR2Content.Items.Infusion.itemIndex, true, false);
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