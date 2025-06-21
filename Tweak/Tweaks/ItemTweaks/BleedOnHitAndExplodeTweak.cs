using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BleedOnHitAndExplodeTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const int BaseRadius = 16;
        public const int StackRadius = 8;
        public const int DamageCoefficient = 1;
        public const float 半数 = 1f;

        public static readonly GameObject explosionEffect = GameObjectPaths.BleedOnHitAndExplodeExplosion.Load<GameObject>();

        private static readonly BlastAttack blastAttack = new() {
            crit = false,
            damageColorIndex = DamageColorIndex.Item,
            damageType = DamageType.AOE,
            falloffModel = BlastAttack.FalloffModel.SweetSpot,
            procChainMask = default,
            procCoefficient = 0,
        };

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            var delayBlast = GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect.GetComponent<DelayBlast>();
            delayBlast.baseForce = 0f;
            delayBlast.bonusForce = Vector3.zero;
            delayBlast.damageColorIndex = DamageColorIndex.Item;
            delayBlast.damageType = DamageType.AOE | DamageType.BleedOnHit;
            delayBlast.falloffModel = BlastAttack.FalloffModel.SweetSpot;
            delayBlast.maxTimer = 0.1f;
        }

        private void GlobalEventManager_OnCharacterDeath(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items), "BleedOnHitAndExplode"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_2)
                        .EmitDelegate((int itemCount, DamageReport damageReport, CharacterBody victimBody) => {
                            if (itemCount > 0 && (victimBody.HasBuff(RoR2Content.Buffs.SuperBleed.buffIndex) || victimBody.HasBuff(RoR2Content.Buffs.Bleeding.buffIndex))) {
                                var baseDamage = 0f;
                                foreach (var dotStack in DotController.FindDotController(victimBody.gameObject).dotStackList) {
                                    if (dotStack.dotIndex == DotController.DotIndex.Bleed || dotStack.dotIndex == DotController.DotIndex.SuperBleed) {
                                        baseDamage += dotStack.damage * Mathf.Ceil(dotStack.timer / dotStack.dotDef.interval);
                                    }
                                }
                                blastAttack.attacker = damageReport.attacker;
                                blastAttack.baseDamage = Util.OnKillProcDamage(baseDamage, Util2.CloseTo1(itemCount, 半数));
                                blastAttack.position = damageReport.damageInfo.position;
                                blastAttack.radius = BaseRadius + StackRadius * (itemCount - 1) + 1.6f * victimBody.bestFitRadius;
                                blastAttack.teamIndex = damageReport.attackerTeamIndex;
                                blastAttack.Fire();
                                EffectManager.SpawnEffect(explosionEffect, new EffectData {
                                    origin = blastAttack.position,
                                    scale = blastAttack.radius
                                }, true);
                                Util.PlaySound("Play_bleedOnCritAndExplode_explode", victimBody.gameObject);
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                "BleedOnHitAndExplode :: Hook Failed!".LogError();
            }
        }
    }
}