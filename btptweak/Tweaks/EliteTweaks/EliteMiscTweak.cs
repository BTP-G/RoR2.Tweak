using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BtpTweak.Tweaks.EliteTweaks {

    internal class EliteMiscTweak : TweakBase<EliteMiscTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            Stage.onStageStartGlobal += On_StageStartGlobal;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.LightningStake.LoadComponent<ProjectileImpactExplosion>().blastRadius = 10f;
            var ClassType_EffectHooks = typeof(Configuration).Assembly.GetTypes().First(type => type.Name == "EffectHooks");
            var targetMethod = ClassType_EffectHooks.GetMethod("ApplyAspectOnHitEnemyEffects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            HookEndpointManager.Modify<Action<ILContext>>(targetMethod, (ILContext il) => {
                var cursor = new ILCursor(il);
                cursor.GotoNext(i => i.MatchCall(ClassType_EffectHooks, "CreateFrostBlade"));
                cursor.Index -= 4;
                cursor.RemoveRange(6);
                cursor.GotoNext(i => i.MatchCall(ClassType_EffectHooks, "ApplyBurn"));
                cursor.Index -= 3;
                cursor.RemoveRange(5);
                cursor.GotoNext(i => i.MatchCall(ClassType_EffectHooks, "ApplyCollapse"));
                cursor.Index -= 3;
                cursor.RemoveRange(5);
            });
        }

        private void On_StageStartGlobal(Stage stage) {
            DropHooks.runDropCount = 0;
            var monsterMult = 1 /*+ 0.5f * Mathf.Pow(Run.instance.stageClearCount, 1.25f)*/;
            Configuration.AspectWhiteMonsterDamageMult.Value = (float)Configuration.AspectWhiteMonsterDamageMult.DefaultValue * monsterMult;
            Configuration.AspectBlueMonsterDamageMult.Value = (float)Configuration.AspectBlueMonsterDamageMult.DefaultValue * monsterMult;
            Configuration.AspectRedMonsterDamageMult.Value = (float)Configuration.AspectRedMonsterDamageMult.DefaultValue * monsterMult;
            Configuration.AspectVoidMonsterDamageMult.Value = (float)Configuration.AspectVoidMonsterDamageMult.DefaultValue * monsterMult;
            Configuration.AspectEarthMonsterLeechMult.Value = (float)Configuration.AspectEarthMonsterLeechMult.DefaultValue * monsterMult;
            Configuration.AspectPlatedMonsterPlatingMult.Value = (float)Configuration.AspectPlatedMonsterPlatingMult.DefaultValue * monsterMult;
        }
    }
}