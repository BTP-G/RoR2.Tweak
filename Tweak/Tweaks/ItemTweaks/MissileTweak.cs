using BTP.RoR2Plugin.Pools;
using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class MissileTweak : TweakBase<MissileTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BasePercnetChance = 10f;
        public const float 半数 = 9;
        public const float DamageCoefficient = 1f;
        public const float Interval = 1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<ProjectileController>().procCoefficient = 0.5f;
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<QuaternionPID>().gain *= 100;
            var missileController = GlobalEventManager.CommonAssets.missilePrefab.GetComponent<MissileController>();
            missileController.acceleration *= 2f;
            missileController.delayTimer *= 0.5f;
            missileController.maxSeekDistance = float.MaxValue;
            missileController.turbulence = 0;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Missile")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldarg_2)
                        .Emit(OpCodes.Ldloc_0)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                            if (itemCount > 0 && Util.CheckRoll(Util2.CloseTo(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerBody.master)) {
                                var missileInfo = new MissilePoolKey {
                                    missilePrefab = GlobalEventManager.CommonAssets.missilePrefab,
                                    procChainMask = damageInfo.procChainMask,
                                    isCrit = damageInfo.crit,
                                    target = victim,
                                    attackerBody = attackerBody,
                                };
                                missileInfo.procChainMask.AddRYProcs();
                                MissilePool.RentPool(damageInfo.attacker).AddMissile(missileInfo,
                                                                                     Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount));
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("Missile :: FireHook Failed!");
            }
        }
    }
}