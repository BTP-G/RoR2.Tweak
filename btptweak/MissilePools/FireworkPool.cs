using BtpTweak.Utils;
using UnityEngine;

namespace BtpTweak.MissilePools
{
    public class FireworkPool : MissilePool
    {
        public static readonly GameObject fireworkPrefab = "RoR2/Base/Firework/FireworkProjectile.prefab".Load<GameObject>();
        protected override GameObject MissilePrefab => fireworkPrefab;
    }
}