using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {
        public static int 造物难度敌人珍珠_ = 0;
        public static int 虚空潜能古代权杖掉落数_ = 0;
        public static int 战斗祭坛物品掉落数_ = 0;
        public static int bandit2Count = 0;

        public static void AddHook() {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            PickupDropletController.onDropletHitGroundServer += PickupDropletController_onDropletHitGroundServer;
            ShrineCombatBehavior.onDefeatedServerGlobal += ShrineCombatBehavior_onDefeatedServerGlobal;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            IL.RoR2.Orbs.MissileVoidOrb.Begin += IL_MissileVoidOrb_Begin;
        }

        public static void RemoveHook() {
            CharacterBody.onBodyStartGlobal -= CharacterBody_onBodyStartGlobal;
            PickupDropletController.onDropletHitGroundServer -= PickupDropletController_onDropletHitGroundServer;
            ShrineCombatBehavior.onDefeatedServerGlobal -= ShrineCombatBehavior_onDefeatedServerGlobal;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState_OnEnter;
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= IL_MissileVoidOrb_Begin;
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active) {
                if (body.teamComponent.teamIndex == TeamIndex.Player) {
                    if (body.isPlayerControlled && body.name.StartsWith("Bandit2")) {
                        if (SkillHook.盗贼标记_.TryGetValue(body.inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex), out int buffCount)) {
                            body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, buffCount);
                        } else {
                            ++bandit2Count;
                            body.inventory.GiveItem(JunkContent.Items.SkullCounter.itemIndex, bandit2Count);
                            SkillHook.盗贼标记_.Add(bandit2Count, 0);
                        }
                    } else if (body.bodyIndex == GlobalEventHook.工程师固定炮台_ || body.bodyIndex == GlobalEventHook.工程师移动炮台_) {
                        body.inventory.infusionBonus = body.masterObject.GetComponent<Deployable>().ownerMaster.inventory.infusionBonus;
                    }
                    if (FinalBossHook.处于天文馆_) {
                        body.AddBuff(BuffCatalog.FindBuffIndex("ZetWarped"));
                    }
                } else if (Main.是否选择造物难度_ && body.inventory) {
                    if (body.bodyIndex == HealthHook.虚灵_) {
                        body.inventory.GiveItem(DLC1Content.Items.BearVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.BleedOnHitVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.ChainLightningVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.ExplodeOnDeathVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.MissileVoid.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.MoreMissile.itemIndex);
                        body.inventory.GiveItem(DLC1Content.Items.SlowOnHitVoid.itemIndex);
                    } else if (!body.isBoss && Util.CheckRoll(1)) {
                        int random = UnityEngine.Random.Range(0, PlayerCharacterMasterController.instances.Count - 1);
                        body.inventory.CopyItemsFrom(PlayerCharacterMasterController.instances[random].master.inventory);
                        body.hasOneShotProtection = true;
                        ChatMessage.SendColored("天神下凡！", Color.red);
                    }
                    body.inventory.GiveItem(RoR2Content.Items.Pearl.itemIndex, 造物难度敌人珍珠_);
                }
            }
        }

        private static void ShrineCombatBehavior_onDefeatedServerGlobal(ShrineCombatBehavior shrine) {
            if (战斗祭坛物品掉落数_ > 0) {
                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
                Vector3 pos = shrine.transform.position + (6 * Vector3.up);
                Vector3 velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / 战斗祭坛物品掉落数_, Vector3.up);
                for (int i = 0; i < 战斗祭坛物品掉落数_; ++i) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
                战斗祭坛物品掉落数_ /= 2;
            }
        }

        private static void PickupDropletController_onDropletHitGroundServer(ref GenericPickupController.CreatePickupInfo createPickupInfo, ref bool shouldSpawn) {
            if (虚空潜能古代权杖掉落数_ > 0) {
                if (createPickupInfo.pickupIndex == PickupCatalog.FindPickupIndex(ItemTier.Tier3)) {
                    PickupIndex ancientScepter = PickupCatalog.FindPickupIndex(AncientScepter.AncientScepterItem.instance.ItemDef.itemIndex);
                    for (int i = 0; i < createPickupInfo.pickerOptions.Length; i++) {
                        if (createPickupInfo.pickerOptions[i].pickupIndex == ancientScepter) {
                            --虚空潜能古代权杖掉落数_;
                            return;
                        }
                    }
                    createPickupInfo.pickerOptions[0].pickupIndex = ancientScepter;
                    --虚空潜能古代权杖掉落数_;
                }
            }
        }

        private static void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (Main.是否选择造物难度_ && RunHook.往日不再_ == false) {
                if (NetworkServer.active) {
                    ChatMessage.SendColored("--世界不再是你熟悉的那样！！！", Color.red);
                }
                RunHook.往日不再_ = true;
            }
        }

        private static void IL_MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 75f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 150f);
            } else {
                Main.logger_.LogError("MissileVoidOrb Hook Error");
            }
        }
    }
}