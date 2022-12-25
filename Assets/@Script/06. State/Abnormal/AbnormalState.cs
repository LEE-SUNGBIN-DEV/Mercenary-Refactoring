using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbnormalState
{
    protected ABNORMAL_TYPE state;
    protected float duration;
    protected string animationParameter;

    public abstract void SetDuration(float duration);
    public abstract bool UpdateState(AbnormalStateController controller);

    #region Property
    public ABNORMAL_TYPE State { get { return state; } }
    #endregion
}
