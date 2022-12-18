using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalStateController
{
    private BaseActor actor;
    private int actorStateBit;
    private Dictionary<ABNORMAL_STATE, AbnormalState> stateDictionary;

    public AbnormalStateController(BaseActor actor)
    {
        this.actor = actor;
        stateDictionary = new Dictionary<ABNORMAL_STATE, AbnormalState>()
        {
            { ABNORMAL_STATE.Stun, new AbnormalState(ABNORMAL_STATE.Stun, Constants.ANIMATOR_PARAMETERS_BOOL_STUN)}
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

    public void AddState(ABNORMAL_STATE targetState, float duration)
    {
        actorStateBit |= (int)targetState;
        stateDictionary[targetState].Duration += duration;
    }
    public void AddState(AbnormalState targetState, float duration)
    {
        AddState(targetState.State, duration);
    }
    public void SubtractState(ABNORMAL_STATE targetState)
    {
        actorStateBit &= ~(int)targetState;
    }
    public void SubtractState(AbnormalState targetState)
    {
        SubtractState(targetState.State);
    }
    public bool CheckState(ABNORMAL_STATE targetState)
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
