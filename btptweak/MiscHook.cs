using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {
        public static BodyIndex bandit2Bodyindex;
        public static bool 往日不再 = false;
        public static int 造物难度敌人珍珠 = 0;
        public static int 古代权杖掉落数 = 0;
        public static int 战斗祭坛物品掉落数 = 0;

        public static void AddHook() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += IL_MissileVoidOrb_Begin;
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.PickupDropletController.CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3 += PickupDropletController_CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection += SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer += ShrineCombatBehavior_OnDefeatedServer;
        }

        public static void RemoveHook() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= IL_MissileVoidOrb_Begin;
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.PickupDropletController.CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3 -= PickupDropletController_CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection -= SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer -= ShrineCombatBehavior_OnDefeatedServer;
        }

        private static void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body) {
            orig(self, body);
            if (NetworkServer.active) {
                if (self.teamIndex == TeamIndex.Player) {
                    if (body.isPlayerControlled && body.bodyIndex == bandit2Bodyindex) {
                        if (SkillHook.盗贼标记_.ContainsKey(body.playerControllerId)) {
                            for (int i = SkillHook.盗贼标记_[body.playerControllerId]; i > 0; --i) {
                                body.AddBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                            }
                        } else {
                            SkillHook.盗贼标记_.Add(body.playerControllerId, 0);
                        }
                    }
                } else if (BtpTweak.是否选择造物难度_) {
                    self.inventory.GiveItem(RoR2Content.Items.Pearl.itemIndex, 造物难度敌人珍珠);
                }
            }
        }

        private static void ShrineCombatBehavior_OnDefeatedServer(On.RoR2.ShrineCombatBehavior.orig_OnDefeatedServer orig, ShrineCombatBehavior self) {
            orig(self);
            if (NetworkServer.active && 战斗祭坛物品掉落数 > 0) {
                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
                Vector3 pos = self.transform.position + (6 * Vector3.up);
                Vector3 velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / 战斗祭坛物品掉落数, Vector3.up);
                for (int i = 0; i < 战斗祭坛物品掉落数; ++i) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
                战斗祭坛物品掉落数 /= 2;
            }
        }

        private static void PickupDropletController_CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3(On.RoR2.PickupDropletController.orig_CreatePickupDroplet_CreatePickupInfo_Vector3_Vector3 orig, GenericPickupController.CreatePickupInfo pickupInfo, Vector3 position, Vector3 velocity) {
            if (古代权杖掉落数 > 0) {
                if (pickupInfo.pickupIndex == PickupCatalog.FindPickupIndex(ItemTier.Tier3)) {
                    pickupInfo.pickerOptions[0].pickupIndex = PickupCatalog.FindPickupIndex(AncientScepter.AncientScepterItem.instance.ItemDef.itemIndex);
                    --古代权杖掉落数;
                }
            }
            orig(pickupInfo, position, velocity);
        }

        private static WeightedSelection<DirectorCard> SceneDirector_GenerateInteractableCardSelection(On.RoR2.SceneDirector.orig_GenerateInteractableCardSelection orig, SceneDirector self) {
            if (NetworkServer.active && BtpTweak.是否选择造物难度_) {
                self.onPopulateCreditMultiplier += 0.25f * Run.instance.participatingPlayerCount;
            }
            return orig(self);
        }

        private static void IL_MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 75f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 150f);
            } else {
                BtpTweak.logger_.LogError("MissileVoidOrb Hook Error");
            }
        }
    }
}