using R2API;
using RoR2;

namespace BtpTweak.Tweaks.EquipmentTweaks {

    internal class TeamWarCryTweak : TweakBase<TeamWarCryTweak>, IOnModLoadBehavior {
        public const float 每层战争号角攻速提升系数 = 0.25f;
        public const float 每层战争号角移速提升系数 = 0.25f;

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.EquipmentSlot.FireTeamWarCry += EquipmentSlot_FireTeamWarCry;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private bool EquipmentSlot_FireTeamWarCry(On.RoR2.EquipmentSlot.orig_FireTeamWarCry orig, EquipmentSlot self) {
            foreach (var t in TeamComponent.GetTeamMembers(self.teamComponent.teamIndex)) {
                t.body.AddTimedBuff(RoR2Content.Buffs.TeamWarCry, 7f);
            }
            var effectData = new EffectData {
                origin = self.characterBody.corePosition
            };
            effectData.SetNetworkedObjectReference(self.characterBody.gameObject);
            EffectManager.SpawnEffect(AssetReferences.teamWarCryActivation, effectData, true);
            Util.PlaySound("Play_teamWarCry_activate", self.characterBody.gameObject);
            return true;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args) {
            if (sender.HasBuff(RoR2Content.Buffs.TeamWarCry.buffIndex) && sender.inventory) {
                var a = sender.inventory.GetItemCount(RoR2Content.Items.EnergizedOnEquipmentUse.itemIndex);
                args.attackSpeedMultAdd += 每层战争号角攻速提升系数 * a;
                args.moveSpeedMultAdd += 每层战争号角移速提升系数 * a;
            }
        }
    }
}