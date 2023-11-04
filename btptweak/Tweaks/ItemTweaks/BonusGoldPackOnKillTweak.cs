using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BonusGoldPackOnKillTweak : TweakBase<BonusGoldPackOnKillTweak> {

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => x.MatchLdstr("Prefabs/NetworkedObjects/BonusMoneyPack"))) {
                ilcursor.RemoveRange(15);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 18);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate((CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) => {
                    GameObject BonusMoneyPack = Object.Instantiate(AssetReferences.bonusMoneyPack, pos, Random.rotation);
                    TeamFilter TeamFilter = BonusMoneyPack.GetComponent<TeamFilter>();
                    if (TeamFilter) {
                        TeamFilter.teamIndex = attacterTeamindex;
                        BonusMoneyPack.GetComponentInChildren<GravitatePickup>().gravitateTarget = attacterBody.coreTransform;
                    }
                    NetworkServer.Spawn(BonusMoneyPack);
                });
            } else {
                Main.Logger.LogError("BonusGoldPackOnKill Hook Failed!");
            }
        }
    }
}