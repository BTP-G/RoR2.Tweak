using RoR2;
using TPDespair.ZetArtifacts;

namespace BTP.RoR2Plugin.Tweaks {

    internal class DropifactTweak : TweakBase<DropifactTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            Run.onRunStartGlobal += OnRunStartGlobal;
            CharacterBody.onBodyInventoryChangedGlobal += OnBodyInventoryChangedGlobal;
        }

        private void OnRunStartGlobal(Run obj) {
            ZetArtifactsPlugin.DropifactVoidT1.Value = false;
            ZetArtifactsPlugin.DropifactVoidT2.Value = false;
            ZetArtifactsPlugin.DropifactVoidT3.Value = false;
            ZetArtifactsPlugin.DropifactVoid.Value = false;
            ZetArtifactsPlugin.DropifactLunar.Value = false;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = false;
        }

        private void OnBodyInventoryChangedGlobal(CharacterBody body) {
            var isLunar = body.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex);
            var isVoid = body.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex);
            ZetArtifactsPlugin.DropifactVoidT1.Value = isVoid;
            ZetArtifactsPlugin.DropifactVoidT2.Value = isVoid;
            ZetArtifactsPlugin.DropifactVoidT3.Value = isVoid;
            ZetArtifactsPlugin.DropifactVoid.Value = isVoid;
            ZetArtifactsPlugin.DropifactLunar.Value = isLunar;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = isLunar && isVoid;
        }
    }
}