using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectController<T> where T : BaseActor
{
    protected T actor;
    protected Dictionary<BUFF_TYPE, BaseBuff<T>> buffDictionary;
    protected Dictionary<DEBUFF_TYPE, BaseDebuff<T>> debuffDictionary;

    public StatusEffectController(T actor)
    {
        this.actor = actor;
        buffDictionary = new Dictionary<BUFF_TYPE, BaseBuff<T>>();
        debuffDictionary = new Dictionary<DEBUFF_TYPE, BaseDebuff<T>>();
    }

    public virtual void Update()
    {
        UpdateBuff();
        UpdateDebuff();
    }

    public void UpdateBuff()
    {
        foreach (BaseBuff<T> buff in buffDictionary.Values)
        {
            if(buff.Lifetime <= 0)
            {
                buff.Disable(actor);
            }
            else
            {
                buff.Update(actor);
            }
        }
    }
    public void UpdateDebuff()
    {
        foreach (BaseDebuff<T> debuff in debuffDictionary.Values)
        {
            if (debuff.Duration <= 0)
            {
                debuff.Disable(actor);
            }
            else
            {
                debuff.Update(actor);
            }
        }
    }

    public BaseBuff<T> FindBuff(BUFF_TYPE buff)
    {
        buffDictionary.TryGetValue(buff, out BaseBuff<T> value);
        return value;
    }
    public BaseDebuff<T> FindDebuff(DEBUFF_TYPE debuff)
    {
        debuffDictionary.TryGetValue(debuff, out BaseDebuff<T> value);
        return value;
    }
    public void AddBuff(BUFF_TYPE buff, float duration = 0)
    {
        if (FindBuff(buff) == null)
        {
        }

        if (duration != 0)
        {
            buffDictionary[buff].SetLifetime(duration);
        }

        buffDictionary[buff].Enable(actor);

    }
    public void SubBuff(BUFF_TYPE buff)
    {
        if (FindBuff(buff) != null)
        {
            buffDictionary[buff].SetLifetime(0f);
        }
    }
    public void AddDebuff(DEBUFF_TYPE debuff, float duration = 0)
    {
        if (FindDebuff(debuff) == null)
        {
        }

        if (duration != 0)
        {
            debuffDictionary[debuff].SetLifetime(duration);
        }

        debuffDictionary[debuff].Enable(actor);
    }
    public void SubDebuff(DEBUFF_TYPE debuff)
    {
        if (FindDebuff(debuff) != null)
        {
            debuffDictionary[debuff].SetLifetime(0f);
        }
    }

    #region Property
    public BaseActor Actor { get { return actor; } }

    public Dictionary<BUFF_TYPE, BaseBuff<T>> BuffDictionary { get { return buffDictionary; } }
    public Dictionary<DEBUFF_TYPE, BaseDebuff<T>> DebuffDictionary { get { return debuffDictionary; } }
    #endregion
}
