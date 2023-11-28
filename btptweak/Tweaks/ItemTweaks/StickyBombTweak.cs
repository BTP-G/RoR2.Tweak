using BtpTweak.ProjectileFountains;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class StickyBombTweak : TweakBase<StickyBombTweak> {
        public const int PercnetChance = 5;
        public const float BaseDamageCoefficient = 0.8f;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void Load() {
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            AssetReferences.stickyBombProjectile.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.2f;
            AssetReferences.stickyBombProjectile.RemoveComponent<LoopSound>();
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("StickyBomb")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) => {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Count) && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerMaster)) {
                        var simpleProjectileInfo = new ProjectileFountain.SimpleProjectileInfo {
                            attacker = damageInfo.attacker,
                            procChainMask = damageInfo.procChainMask,
                            isCrit = damageInfo.crit,
                        };
                        simpleProjectileInfo.procChainMask.AddWhiteProcs();
                        (victim.GetComponent<StickyBombFountain>()
                        ?? victim.AddComponent<StickyBombFountain>()).AddProjectile(simpleProjectileInfo,
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