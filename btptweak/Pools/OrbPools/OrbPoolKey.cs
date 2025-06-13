using RoR2;

namespace BTP.RoR2Plugin.Pools.OrbPools {

    public struct OrbPoolKey {
        public bool isCrit;
        public CharacterBody attackerBody;
        public float 通用浮点数;
        public HurtBox target;
        public ProcChainMask procChainMask;
    }
}