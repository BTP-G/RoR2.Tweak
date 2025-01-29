using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BearVoidTweak : TweakBase<BearVoidTweak>, IOnModLoadBehavior {
        public const int BasePercentChance = 50;

        public void OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.Match(OpCodes.Brtrue),
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdfld<HealthComponent>("body"),
                                     x => x.MatchLdsfld(typeof(DLC1Content.Buffs), "BearVoidReady"))) {  // 113
                ilcursor.Index += 10;
                ilcursor.RemoveRange(53)
                        .Emit(OpCodes.Ldarg, 0)
                        .Emit(OpCodes.Ldarg, 1)
                        .EmitDelegate((HealthComponent healthComponent, DamageInfo damageInfo) => {
                            var body = healthComponent.body;
                            if (body.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex) || Util.CheckRoll(BasePercentChance)) {
                                EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab, new EffectData() {
                                    origin = damageInfo.position,
                                    rotation = Util.QuaternionSafeLookRotation(damageInfo.force != Vector3.zero ? damageInfo.force : Random.onUnitSphere)
                                }, true);
                                damageInfo.rejected = true;
                            }
                            body.RemoveBuff(DLC1Content.Buffs.BearVoidReady.buffIndex);
                            body.AddTimedBuff(DLC1Content.Buffs.BearVoidCooldown.buffIndex, 15f * Mathf.Pow(0.9f, body.inventory.GetItemCount(DLC1Content.Items.BearVoid)));
                        });
            } else {
                Main.Logger.LogError("BearVoid Hook Failed!");
            }
        }
    }
}