using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin {

    public static class BetterEvents {

        static BetterEvents() {
            Init();
        }

        public delegate void BetterHitEnemyEventHandler(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody);

        public delegate void BetterHitAllEventHandler(DamageInfo damageInfo, CharacterBody attackerBody);

        public static event BetterHitEnemyEventHandler OnHitEnemy;

        public static event BetterHitAllEventHandler OnHitAll;

        private static void Init() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitAllProcess += GlobalEventManager_OnHitAll;
        }

        private static void GlobalEventManager_OnHitAll(On.RoR2.GlobalEventManager.orig_OnHitAllProcess orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject hitObject) {
            if (damageInfo.procCoefficient == 0f || damageInfo.rejected || damageInfo.attacker == null) {
                return;
            }
            if (damageInfo.attacker.TryGetComponent<CharacterBody>(out var attackerBody) && attackerBody.master is not null) {
                OnHitAll?.Invoke(damageInfo, attackerBody);
            }
        }

        private static void GlobalEventManager_OnHitEnemy(ILContext il) {
            var iLCursor = new ILCursor(il);
            if (iLCursor.TryGotoNext(MoveType.Before,
                                     x => x.MatchLdloc(7),
                                     x => x.MatchCallvirt<CharacterMaster>("get_inventory"),
                                     x => x.MatchStloc(8))) {
                iLCursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                            OnHitEnemy?.Invoke(damageInfo, attackerBody, victimBody);
                        });
            } else {
                 "OnHitEnemy Hook Failed!".LogError();
            }
        }
    }
}