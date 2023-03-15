using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE_SWITCH_BY
{
    FORCED,
    WEIGHT
}

public class StateController
{
    protected Animator animator;
    protected IActionState prevState;
    protected IActionState currentState;
    protected Dictionary<ACTION_STATE, IActionState> stateDictionary;

    public StateController(Animator animator)
    {
        this.animator = animator;
        stateDictionary = new Dictionary<ACTION_STATE, IActionState>();
    }

    public virtual void Update()
    {
        currentState?.Update();
    }

    #region State Functions
    private void SwitchState(ACTION_STATE targetState, float duration = 0f)
    {
        prevState = currentState;
        currentState?.Exit();
        currentState = stateDictionary[targetState];
        if (currentState is IDurationState lifetimeState)
        {
            lifetimeState.SetDuration(duration);
        }
        currentState?.Enter();
    }

    public virtual bool SetState(ACTION_STATE targetState, STATE_SWITCH_BY mode, float duration = 0f)
    {
        if (stateDictionary.ContainsKey(targetState))
        {
            switch(mode)
            {
                case STATE_SWITCH_BY.WEIGHT:
                    if (stateDictionary[targetState].StateWeight > currentState?.StateWeight)
                    {
                        SwitchState(targetState, duration);
                        return true;
                    }
                    return false;

                case STATE_SWITCH_BY.FORCED:
                    SwitchState(targetState, duration);
                    return true;
            }
        }
        return false;
    }

    public virtual ACTION_STATE CompareStateWeight(ACTION_STATE targetStateA, ACTION_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    // Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public virtual bool SetStateNotInTransition(int currentNameHash, ACTION_STATE targetState)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && !animator.IsInTransition(0))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }

    // !! 해당 Animation의 특정 구간을 만족하고, Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public virtual bool SetStateByAnimationTimeUpTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime)
    {
        if (animator.IsAnimationNormalizeTimeUpTo(currentNameHash, normalizedTime) && !animator.IsInTransition(0))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public virtual bool SetStateByAnimationTimeDownTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime)
    {
        if (animator.IsAnimationNormalizeTimeDownTo(currentNameHash, normalizedTime) && !animator.IsInTransition(0))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public virtual bool SetStateByAnimationTimeBetweenTo(int currentNameHash, ACTION_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime)
    {
        if (animator.IsAnimationNormalizeTimeBetweenTo(currentNameHash, lowerNormalizedTime, upperNormalizedTime) && !animator.IsInTransition(0))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    #endregion


    #region Property
    public Dictionary<ACTION_STATE, IActionState> StateDictionary { get { return stateDictionary; } }
    public IActionState PrevState { get { return prevState; } }
    public IActionState CurrentState { get { return currentState; } }
    #endregion
}
