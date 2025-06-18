using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Buffs {

    internal class DroneCommanderSpawnCooldown : TweakBase<VoidFire>, IOnModLoadBehavior {
        public static BuffDef BuffDef { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            BuffDef = ScriptableObject.CreateInstance<BuffDef>();
            BuffDef.name = "DroneCommander SpawnCooldown";
            BuffDef.iconSprite = Texture2DPaths.texDroneWeaponsIcon.Load<Sprite>();
            //DroneCommanderSpawnCooldown.buffColor = Color.gray;
            BuffDef.canStack = false;
            BuffDef.isHidden = false;
            BuffDef.isDebuff = false;
            BuffDef.isCooldown = true;
            if (!ContentAddition.AddBuffDef(BuffDef)) {
                ("Buff '" + BuffDef.name + "' failed to be added!").LogError();
            }
        }
    }
}