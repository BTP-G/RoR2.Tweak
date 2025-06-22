using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal class SceneDirectorTweak : ModComponent, IModLoadMessageHandler {
        private int choiceIndex;

        void IModLoadMessageHandler.Handle() {
            SceneDirector.onGenerateInteractableCardSelection += SceneDirector_onGenerateInteractableCardSelection;
            On.RoR2.SceneDirector.SelectCard += SceneDirector_SelectCard;
        }

        private void SceneDirector_onGenerateInteractableCardSelection(SceneDirector arg1, DirectorCardCategorySelection arg2) {
            choiceIndex = 0;
        }

        private DirectorCard SceneDirector_SelectCard(On.RoR2.SceneDirector.orig_SelectCard orig, SceneDirector self, WeightedSelection<DirectorCard> deck, int maxCost) {
            if (RunInfo.已选择大旋风难度 && choiceIndex < deck.Count) {
                var card = deck.GetChoice(choiceIndex++).value;
                if (card.cost > maxCost) {
                    self.interactableCredit += card.cost;
                }
                return card;
            }
            return orig(self, deck, maxCost);
        }
    }
}