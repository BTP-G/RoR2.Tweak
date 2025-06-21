using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BarrierOnKillTweak : ModComponent, IModLoadMessageHandler {
        public const float AddBarrierFraction = 0.0075f;

        public void Handle() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                                   x => x.MatchLdsfld(typeof(RoR2Content.Items), "BarrierOnKill"),
                                   x => x.MatchCallvirt<Inventory>("GetItemCount"))
                && cursor.TryGotoNext(c => c.MatchCallvirt<HealthComponent>("AddBarrier"))) {
                cursor.Emit(OpCodes.Ldloc, 15)
                      .Emit(OpCodes.Ldloc, 49)
                      .EmitDelegate((CharacterBody attackerBody, int BarrierOnKillCount) => {
                          return AddBarrierFraction * attackerBody.maxBarrier * BarrierOnKillCount;
                      });
                cursor.Emit(OpCodes.Add);
            } else {
                "BarrierOnKill :: Hook Failed!".LogError();
            }
        }
    }
}