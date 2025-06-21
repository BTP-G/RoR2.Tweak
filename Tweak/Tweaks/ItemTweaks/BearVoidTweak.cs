using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BearVoidTweak : ModComponent, IModLoadMessageHandler {
        public const int BasePercentChance = 50;

        public void Handle() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(DLC1Content.Buffs), "BearVoidReady"),
                i => i.MatchCallvirt<CharacterBody>("HasBuff"))) {
                cursor.Emit(OpCodes.Ldarg, 0)
                      .Emit(OpCodes.Ldarg, 1)
                      .EmitDelegate((bool hasBearVoidReady, HealthComponent healthComponent, DamageInfo damageInfo) => {
                          if (!hasBearVoidReady) return;
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
                cursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                "BearVoid Hook Failed!".LogError();
            }
        }
    }
}