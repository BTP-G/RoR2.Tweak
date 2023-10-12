using BtpTweak.Utils;
using UnityEngine;

namespace BtpTweak.MissilePools {

    public class FireworkPool : MissilePool {
        protected override GameObject MissilePrefab => AssetReferences.fireworkPrefab;
    }
}