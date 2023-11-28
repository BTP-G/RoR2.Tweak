using BtpTweak.ProjectileFountains;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Threading.Tasks;
using UnityEngine;
using static BtpTweak.ProjectileFountains.ProjectileFountain;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class FireballsOnHitTweak : TweakBase<FireballsOnHitTweak> {
        public const int BasePercentChance = 10;
        public const float 半数 = 9;
        public const float DamageCoefficient = 0.7f;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void Load() {
            var projectileController = AssetReferences.fireMeatBallProjectile.GetComponent<ProjectileController>();
            projectileController.procCoefficient = 1f;
            var projectileImpactExplosion = AssetReferences.fireMeatBallProjectile.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.blastProcCoefficient = 0.33f;
            projectileImpactExplosion.blastRadius = 10;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("FireballsOnHit")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(async (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) => {
                    if (itemCount == 0) {
                        return;
                    }
                    var simpleProjectileInfo = default(SimpleProjectileInfo);
                    var result = 0f;
                    await Task.Run(() => {
                        if (!damageInfo.procChainMask.HasProc(ProcType.Meatball) && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                            simpleProjectileInfo = new SimpleProjectileInfo {
                                attacker = damageInfo.attacker,
                                procChainMask = damageInfo.procChainMask,
                                isCrit = damageInfo.crit,
                            };
                            simpleProjectileInfo.procChainMask.AddYellowProcs();
                            result = Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount);
                        }
                    });
                    if (result > 0f) {
                        (victim.GetComponent<FireFountain>()
                        ?? victim.AddComponent<FireFountain>()).AddProjectile(simpleProjectileInfo, result);
                    }
                });
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("FireballsOnHit :: Hook Failed!");
            }
        }
    }
}