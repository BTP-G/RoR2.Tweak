using BtpTweak.Utils;
using HIFUEngineerTweaks.Skills;
using RoR2.CharacterAI;
using System;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class EngiTweak : TweakBase {

        public override void Load() {
            base.Load();
            Array.Find("RoR2/Base/Engi/EngiTurretMaster.prefab".LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 2;
            "RoR2/Base/Engi/EngiBubbleShield.prefab".LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = BubbleShield.duration * 0.9f;
        }
    }
}