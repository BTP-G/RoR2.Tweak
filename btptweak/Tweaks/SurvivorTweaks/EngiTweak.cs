using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using HIFUEngineerTweaks.Skills;
using RoR2;
using RoR2.CharacterAI;
using System;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class EngiTweak : TweakBase<EngiTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
        }

        public void Load() {
            Array.Find(GameObjectPaths.EngiTurretMaster.LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 2;
            GameObjectPaths.EngiBubbleShield.LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = BubbleShield.duration * 0.9f;
        }
    }
}