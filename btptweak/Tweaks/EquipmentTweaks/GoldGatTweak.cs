using EntityStates.GoldGat;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.EquipmentTweaks {

    internal class GoldGatTweak : TweakBase<GoldGatTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.EntityStates.GoldGat.GoldGatFire.FireBullet += GoldGatFire_FireBullet;
        }

        private void GoldGatFire_FireBullet(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(MoveType.After, x => x.MatchLdsfld<GoldGatFire>("baseMoneyCostPerBullet"))) {
                ++iLCursor.Index;
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.Emit<GoldGatFire>(OpCodes.Ldfld, "totalStopwatch");
                iLCursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("GoldGat Hook Failed!");
            }
            if (iLCursor.TryGotoNext(MoveType.After, x => x.MatchLdsfld<GoldGatFire>("damageCoefficient"))) {
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.EmitDelegate((GoldGatFire goldGat) => (float)goldGat.body.inventory.GetItemCount(DLC1Content.Items.GoldOnHurt.itemIndex));
                iLCursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("GoldGat DamageHook 1 Failed!");
            }
            if (iLCursor.TryGotoNext(x => x.MatchStfld<BulletAttack>("damage"))) {
                iLCursor.Emit(OpCodes.Ldloc_2);
                iLCursor.Emit(OpCodes.Conv_R4);
                iLCursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("GoldGat DamageHook 2 Failed!");
            }
        }
    }
}