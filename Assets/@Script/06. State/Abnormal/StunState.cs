using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : AbnormalState
{
    public StunState()
    {
        state = ABNORMAL_TYPE.Stun;
        animationParameter = Constants.ANIMATOR_PARAMETERS_BOOL_STUN;
        duration = 0f;
    }

    public override void SetDuration(float duration)
    {
        if (this.duration < duration)
            this.duration = duration;
    }

    public override bool UpdateState(AbnormalStateController controller)
    {
        if (duration > 0f)
        {
            controller.Actor.Animator.SetBool(animationParameter, true);
            duration -= Time.deltaTime;
            return true;
        }
        else
        {
            controller.Actor.Animator.SetBool(animationParameter, false);
            duration = 0f;
            return false;
        }
    }
}
