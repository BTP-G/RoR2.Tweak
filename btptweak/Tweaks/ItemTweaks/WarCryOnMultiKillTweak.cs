using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class WarCryOnMultiKillTweak : TweakBase<WarCryOnMultiKillTweak>, IOnModLoadBehavior {
        public const int MaxBuffCount = 3;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.AddMultiKill += CharacterBody_AddMultiKill;
        }

        [Server]
        private void CharacterBody_AddMultiKill(On.RoR2.CharacterBody.orig_AddMultiKill orig, CharacterBody self, int kills) {
            if (!NetworkServer.active) {
                Debug.LogWarning("[Server] function 'System.Void RoR2.CharacterBody::AddMultiKill(System.Int32)' called on client");
                return;
            }
            self.multiKillTimer = 1f;
            self.multiKillCount += kills;
            int itemCount = self.inventory?.GetItemCount(RoR2Content.Items.WarCryOnMultiKill) ?? 0;
            if (itemCount > 0 && MaxBuffCount * itemCount >= self.GetBuffCount(RoR2Content.Buffs.WarCryBuff.buffIndex)) {
                self.AddTimedBuff(RoR2Content.Buffs.WarCryBuff.buffIndex, 4f);
            }
        }

        private void CharacterBody_RecalculateStats(ILContext il) {
            var c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                              x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "WarCryBuff"),
                              x => x.MatchCall<CharacterBody>("HasBuff"))) {
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("WarCryBuff Hook Failed!");
            }
        }
    }
}