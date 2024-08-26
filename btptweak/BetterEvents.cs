using Cysharp.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak {

    public static class BetterEvents {

        static BetterEvents() {
            Init();
        }

        public delegate void BetterHitEnemyEventHandler(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody);

        public delegate void BetterHitAllEventHandler(DamageInfo damageInfo, CharacterBody attackerBody);

        public static event BetterHitEnemyEventHandler OnHitEnemy;

        public static event BetterHitAllEventHandler OnHitAll;

        private static void Init() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitAll += GlobalEventManager_OnHitAll;
        }

        private static void GlobalEventManager_OnHitAll(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject hitObject) {
            if (damageInfo.procCoefficient == 0f || damageInfo.rejected || !damageInfo.attacker) {
                return;
            }
            if (damageInfo.attacker.TryGetComponent<CharacterBody>(out var attackerBody) && attackerBody.master) {
                OnHitAll?.Invoke(damageInfo, attackerBody);
            }
        }

        private static void GlobalEventManager_OnHitEnemy(ILContext il) {
            var iLCursor = new ILCursor(il);
            if (iLCursor.TryGotoNext(MoveType.Before,
                                     x => x.MatchLdloc(4),
                                     x => x.MatchCallvirt<CharacterMaster>("get_inventory"),
                                     x => x.MatchStloc(5))) {
                iLCursor.Emit(OpCodes.Ldarg_1);
                iLCursor.Emit(OpCodes.Ldloc_1);
                iLCursor.Emit(OpCodes.Ldloc_2);
                iLCursor.EmitDelegate((DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    OnHitEnemy?.Invoke(damageInfo, attackerBody, victimBody);
                });
            } else {
                Main.Logger.LogError("OnHitEnemy Hook Failed!");
            }
        }
    }
}