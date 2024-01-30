using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff<T> where T : BaseActor
{
    protected BUFF_TYPE buff;
    protected string name;
    protected string tooltip;
    protected float lifetime;
    protected bool isRemovable;

    public BaseBuff(BuffData buffData)
    {
        buff = buffData.buff;
        name = buffData.name;
        tooltip = buffData.tooltip;
        lifetime = buffData.lifetime;
        isRemovable = buffData.isRemovable;
    }

    public abstract void SetLifetime(float lifetime);
    public abstract void Enable(T actor);
    public abstract void Update(T actor);
    public abstract void Disable(T actor);

    #region Property
    public BUFF_TYPE StatusEffect { get { return buff; } }
    public float Lifetime { get { return lifetime; } }
    #endregion
}
