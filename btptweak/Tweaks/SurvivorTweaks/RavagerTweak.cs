using BtpTweak.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using R2API;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class RavagerTweak : TweakBase<RavagerTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float InfusionToDamageCoefficient = 0.1f;
        public const float JumpPowerMultCoefficient = 0.1f;
        public const float ThrowSlashVelocityDamageCoefficient = 0.05f;

        void IOnModLoadBehavior.OnModLoad() {
            var targetMethod = typeof(ThrowSlash).GetMethod("OnEnter", BindingFlags.Public | BindingFlags.Instance);
            HookEndpointManager.Modify<Action<ILContext>>(targetMethod, (ILContext il) => {
                var cursor = new ILCursor(il);
                cursor.GotoNext(i => i.MatchLdarg(0), i => i.MatchLdarg(0));
                cursor.Index += 2;
                cursor.RemoveRange(9).EmitDelegate((ThrowSlash throwSlash) => {
                    var body = throwSlash.characterBody;
                    return Mathf.Clamp01(body.characterMotor.velocity.magnitude / (0.37334976f * body.jumpPower * Mathf.Clamp(body.moveSpeed, 1f, 18f)));
                });
                cursor.GotoNext(i => i.MatchLdarg(0), i => i.MatchLdarg(0));
                cursor.Emit(OpCodes.Ldarg_0).GotoNext(MoveType.After, i => i.MatchLdfld<ThrowSlash>("charge"));
                cursor.RemoveRange(7).EmitDelegate((ThrowSlash throwSlash, float charge) => {
                    var body = throwSlash.characterBody;
                    var maxDamageCoefficient = Slash._damageCoefficient + Mathf.Pow(body.characterMotor.velocity.magnitude * ThrowSlashVelocityDamageCoefficient * (body.jumpPower / body.baseJumpPower), 1.5f);
                    return Mathf.Lerp(Slash._damageCoefficient, maxDamageCoefficient, charge);
                });
            });
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            Slash._damageCoefficient = 2.5f;
            SlashCombo._damageCoefficient = 2f;
            SlashCombo.finisherDamageCoefficient = 4f;
            SpinSlash._damageCoefficient = 10f;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.RobRavager, RecalculateRavagerStats);
            var bodyPrefab = BodyCatalog.GetBodyPrefab(BodyIndexes.RobRavager);
            var c = bodyPrefab.GetComponent<RedGuyController>();
            c.drainRate = 10f;
            c.altDrainRate = 5f;
        }

        private void RecalculateRavagerStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.baseDamageAdd += InfusionToDamageCoefficient * inventory.infusionBonus;
            args.jumpPowerMultAdd += JumpPowerMultCoefficient * inventory.GetItemCount(RoR2Content.Items.JumpBoost.itemIndex);
        }
    }
}