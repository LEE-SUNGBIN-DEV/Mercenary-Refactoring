using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalState
{
    private ABNORMAL_STATE state;
    private float duration;
    private string animationParameter;

    public AbnormalState(ABNORMAL_STATE state, string animationParameter)
    {
        this.state = state;
        this.animationParameter = animationParameter;
        duration = 0f;
    }

    public bool CheckState(int targetState)
    {
        return (targetState &= (int)state) == (int)state;
    }

    public bool UpdateState(AbnormalStateController controller)
    {
        if(duration > 0f)
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

    #region Property
    public ABNORMAL_STATE State { get { return state; } }
    public float Duration { get { return duration; } set { duration = value; } }
    #endregion
}
