using BtpTweak.Pools.ProjectilePools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class StickyBombTweak : TweakBase<StickyBombTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const int PercnetChance = 5;
        public const float BaseDamageCoefficient = 1f;
        public const float Interval = 0.1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.2f;
            AssetReferences.stickyBombProjectile.RemoveComponent<LoopSound>();
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("StickyBomb")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc, 5)
                        .Emit(OpCodes.Ldloc_2)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                            if (itemCount > 0
                            && !damageInfo.procChainMask.HasProc(ProcChainTweak.StickyBombOnHit)
                            && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerMaster)) {
                                var simpleProjectileInfo = new ProjectilePoolKey {
                                    attacker = damageInfo.attacker,
                                    isCrit = damageInfo.crit,
                                    procChainMask = damageInfo.procChainMask,
                                    targetBody = victimBody,
                                };
                                simpleProjectileInfo.procChainMask.AddGRYProcs();
                                StickyBombFountain.RentPool(victimBody.gameObject).AddProjectile(simpleProjectileInfo,
                                                                                                 Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient));
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("StickyBomb :: Hook Failed!");
            }
        }
    }
}