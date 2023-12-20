using EntityStates;
using EntityStates.BrotherMonster;
using RoR2;

namespace BtpTweak.Tweaks.MithrixTweaks.MithrixEntityStates {

    public class EnterCrushingLeap : BaseSkillState {
        public static float baseDuration = 0.5f;

        private float duration;

        public override void OnEnter() {
            base.OnEnter();
            Util.PlaySound(EnterSkyLeap.soundString, gameObject);
            duration = baseDuration / attackSpeedStat;
            PlayAnimation("Body", "EnterSkyLeap", "SkyLeap.playbackRate", duration);
            PlayAnimation("FullBody Override", "BufferEmpty");
            characterDirection.moveVector = characterDirection.forward;
            AimAnimator aimAnimator = GetAimAnimator();
            if (aimAnimator) {
                aimAnimator.enabled = true;
            }
            if (isAuthority) {
                characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (fixedAge > duration && isAuthority) {
                outer.SetNextState(new AimCrushingLeap());
            }
        }
    }
}