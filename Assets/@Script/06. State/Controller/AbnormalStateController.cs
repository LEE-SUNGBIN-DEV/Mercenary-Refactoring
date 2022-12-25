using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalStateController
{
    private BaseActor actor;
    private int actorStateBit;
    private Dictionary<ABNORMAL_TYPE, AbnormalState> stateDictionary;

    public AbnormalStateController(BaseActor actor)
    {
        this.actor = actor;
        stateDictionary = new Dictionary<ABNORMAL_TYPE, AbnormalState>()
        {
            { ABNORMAL_TYPE.Stun, new StunState()}
        };
    }

    public bool UpdateState()
    {
        bool isAnyStateRunning = false;
        foreach(var state in stateDictionary.Values)
        {
            bool isStateRunning = false;
            if (CheckState(state))
            {
                isStateRunning = state.UpdateState(this);
                if (!isStateRunning)
                    SubtractState(state);
            }
            isAnyStateRunning = isAnyStateRunning || isStateRunning;
        }
        return isAnyStateRunning;
    }

    public void AddState(ABNORMAL_TYPE targetState, float duration)
    {
        actorStateBit |= (int)targetState;
        stateDictionary[targetState].SetDuration(duration);
    }
    public void AddState(AbnormalState targetState, float duration)
    {
        AddState(targetState.State, duration);
    }
    public void SubtractState(ABNORMAL_TYPE targetState)
    {
        actorStateBit &= ~(int)targetState;
    }
    public void SubtractState(AbnormalState targetState)
    {
        SubtractState(targetState.State);
    }
    public bool CheckState(ABNORMAL_TYPE targetState)
    {
        return (actorStateBit &= (int)targetState) == (int)targetState;
    }
    public bool CheckState(AbnormalState targetState)
    {
        return CheckState(targetState.State);
    }

    #region Property
    public BaseActor Actor { get { return actor; } }
    public int ActorStateBit { get { return actorStateBit; } set { actorStateBit = value; } }
    #endregion
}
