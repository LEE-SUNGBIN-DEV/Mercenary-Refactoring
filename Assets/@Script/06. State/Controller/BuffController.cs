using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    private BaseActor actor;
    private int buffBit;
    private Dictionary<BUFF, BaseBuff> buffDictionary;

    public BuffController(BaseActor actor)
    {
        this.actor = actor;
        buffDictionary = new Dictionary<BUFF, BaseBuff>()
        {
            { BUFF.Stun, new Stun()}
        };
    }

    public bool UpdateBuff()
    {
        bool isAnyBuffRunning = false;
        foreach(var buff in buffDictionary.Values)
        {
            bool isBuffRunning = false;
            if (CheckBuff(buff))
            {
                isBuffRunning = buff.UpdateBuff(actor);
                if (!isBuffRunning)
                    SubBuff(buff);
            }
            isAnyBuffRunning = isAnyBuffRunning || isBuffRunning;
        }
        return isAnyBuffRunning;
    }

    public void AddBuff(BUFF targetState, float duration)
    {
        buffBit |= (int)targetState;
        buffDictionary[targetState].SetDuration(duration);
    }
    public void AddBuff(BaseBuff targetState, float duration)
    {
        AddBuff(targetState.Buff, duration);
    }
    public void SubBuff(BUFF targetState)
    {
        buffBit &= ~(int)targetState;
    }
    public void SubBuff(BaseBuff targetState)
    {
        SubBuff(targetState.Buff);
    }
    public bool CheckBuff(BUFF targetState)
    {
        return (buffBit &= (int)targetState) == (int)targetState;
    }
    public bool CheckBuff(BaseBuff targetState)
    {
        return CheckBuff(targetState.Buff);
    }

    #region Property
    public BaseActor Actor { get { return actor; } }
    public int BuffBit { get { return buffBit; } set { buffBit = value; } }
    #endregion
}
