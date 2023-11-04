using BtpTweak.Utils;
using HIFUEngineerTweaks.Skills;
using RoR2;
using RoR2.CharacterAI;
using System;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class EngiTweak : TweakBase<EngiTweak>{

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
        }

        public void Load() {
            Array.Find("RoR2/Base/Engi/EngiTurretMaster.prefab".LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 2;
            "RoR2/Base/Engi/EngiBubbleShield.prefab".LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = BubbleShield.duration * 0.9f;
        }
    }
}