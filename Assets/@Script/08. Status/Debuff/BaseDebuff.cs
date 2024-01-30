using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDebuff<T> where T : BaseActor
{
    protected DEBUFF_TYPE debuff;
    protected string name;
    protected string tooltip;
    protected float timer;
    protected float lifetime;
    protected bool isRemovable;

    public BaseDebuff(DebuffData debuffData)
    {
        debuff = debuffData.debuff;
        name = debuffData.name;
        tooltip = debuffData.tooltip;
        lifetime = debuffData.lifetime;
        isRemovable = debuffData.isRemovable;
    }

    public abstract void SetLifetime(float lifetime);
    public abstract void Enable(T actor);
    public abstract void Update(T actor);
    public abstract void Disable(T actor);

    #region Property
    public DEBUFF_TYPE StatusEffect { get { return debuff; } }
    public float Duration { get { return lifetime; } }
    #endregion
}
