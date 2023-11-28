using BtpTweak.MissilePools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Threading.Tasks;
using UnityEngine;
using static BtpTweak.MissilePools.MissilePool;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileTweak : TweakBase<MissileTweak> {
        public const float BasePercnetChance = 10f;
        public const float 半数 = 9;
        public const float DamageCoefficient = 1.5f;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.RoR2.Projectile.MissileController.FixedUpdate += MissileController_FixedUpdate;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.RoR2.Projectile.MissileController.FixedUpdate -= MissileController_FixedUpdate;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void Load() {
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<ProjectileController>().procCoefficient = 0.5f;
            GlobalEventManager.CommonAssets.missilePrefab.GetComponent<QuaternionPID>().gain *= 100;
            var missileController = GlobalEventManager.CommonAssets.missilePrefab.GetComponent<MissileController>();
            missileController.acceleration *= 2f;
            missileController.delayTimer *= 0.5f;
            missileController.maxSeekDistance = float.MaxValue;
            missileController.turbulence = 0;
        }

        private void MissileController_FixedUpdate(On.RoR2.Projectile.MissileController.orig_FixedUpdate orig, MissileController self) {
            self.torquePID.timer = self.timer + Time.fixedDeltaTime;
            orig(self);
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Missile")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.EmitDelegate(async (int itemCount, DamageInfo damageInfo, GameObject victim, CharacterMaster attackerMaster) => {
                    if (itemCount == 0) {
                        return;
                    }
                    var missileInfo = default(MissileInfo);
                    var attacker = damageInfo.attacker;
                    var result = 0f;
                    await Task.Run(() => {
                        if (Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                            missileInfo = new MissileInfo {
                                missilePrefab = GlobalEventManager.CommonAssets.missilePrefab,
                                procChainMask = damageInfo.procChainMask,
                                isCrit = damageInfo.crit,
                                target = victim,
                            };
                            missileInfo.procChainMask.AddGreenProcs();
                            result = Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount);
                        }
                    });
                    if (result > 0f) {
                        (attacker.GetComponent<MissilePool>()
                        ?? attacker.AddComponent<MissilePool>()).AddMissile(missileInfo, result);
                    }
                });
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Missile :: FireHook Failed!");
            }
        }
    }
}