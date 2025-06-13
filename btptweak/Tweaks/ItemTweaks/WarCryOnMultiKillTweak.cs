using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class WarCryOnMultiKillTweak : TweakBase<WarCryOnMultiKillTweak>, IOnModLoadBehavior {
        public const int BaseMaxBuffCount = 3;
        public const int StackMaxBuffCount = 2;
        public const float AttackSpeedMultAddPerBuff = 0.1f;
        public const float MoveSpeedMultAddPerBuff = 0.1f;
        public const float BuffDuration = 4f;

        void IOnModLoadBehavior.OnModLoad() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.AddMultiKill += CharacterBody_AddMultiKill;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            var warCryBuffCount = sender.GetBuffCount(RoR2Content.Buffs.WarCryBuff.buffIndex);
            args.attackSpeedMultAdd += AttackSpeedMultAddPerBuff * warCryBuffCount;
            args.moveSpeedMultAdd += MoveSpeedMultAddPerBuff * warCryBuffCount;
        }

        [Server]
        private void CharacterBody_AddMultiKill(On.RoR2.CharacterBody.orig_AddMultiKill orig, CharacterBody self, int kills) {
            if (!NetworkServer.active) {
                Debug.LogWarning("[Server] function 'System.Void RoR2.CharacterBody::AddMultiKill(System.Int32)' called on client");
                return;
            }
            self.multiKillTimer = 1f;
            self.multiKillCount += kills;
            if (!self.inventory) {
                return;
            }
            var itemCount = self.inventory.GetItemCount(RoR2Content.Items.WarCryOnMultiKill);
            if (itemCount > 0) {
                self.AddTimedBuff(RoR2Content.Buffs.WarCryBuff, BuffDuration, BaseMaxBuffCount * itemCount);
            }
        }

        private void CharacterBody_RecalculateStats(ILContext il) {
            var c = new ILCursor(il);
            c.GotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "WarCryBuff"), x => x.MatchCall<CharacterBody>("HasBuff"));
            c.RemoveRange(4);
            c.GotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "WarCryBuff"), x => x.MatchCall<CharacterBody>("HasBuff"));
            c.RemoveRange(4);
        }
    }
}