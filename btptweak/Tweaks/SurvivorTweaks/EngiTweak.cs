using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.Engi.EngiBubbleShield;
using EntityStates.Engi.EngiWeapon;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class EngiTweak : TweakBase<EngiTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public static float BubbleShieldLifetime;
        public static float BubbleShieldRaius;
        private const float Interval = 0.2f;

        private static readonly BlastAttack _blastAttack = new() {
            attackerFiltering = AttackerFiltering.Default,
            falloffModel = BlastAttack.FalloffModel.None,
            baseForce = 2500f,
            radius = 15f,
        };

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Engi.EngiBubbleShield.Deployed.FixedUpdate += Deployed_FixedUpdate;
            On.EntityStates.Engi.EngiWeapon.FireGrenades.OnExit += FireGrenades_OnExit;
            IL.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            BubbleShieldLifetime = ModConfig.AddSliderOption(Main.Config,
                "工程师",
                "泡泡护盾持续时间",
                600f,
                0f,
                1000f,
                "泡泡护盾持续时间。修改后立即生效，但汉化在下次启动时更改。",
                onChanged: (newValue) => {
                    BubbleShieldLifetime = newValue;
                    GameObjectPaths.EngiBubbleShield.LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = newValue * 0.9f;
                    Deployed.lifetime = newValue;
                }).Value;
            BubbleShieldRaius = ModConfig.AddSliderOption(Main.Config,
                "工程师",
                "泡泡护盾半径",
                15f,
                0f,
                100f,
                "泡泡护盾半径。",
                onChanged: (newValue) => {
                    BubbleShieldRaius = newValue;
                    GameObjectPaths.EngiBubbleShield.LoadComponent<ChildLocator>().FindChild(Deployed.childLocatorString).transform.localScale = BubbleShieldRaius * 2 * Vector3.one;
                }).Value;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.EngiGrenadeProjectile.LoadComponent<ProjectileImpactExplosion>().blastRadius = 5f;

            Array.Find(GameObjectPaths.EngiTurretMaster.LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 2;
            GameObjectPaths.EngiBubbleShield.LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = BubbleShieldLifetime * 0.9f;
            GameObjectPaths.EngiBubbleShield.LoadComponent<ChildLocator>().FindChild(Deployed.childLocatorString).transform.localScale = new Vector3(30f, 30f, 30f);
            Deployed.lifetime = BubbleShieldLifetime;
            var skillDef = SkillDefPaths.EngiBodyPlaceBubbleShield.Load<SkillDef>();
            skillDef.baseRechargeInterval = 10;
            skillDef.baseMaxStock = 2;
            skillDef.requiredStock = 2;
            skillDef.stockToConsume = 2;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Engi, RecalculateEngiStats);
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.EngiTurret, RecalculateEngiStats);
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.EngiWalkerTurret, RecalculateEngiStats);
        }

        private void RecalculateEngiStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.armorAdd += 10 * inventory.GetItemCount(RoR2Content.Items.ArmorPlate.itemIndex);
        }

        private void FireGrenades_OnExit(On.EntityStates.Engi.EngiWeapon.FireGrenades.orig_OnExit orig, FireGrenades self) {
            orig(self);
            while (self.grenadeCount < self.grenadeCountMax) {
                if (self.grenadeCount++ % 2 == 0) {
                    self.FireGrenade("MuzzleLeft");
                } else {
                    self.FireGrenade("MuzzleRight");
                }
            }
        }

        private void CharacterMaster_GetDeployableSameSlotLimit(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchLdsfld(typeof(RoR2Content.Items), "BeetleGland"))) {
                cursor.Index += 7;
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((CharacterMaster master) => master.inventory.GetItemCount(RoR2Content.Items.UtilitySkillMagazine.itemIndex));
                cursor.Emit(OpCodes.Add);
            } else {
                Main.Logger.LogError("CharacterMaster_GetDeployableSameSlotLimit Hook failed!");
            }
        }

        private void Deployed_FixedUpdate(On.EntityStates.Engi.EngiBubbleShield.Deployed.orig_FixedUpdate orig, Deployed self) {
            orig(self);
            if (self.age > Interval) {
                self.age = 0;
                _blastAttack.position = self.transform.position;
                _blastAttack.teamIndex = self.GetComponent<TeamFilter>().teamIndex;
                _blastAttack.Fire();
            }
        }
    }
}