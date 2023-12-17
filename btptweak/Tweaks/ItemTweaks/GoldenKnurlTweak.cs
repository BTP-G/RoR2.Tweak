namespace BtpTweak.Tweaks.ItemTweaks {

    internal class GoldenKnurlTweak : TweakBase<GoldenKnurlTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.CharacterMaster.GiveMoney += CharacterMaster_GiveMoney;
        }

        private void CharacterMaster_GiveMoney(On.RoR2.CharacterMaster.orig_GiveMoney orig, RoR2.CharacterMaster self, uint amount) {
            orig(self, amount + (amount / 2 * (uint)self.inventory.GetItemCount(GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef)));
        }
    }
}