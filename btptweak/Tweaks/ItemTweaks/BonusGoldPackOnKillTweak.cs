using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BonusGoldPackOnKillTweak : TweakBase<BonusGoldPackOnKillTweak>, IOnModLoadBehavior {
        public const int DropPercentChance = 5;
        public const int StackMoney = 5;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BonusGoldPackOnKill")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldloc, 15)
                        .Emit(OpCodes.Ldloc, 18)
                        .Emit(OpCodes.Ldloc, 6)
                        .EmitDelegate((int itemCount, CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) => {
                            if (itemCount > 0 && Util.CheckRoll(DropPercentChance * itemCount, attacterBody.master)) {
                                var bonusMoneyPack = Object.Instantiate(AssetReferences.bonusMoneyPack, pos, Random.rotation);
                                var gravitatePickup = bonusMoneyPack.GetComponentInChildren<GravitatePickup>();
                                if (gravitatePickup) {
                                    gravitatePickup.gravitateTarget = attacterBody.coreTransform;
                                    gravitatePickup.teamFilter.teamIndex = attacterTeamindex;
                                }
                                var moneyPickup = bonusMoneyPack.GetComponentInChildren<MoneyPickup>();
                                if (moneyPickup) {
                                    moneyPickup.goldReward += Run.instance.GetDifficultyScaledCost(StackMoney * itemCount);
                                }
                                NetworkServer.Spawn(bonusMoneyPack);
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BonusGoldPackOnKill Hook Failed!");
            }
        }
    }
}