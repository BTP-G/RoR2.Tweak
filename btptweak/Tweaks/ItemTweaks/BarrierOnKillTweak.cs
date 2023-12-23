using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BarrierOnKillTweak : TweakBase<BarrierOnKillTweak>, IOnModLoadBehavior {
        public const float AddBarrierFraction = 0.01f;

        public void OnModLoad() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                                   x => x.MatchLdsfld(typeof(RoR2Content.Items), "BarrierOnKill"),
                                   x => x.MatchCallvirt<Inventory>("GetItemCount"))
                && cursor.TryGotoNext(c => c.MatchCallvirt<HealthComponent>("AddBarrier"))) {
                cursor.Emit(OpCodes.Ldloc, 15);
                cursor.Emit(OpCodes.Ldloc, 49);
                cursor.EmitDelegate((CharacterBody attackerBody, int BarrierOnKillCount) => {
                    return AddBarrierFraction * attackerBody.maxBarrier * BarrierOnKillCount;
                });
                cursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("BarrierOnKill :: Hook Failed!");
            }
        }
    }
}