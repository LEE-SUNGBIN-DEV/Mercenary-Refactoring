using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : AbnormalState
{
    public StunState()
    {
        state = ABNORMAL_TYPE.Stun;
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
            controller.Actor.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STUN, true);
            duration -= Time.deltaTime;
            return true;
        }
        else
        {
            controller.Actor.Animator.SetBool(Constants.ANIMATOR_PARAMETERS_BOOL_STUN, false);
            duration = 0f;
            return false;
        }
    }
}
