using BTP.RoR2Plugin.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class MissileVoidTweak : ModComponent, IModLoadMessageHandler {
        public const float DamageCoefficient = 0.5f;
        public const float Interval = 0.2f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void MissileVoidOrb_Begin(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.Before, x => x.MatchLdcR4(75f))) {
                ilcursor.Remove()
                        .Emit(OpCodes.Ldc_R4, 100f);
            } else {
                LogExtensions.LogError("MissileVoidOrb Hook Failed!");
            }
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("MissileVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                            if (itemCount < 1 || !victimBody.mainHurtBox) {
                                return;
                            }
                            var shieldFraction = attackerBody.healthComponent.shield / attackerBody.healthComponent.fullShield;
                            if (!Util.CheckRoll(100f * shieldFraction, attackerBody.master)) {
                                return;
                            }
                            var simpleOrbInfo = new OrbPoolKey {
                                attackerBody = attackerBody,
                                isCrit = damageInfo.crit,
                                procChainMask = damageInfo.procChainMask,
                                target = victimBody.mainHurtBox,
                                通用浮点数 = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile.itemIndex),
                            };
                            simpleOrbInfo.procChainMask.AddRYProcs();
                            MissileVoidOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                                      Util.OnHitProcDamage(damageInfo.damage, 0, shieldFraction * DamageCoefficient * itemCount));
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("MissileVoid :: Hook Failed!");
            }
        }
    }
}