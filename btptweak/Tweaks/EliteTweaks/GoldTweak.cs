using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using RoR2.Projectile;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;

namespace BtpTweak.Tweaks.EliteTweaks {

    internal class GoldTweak : TweakBase<GoldTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            RemoveOrigHook();
            BetterEvents.OnHitEnemy += BetterEvents_OnHitEnemy;
        }

        private void RemoveOrigHook() {
            var targetMethod = typeof(GoldenCoastPlus.GoldenCoastPlus).GetMethod("GlobalEventManager_OnHitEnemy", BindingFlags.NonPublic | BindingFlags.Instance);
            HookEndpointManager.Modify<Action<ILContext>>(targetMethod, (ILContext il) => {
                var cursor = new ILCursor(il);
                cursor.RemoveRange(204);
            });
        }

        private void BetterEvents_OnHitEnemy(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) {
            if (attackerBody.HasBuff(GoldenCoastPlus.GoldenCoastPlus.affixGoldDef)) {
                attackerBody.master.GiveMoney((uint)(2f * Run.instance.difficultyCoefficient));
            }
            int itemCount = attackerBody.inventory.GetItemCount(GoldenCoastPlus.GoldenCoastPlus.bigSwordDef);
            if (itemCount > 0
                && !damageInfo.procChainMask.HasProc(ProcType.AACannon)
                && Util.CheckRoll(GoldenCoastPlus.GoldenCoastPlus.SwordChance.Value * damageInfo.procCoefficient, attackerBody.master)
                && Physics.Raycast(damageInfo.position, Vector3.down, out var raycastHit, float.PositiveInfinity, LayerIndex.world.mask)) {
                var fireProjectileInfo = new FireProjectileInfo {
                    crit = damageInfo.crit,
                    damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, itemCount * GoldenCoastPlus.GoldenCoastPlus.SwordDamage.Value),
                    damageColorIndex = DamageColorIndex.Item,
                    force = 10000f,
                    fuseOverride = 0.5f,
                    owner = attackerBody.gameObject,
                    position = raycastHit.point,
                    procChainMask = damageInfo.procChainMask,
                    projectilePrefab = AssetReferences.titanGoldPreFistProjectile,
                    rotation = Quaternion.identity,
                    target = victimBody.gameObject,
                    useFuseOverride = true,
                };
                fireProjectileInfo.procChainMask.AddProc(ProcType.AACannon);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }
    }
}