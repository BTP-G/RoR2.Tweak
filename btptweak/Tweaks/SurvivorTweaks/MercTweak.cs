using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates;
using EntityStates.Merc;
using EntityStates.Merc.Weapon;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System.Linq;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class MercTweak : TweakBase<MercTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private static readonly BullseyeSearch _bullseyeSearch = new();

        private static float _evisDamageCoefficient;
        private static float _uppercutBaseDuration;
        private static float _uppercutBaseDamageCoefficient;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += GroundLight2_OnEnter;
            On.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;
            On.EntityStates.Merc.Uppercut.OnEnter += Uppercut_OnEnter;
            On.EntityStates.Merc.Assaulter2.OnEnter += Assaulter2_OnEnter;
            On.EntityStates.Merc.FocusedAssaultDash.OnEnter += FocusedAssaultDash_OnEnter;
            On.EntityStates.Merc.Evis.OnEnter += Evis_OnEnter;
            On.EntityStates.Merc.Evis.SearchForTarget += Evis_SearchForTarget;
            IL.EntityStates.Merc.Evis.FixedUpdate += Evis_FixedUpdate;
            IL.EntityStates.Merc.EvisDash.FixedUpdate += EvisDash_FixedUpdate;
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += ThrowEvisProjectile_OnEnter;
            EntityStateConfigurationPaths.EntityStatesMercWeaponGroundLight2.Load<EntityStateConfiguration>().Set("ignoreAttackSpeed", "1");
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            ArrayUtils.ArrayAppend(ref SteppedSkillDefPaths.MercGroundLight2.Load<SteppedSkillDef>().keywordTokens, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyWhirlwind.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyUppercut.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref MercDashSkillDefPaths.MercBodyAssaulter.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            var t = SkillDefPaths.MercBodyFocusedAssault.Load<SkillDef>().keywordTokens;
            ArrayUtils.ArrayAppend(ref t, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref t, "KEYWORD_STUNNING");
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyEvis.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            ArrayUtils.ArrayAppend(ref SkillDefPaths.MercBodyEvisProjectile.Load<SkillDef>().keywordTokens, "KEYWORD_FLEETING");
            Evis.minimumDuration = 1f;
            _evisDamageCoefficient = Evis.damageCoefficient;
            _uppercutBaseDuration = Uppercut.baseDuration;
            _uppercutBaseDamageCoefficient = Uppercut.baseDamageCoefficient;
        }

        private void GroundLight2_OnEnter(On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, GroundLight2 self) {
            orig(self);
            if (self.isAuthority) {
                self.overlapAttack.damage *= self.attackSpeedStat;
            }
            self.durationBeforeInterruptable = self.isComboFinisher
                ? GroundLight2.comboFinisherBaseDurationBeforeInterruptable
                : GroundLight2.baseDurationBeforeInterruptable;
        }

        private void Assaulter2_OnEnter(On.EntityStates.Merc.Assaulter2.orig_OnEnter orig, Assaulter2 self) {
            self.damageCoefficient *= self.characterBody.attackSpeed;
            orig(self);
        }

        private void FocusedAssaultDash_OnEnter(On.EntityStates.Merc.FocusedAssaultDash.orig_OnEnter orig, FocusedAssaultDash self) {
            self.damageCoefficient *= self.characterBody.attackSpeed;
            orig(self);
            self.delayedDamageCoefficient *= self.attackSpeedStat;
        }

        private void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, WhirlwindBase self) {
            var body = self.characterBody;
            self.baseDuration *= body.attackSpeed;
            self.baseDamageCoefficient *= body.attackSpeed;
            orig(self);
            self.moveSpeedStat = body.baseMoveSpeed;
        }

        private void Uppercut_OnEnter(On.EntityStates.Merc.Uppercut.orig_OnEnter orig, Uppercut self) {
            var body = self.characterBody;
            Uppercut.baseDuration = _uppercutBaseDuration * body.attackSpeed;
            Uppercut.baseDamageCoefficient = _uppercutBaseDamageCoefficient * body.attackSpeed;
            orig(self);
            self.moveSpeedStat = body.baseMoveSpeed;
        }

        private void EvisDash_FixedUpdate(ILContext il) {
            var cursor = new ILCursor(il);
            cursor.GotoNext(MoveType.After,
                            c => c.MatchLdarg(0),
                            c => c.MatchCall<EntityState>("get_isAuthority"));
            cursor.Emit(OpCodes.Ldloc_0);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate((bool isAuthority, bool flag, EvisDash evisDash) => {
                if (isAuthority) {
                    _bullseyeSearch.searchOrigin = evisDash.characterBody.corePosition;
                    _bullseyeSearch.searchDirection = evisDash.inputBank.aimDirection;
                    _bullseyeSearch.maxDistanceFilter = evisDash.characterBody.bestFitRadius + EvisDash.overlapSphereRadius * (flag ? EvisDash.lollypopFactor : 1f);
                    _bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
                    _bullseyeSearch.teamMaskFilter = TeamMask.GetEnemyTeams(evisDash.teamComponent.teamIndex);
                    _bullseyeSearch.RefreshCandidates();
                    if (_bullseyeSearch.GetResults().Any()) {
                        evisDash.outer.SetNextState(new Evis());
                        return true;
                    }
                }
                return false;
            });
            cursor.Index += 1;
            cursor.Emit(OpCodes.Ret);
        }

        private void Evis_OnEnter(On.EntityStates.Merc.Evis.orig_OnEnter orig, Evis self) {
            orig(self);
            Evis.damageCoefficient = _evisDamageCoefficient * self.attackSpeedStat;
        }

        private void Evis_FixedUpdate(ILContext il) {
            var cursor = new ILCursor(il);
            cursor.GotoNext(c => c.MatchLdarg(0), c => c.MatchLdfld<BaseState>("attackSpeedStat"));
            cursor.RemoveRange(3);
        }

        private HurtBox Evis_SearchForTarget(On.EntityStates.Merc.Evis.orig_SearchForTarget orig, Evis self) {
            _bullseyeSearch.searchOrigin = self.characterBody.corePosition;
            _bullseyeSearch.searchDirection = self.inputBank.aimDirection;
            _bullseyeSearch.maxDistanceFilter = Evis.maxRadius;
            _bullseyeSearch.sortMode = BullseyeSearch.SortMode.Angle;
            _bullseyeSearch.teamMaskFilter = TeamMask.GetEnemyTeams(self.teamComponent.teamIndex);
            _bullseyeSearch.RefreshCandidates();
            return _bullseyeSearch.GetResults().FirstOrDefault();
        }

        private void ThrowEvisProjectile_OnEnter(On.EntityStates.Merc.Weapon.ThrowEvisProjectile.orig_OnEnter orig, EntityStates.Merc.Weapon.ThrowEvisProjectile self) {
            var body = self.characterBody;
            self.baseDuration *= body.attackSpeed;
            self.baseDelayBeforeFiringProjectile *= body.attackSpeed;
            self.damageCoefficient *= body.attackSpeed;
            orig(self);
        }
    }
}