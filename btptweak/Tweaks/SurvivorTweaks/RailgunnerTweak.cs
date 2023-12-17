using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class RailgunnerTweak : TweakBase<RailgunnerTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            IL.EntityStates.Railgunner.Reload.Reloading.OnEnter += Reloading_OnEnter;
            IL.EntityStates.Railgunner.Weapon.BaseFireSnipe.ModifyBullet += BaseFireSnipe_ModifyBullet;
            On.EntityStates.Railgunner.Reload.Boosted.OnEnter += Boosted_OnEnter;
            On.EntityStates.Railgunner.Reload.Waiting.FixedUpdate += Waiting_FixedUpdate;
            On.EntityStates.Railgunner.Scope.BaseWindUp.OnEnter += BaseWindUp_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.RailgunnerMineAltDetonated.LoadComponent<SlowDownProjectiles>().slowDownCoefficient = 0.01f;
            RoR2ResourcesPaths.RailgunnerBodyFireSnipeHeavy.Load<RailgunSkillDef>().mustKeyPress = false;
            LanguageAPI.Add("KEYWORD_ACTIVERELOAD_ALT", $"<style=cKeywordName>手动上弹</style><style=cSub>按{ModConfig.ReloadKey.Value.MainKey.ToUtil()}键给你的磁轨炮上弹。<style=cIsDamage>完美上弹</style>后，下一发射弹额外造成{"50%".ToDmg() + "（每层备用弹夹+10%）".ToStk()}伤害。");
            ArrayUtils.ArrayAppend(ref RoR2ResourcesPaths.RailgunnerBodyScopeLight.Load<RailgunSkillDef>().keywordTokens, "KEYWORD_ACTIVERELOAD_ALT");
        }

        private void BaseFireSnipe_ModifyBullet(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   x => ILPatternMatchingExt.MatchLdloc(x, 3),
                                   x => ILPatternMatchingExt.MatchCallvirt<EntityStates.Railgunner.Reload.Boosted>(x, "GetBonusDamage"),
                                   ILPatternMatchingExt.MatchAdd)) {
                ++cursor.Index;
                cursor.RemoveRange(2);
                cursor.EmitDelegate((EntityStates.Railgunner.Reload.Boosted boosted) => boosted.bonusDamageCoefficient);
                cursor.Emit(OpCodes.Mul);
            } else {
                Main.Logger.LogError("Railgunner ReloadBoost Hook Failed!");
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
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchStloc(x, 1))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((EntityStates.Railgunner.Reload.Reloading reloading) => {
                    var body = reloading.characterBody;
                    reloading.attackSpeedStat = Mathf.Max(0.04f, reloading.attackSpeedStat - body.baseAttackSpeed * 0.1f * body.inventory.GetItemCount(RoR2Content.Items.SecondarySkillMagazine.itemIndex));
                });
            } else {
                Main.Logger.LogError("Railgunner ReloadingOnEnter Hook Failed!");
            }
        }

        private void Waiting_FixedUpdate(On.EntityStates.Railgunner.Reload.Waiting.orig_FixedUpdate orig, EntityStates.Railgunner.Reload.Waiting self) {
            orig(self);
            if (Input.GetKey(ModConfig.ReloadKey.Value.MainKey) && !self.isReloadQueued && self.CanReload()) {
                self.outer.SetNextState(new EntityStates.Railgunner.Reload.Reloading());
            }
        }
    }
}