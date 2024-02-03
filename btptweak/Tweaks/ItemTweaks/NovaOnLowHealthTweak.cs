using BtpTweak.Messages;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class NovaOnLowHealthTweak : TweakBase<NovaOnLowHealthTweak>, IOnModLoadBehavior {
        private static readonly Dictionary<EntityStates.VagrantNovaItem.ReadyState, float> damagePool = [];

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.VagrantNovaItem.ReadyState.OnEnter += ReadyState_OnEnter;
            On.EntityStates.VagrantNovaItem.ReadyState.OnDamaged += ReadyState_OnDamaged;
            On.EntityStates.VagrantNovaItem.ReadyState.OnExit += ReadyState_OnExit;
            IL.EntityStates.VagrantNovaItem.DetonateState.OnEnter += DetonateState_OnEnter;
        }

        private void DetonateState_OnEnter(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchMul(), c => c.MatchStfld<BlastAttack>("baseDamage"))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((EntityStates.VagrantNovaItem.DetonateState state) => {
                    state.duration /= state.attachedBody.attackSpeed;
                    return (float)state.GetItemStack();
                });
                cursor.Emit(OpCodes.Mul);
            } else {
                Main.Logger.LogError(GetType().FullName + " add hook 'DetonateState_OnEnter1' failed!");
            }
            if (cursor.TryGotoNext(c => c.MatchStfld<BlastAttack>("damageType"))) {
                cursor.Index -= 1;
                cursor.Remove();
                cursor.Emit(OpCodes.Ldc_I4, (int)DamageType.Shock5s);
            } else {
                Main.Logger.LogError(GetType().FullName + " add hook 'DetonateState_OnEnter2' failed!");
            }
        }

        private void ReadyState_OnEnter(On.EntityStates.VagrantNovaItem.ReadyState.orig_OnEnter orig, EntityStates.VagrantNovaItem.ReadyState self) {
            orig(self);
            damagePool.Add(self, 0);
        }

        private void ReadyState_OnDamaged(On.EntityStates.VagrantNovaItem.ReadyState.orig_OnDamaged orig, EntityStates.VagrantNovaItem.ReadyState self, DamageReport report) {
            if (report.victimBody == self.attachedBody) {
                if ((damagePool[self] += report.damageDealt) > self.attachedHealthComponent.fullCombinedHealth * 0.75f) {
                    self.outer.SetNextStateServer(new EntityStates.VagrantNovaItem.ChargeState());
                }
            }
        }

        private void ReadyState_OnExit(On.EntityStates.VagrantNovaItem.ReadyState.orig_OnExit orig, EntityStates.VagrantNovaItem.ReadyState self) {
            orig(self);
            damagePool.Remove(self);
        }
    }
}