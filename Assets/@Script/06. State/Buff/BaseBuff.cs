using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff
{
    protected BUFF_TYPE buffType;
    protected BUFF buff;
    protected float duration;
    protected string animationParameter;

    public abstract void SetDuration(float duration);
    public abstract void EnableBuff(BaseActor actor);
    public abstract bool UpdateBuff(BaseActor actor);
    public abstract void DisableBuff(BaseActor actor);

    #region Property
    public BUFF Buff { get { return buff; } }
    #endregion
}
