using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Buffs {
    internal class VoidFire : TweakBase<VoidFire>, IOnModLoadBehavior {
        public static BuffDef BuffDef { get; private set; }
        void IOnModLoadBehavior.OnModLoad() {
            BuffDef = ScriptableObject.CreateInstance<BuffDef>();
            BuffDef.name = "Void Fire";
            BuffDef.iconSprite = Texture2DPaths.texBuffVoidFog.Load<Sprite>();
            BuffDef.buffColor = Color.red;
            BuffDef.canStack = false;
            BuffDef.isHidden = true;
            BuffDef.isDebuff = false;
            BuffDef.isCooldown = false;
            if (!ContentAddition.AddBuffDef(BuffDef)) {
                ("Buff '" + BuffDef.name + "' failed to be added!").LogError();
            }
        }
    }
}
