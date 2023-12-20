using RoR2;
using RoR2.Orbs;

namespace BtpTweak.OrbPools {

    internal class MissileVoidOrbPool : OrbPool<SimpleOrbInfo, MissileVoidOrb> {
        private int itemMoreMissileStack = 0;
        private Inventory inventory;
        protected override float OrbInterval => 0.1f;

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, float damageValue, TeamIndex teamIndex) {
            if (Pool.TryGetValue(simpleOrbInfo, out var bounceOrb)) {
                bounceOrb.damageValue += damageValue;
            } else {
                Pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attacker,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    target = simpleOrbInfo.target,
                    teamIndex = teamIndex,
                });
            }
        }

        protected override void ModifyOrb(ref MissileVoidOrb orb) {
            orb.origin = transform.position;
            orb.procChainMask.AddProc(ProcType.Missile);
            if (itemMoreMissileStack > 0) {
                orb.damageValue *= (itemMoreMissileStack + 1) * 0.5f;
                OrbManager.instance.AddOrb(new MissileVoidOrb() {
                    attacker = orb.attacker,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = orb.damageValue,
                    isCrit = orb.isCrit,
                    origin = orb.origin,
                    procChainMask = orb.procChainMask,
                    procCoefficient = 0.2f,
                    target = orb.target,
                    teamIndex = orb.teamIndex,
                });
                OrbManager.instance.AddOrb(new MissileVoidOrb() {
                    attacker = orb.attacker,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = orb.damageValue,
                    isCrit = orb.isCrit,
                    origin = orb.origin,
                    procChainMask = orb.procChainMask,
                    procCoefficient = 0.2f,
                    target = orb.target,
                    teamIndex = orb.teamIndex,
                });
            }
        }

        private void Awake() {
            var body = GetComponent<CharacterBody>();
            inventory = body.inventory;
            body.onInventoryChanged += MissileVoidOrbPool_onInventoryChanged;
        }

        private void OnDestroy() {
            GetComponent<CharacterBody>().onInventoryChanged -= MissileVoidOrbPool_onInventoryChanged;
        }

        private void MissileVoidOrbPool_onInventoryChanged() {
            itemMoreMissileStack = inventory.GetItemCount(DLC1Content.Items.MoreMissile.itemIndex);
        }
    }
}