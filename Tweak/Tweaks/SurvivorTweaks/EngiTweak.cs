using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
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

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class EngiTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private const float Interval = 0.2f;

        private static readonly BlastAttack _blastAttack = new() {
            attackerFiltering = AttackerFiltering.Default,
            falloffModel = BlastAttack.FalloffModel.None,
            baseForce = 2500f,
            radius = 15f,
        };

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.Engi.EngiBubbleShield.Deployed.FixedUpdate += Deployed_FixedUpdate;
            On.EntityStates.Engi.EngiWeapon.FireGrenades.OnExit += FireGrenades_OnExit;
            IL.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            Settings.BubbleShieldLifetime = Settings.AddSliderOption(Settings.Config,
                 "工程师",
                 "泡泡护盾持续时间",
                 600f,
                 0f,
                 1000f,
                 "泡泡护盾持续时间。修改后立即生效，但汉化在下次启动时更改。",
                 onChanged: (newValue) => {
                     GameObjectPaths.EngiBubbleShield.LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = newValue * 0.9f;
                     Deployed.lifetime = newValue;
                 });
            Settings.BubbleShieldRaius = Settings.AddSliderOption(Settings.Config,
                "工程师",
                "泡泡护盾半径",
                15f,
                0f,
                100f,
                "泡泡护盾半径。",
                onChanged: (newValue) => {
                    GameObjectPaths.EngiBubbleShield.LoadComponent<ChildLocator>().FindChild(Deployed.childLocatorString).transform.localScale = newValue * 2 * Vector3.one;
                });
        }

        void IRoR2LoadedMessageHandler.Handle() {
            GameObjectPaths.EngiGrenadeProjectile.LoadComponent<ProjectileImpactExplosion>().blastRadius = 5f;
            Array.Find(GameObjectPaths.EngiTurretMaster.LoadComponents<AISkillDriver>(), match => match.customName == "FireAtEnemy").maxDistance *= 2;
            GameObjectPaths.EngiBubbleShield.LoadComponent<BeginRapidlyActivatingAndDeactivating>().delayBeforeBeginningBlinking = Settings.BubbleShieldLifetime.Value * 0.9f;
            GameObjectPaths.EngiBubbleShield.LoadComponent<ChildLocator>().FindChild(Deployed.childLocatorString).transform.localScale = Settings.BubbleShieldRaius.Value * 2 * Vector3.one;
            Deployed.lifetime = Settings.BubbleShieldLifetime.Value;
            var skillDef = SkillDefPaths.EngiBodyPlaceBubbleShield.Load<SkillDef>();
            skillDef.baseRechargeInterval = 10;
            skillDef.baseMaxStock = 2;
            skillDef.requiredStock = 2;
            skillDef.stockToConsume = 2;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Engi, RecalculateEngiStats);
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.EngiTurret, RecalculateEngiStats);
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.EngiWalkerTurret, RecalculateEngiStats);
            var body = BodyCatalog.GetBodyPrefabBodyComponent(BodyIndexes.EngiTurret);
            body.baseMaxShield = body.baseMaxHealth;
            body.baseArmor = 50f;
            body.PerformAutoCalculateLevelStats();
            body = BodyCatalog.GetBodyPrefabBodyComponent(BodyIndexes.EngiWalkerTurret);
            body.baseMaxShield = body.baseMaxHealth;
            body.baseArmor = 50f;
            body.PerformAutoCalculateLevelStats();
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
                "CharacterMaster_GetDeployableSameSlotLimit Hook failed!".LogError();
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