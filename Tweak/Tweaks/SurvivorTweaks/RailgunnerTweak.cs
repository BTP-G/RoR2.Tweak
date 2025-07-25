﻿using BTP.RoR2Plugin.Language;
using BTP.RoR2Plugin.Utils;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class RailgunnerTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float HH44DamageCoefficient = 4f;
        public const float HH44PiercingDamageCoefficientPerTarget = 0.5f;
        public const float M99DamageCoefficient = 8f;
        public const float M99PiercingDamageCoefficientPerTarget = 0.9f;
        public const float CryochargeDamageCoefficient = 20f;
        public const float CryochargeProcCoefficient = 2f;
        public const float SuperchargeDamageCoefficient = 30f;
        public const float SuperchargeCritDamageCoefficient = 1.5f;
        public const float PolarFieldDeviceSlowDownCoefficient = 0.01f;

        void IModLoadMessageHandler.Handle() {
            IL.EntityStates.Railgunner.Reload.Reloading.OnEnter += Reloading_OnEnter;
            IL.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += BaseFireSnipe_ModifyBullet;
            On.EntityStates.Railgunner.Reload.Boosted.OnEnter += Boosted_OnEnter;
            On.EntityStates.Railgunner.Reload.Waiting.FixedUpdate += Waiting_FixedUpdate;
            On.EntityStates.Railgunner.Scope.BaseWindUp.OnEnter += BaseWindUp_OnEnter;
            EntityStateConfigurationPaths.EntityStatesRailgunnerWeaponFirePistol.Load<EntityStateConfiguration>().Set("selfKnockbackForce", "0");
            EntityStateConfigurationPaths.EntityStatesRailgunnerWeaponFireSnipeHeavy.Load<EntityStateConfiguration>().Set(new System.Collections.Generic.Dictionary<string, string> {
                ["damageCoefficient"] = M99DamageCoefficient.ToString(),
                ["piercingDamageCoefficientPerTarget"] = M99PiercingDamageCoefficientPerTarget.ToString(),
            });
            EntityStateConfigurationPaths.EntityStatesRailgunnerWeaponFireSnipeSuper.Load<EntityStateConfiguration>().Set("damageCoefficient", SuperchargeDamageCoefficient.ToString());
            EntityStateConfigurationPaths.EntityStatesRailgunnerWeaponFireSnipeCryo.Load<EntityStateConfiguration>().Set("procCoefficient", CryochargeProcCoefficient.ToString());
            //ProcCoefficientCatalog.AddSkill("RailgunnerBodyChargeSnipeCryo", "SKILL_PROJECTILE_NAME", CryochargeProcCoefficient);
            //ProcCoefficientCatalog.AddSkill("RailgunnerBodyFireSnipeCryo", "SKILL_PROJECTILE_NAME", CryochargeProcCoefficient);
        }

        void IRoR2LoadedMessageHandler.Handle() {
            GameObjectPaths.RailgunnerMineAltDetonated.LoadComponent<SlowDownProjectiles>().slowDownCoefficient = PolarFieldDeviceSlowDownCoefficient;
            GameObjectPaths.RailgunnerMineAltDetonated.LoadComponent<BuffWard>().radius = 15f;
            GameObjectPaths.RailgunnerPistolProjectile.LoadComponent<ProjectileSimple>().lifetime = 1f;
            RoR2ResourcesPaths.RailgunnerBodyFireSnipeHeavy.Load<RailgunSkillDef>().mustKeyPress = false;
            LanguageAPI.Add("KEYWORD_ACTIVERELOAD_ALT", $"<style=cKeywordName>手动上弹</style><style=cSub>按{Settings.ReloadKey.Value.MainKey.ToUtil()}键给你的磁轨炮上弹。<style=cIsDamage>完美上弹</style>后，下一发射弹额外造成{"50%".ToDmg()}{"（每层备用弹夹+10%）".ToStk()}伤害。");
            ArrayUtils.ArrayAppend(ref RoR2ResourcesPaths.RailgunnerBodyScopeLight.Load<RailgunSkillDef>().keywordTokens, "KEYWORD_ACTIVERELOAD_ALT");
        }

        private void BaseFireSnipe_ModifyBullet(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   x => x.MatchLdloc(3),
                                   x => x.MatchCallvirt<EntityStates.Railgunner.Reload.Boosted>("GetBonusDamage"),
                                   ILPatternMatchingExt.MatchAdd)) {
                ++cursor.Index;
                cursor.RemoveRange(2);
                cursor.EmitDelegate((EntityStates.Railgunner.Reload.Boosted boosted) => boosted.bonusDamageCoefficient);
                cursor.Emit(OpCodes.Mul);
            } else {
                LogExtensions.LogError("Railgunner ReloadBoost Hook Failed!");
            }
        }

        private void BaseWindUp_OnEnter(On.EntityStates.Railgunner.Scope.BaseWindUp.orig_OnEnter orig, EntityStates.Railgunner.Scope.BaseWindUp self) {
            if (self.isAuthority) {
                HurtBox.sniperTargetRadiusSqr = Mathf.Pow(1 + self.characterBody.inventory.GetItemCount(DLC1Content.Items.CritDamage.itemIndex), 2f);
            }
            orig(self);
        }

        private void Boosted_OnEnter(On.EntityStates.Railgunner.Reload.Boosted.orig_OnEnter orig, EntityStates.Railgunner.Reload.Boosted self) {
            orig(self);
            self.bonusDamageCoefficient = 1.5f + 0.1f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
        }

        private void Reloading_OnEnter(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After, x => x.MatchStloc(1))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((EntityStates.Railgunner.Reload.Reloading reloading) => {
                    var body = reloading.characterBody;
                    reloading.attackSpeedStat = Mathf.Max(0.001f, reloading.attackSpeedStat - body.baseAttackSpeed * 0.15f * body.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine.itemIndex));
                });
            } else {
                LogExtensions.LogError("Railgunner ReloadingOnEnter Hook Failed!");
            }
        }

        private void Waiting_FixedUpdate(On.EntityStates.Railgunner.Reload.Waiting.orig_FixedUpdate orig, EntityStates.Railgunner.Reload.Waiting self) {
            orig(self);
            if (Input.GetKey(Settings.ReloadKey.Value.MainKey) && !self.isReloadQueued && self.CanReload()) {
                self.outer.SetNextState(new EntityStates.Railgunner.Reload.Reloading());
            }
        }
    }
}