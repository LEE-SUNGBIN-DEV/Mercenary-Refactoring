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
    protected IActionState mainState;
    protected IActionState subState;
    protected Dictionary<ACTION_STATE, IActionState> stateDictionary;

    public StateController(Animator animator)
    {
        this.animator = animator;
        stateDictionary = new Dictionary<ACTION_STATE, IActionState>();
    }

    public virtual void Update()
    {
        mainState?.Update();
        subState?.Update();
    }

    #region Main State Functions
    private void SwitchState(ACTION_STATE targetState, float duration = 0f)
    {
        mainState?.Exit();
        mainState = stateDictionary[targetState];
        if (mainState is IDurationState lifetimeState)
        {
            lifetimeState.SetDuration(duration);
        }
        if(subState?.StateWeight < mainState?.StateWeight)
        {
            SetSubState(ACTION_STATE.COMMON_UPPER_EMPTY, STATE_SWITCH_BY.FORCED);
        }
        mainState?.Enter();
    }

    public virtual bool SetState(ACTION_STATE targetState, STATE_SWITCH_BY mode, float duration = 0f)
    {
        if (stateDictionary.ContainsKey(targetState))
        {
            switch(mode)
            {
                case STATE_SWITCH_BY.WEIGHT:
                    if (stateDictionary[targetState].StateWeight > mainState?.StateWeight)
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
    public virtual bool SetStateNotInTransition(int currentNameHash, ACTION_STATE targetState, int targetLayer = 0)
    {
        if (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == currentNameHash
            && !animator.IsInTransition(targetLayer))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }

    // !! 해당 Animation의 특정 구간을 만족하고, Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public virtual bool SetStateByAnimationTimeUpTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime, int targetLayer = 0)
    {
        if (animator.IsAnimationNormalizeTimeUpTo(currentNameHash, normalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public virtual bool SetStateByAnimationTimeDownTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime, int targetLayer = 0)
    {
        if (animator.IsAnimationNormalizeTimeDownTo(currentNameHash, normalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public virtual bool SetStateByAnimationTimeBetweenTo(int currentNameHash, ACTION_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime, int targetLayer = 0)
    {
        if (animator.IsAnimationNormalizeTimeBetweenTo(currentNameHash, lowerNormalizedTime, upperNormalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    #endregion

    #region Sub State Functions
    public void SwitchSubState(ACTION_STATE targetState, float duration = 0f)
    {
        subState?.Exit();
        subState = stateDictionary[targetState];
        if (subState is IDurationState lifetimeState)
        {
            lifetimeState.SetDuration(duration);
        }
        subState?.Enter();
    }

    public virtual bool SetSubState(ACTION_STATE targetState, STATE_SWITCH_BY mode, float duration = 0f)
    {
        if (stateDictionary.ContainsKey(targetState))
        {
            switch (mode)
            {
                case STATE_SWITCH_BY.WEIGHT:
                    if (stateDictionary[targetState].StateWeight > subState?.StateWeight)
                    {
                        SwitchSubState(targetState, duration);
                        return true;
                    }
                    return false;

                case STATE_SWITCH_BY.FORCED:
                    SwitchSubState(targetState, duration);
                    return true;
            }
        }
        return false;
    }
    // Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public bool SetSubStateNotInTransition(int currentNameHash, ACTION_STATE targetState, int targetLayer = 1)
    {
        if (animator.GetCurrentAnimatorStateInfo(targetLayer).shortNameHash == currentNameHash
            && !animator.IsInTransition(targetLayer))
        {
            return SetSubState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }

    // !! 해당 Animation의 특정 구간을 만족하고, Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public bool SetSubStateByAnimationTimeUpTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime, int targetLayer = 1)
    {
        if (animator.IsAnimationNormalizeTimeUpTo(currentNameHash, normalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetSubState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public bool SetSubStateByAnimationTimeDownTo(int currentNameHash, ACTION_STATE targetState, float normalizedTime, int targetLayer = 1)
    {
        if (animator.IsAnimationNormalizeTimeDownTo(currentNameHash, normalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetSubState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    public bool SetSubStateByAnimationTimeBetweenTo(int currentNameHash, ACTION_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime, int targetLayer = 1)
    {
        if (animator.IsAnimationNormalizeTimeBetweenTo(currentNameHash, lowerNormalizedTime, upperNormalizedTime, targetLayer) && !animator.IsInTransition(targetLayer))
        {
            return SetSubState(targetState, STATE_SWITCH_BY.FORCED);
        }
        return false;
    }
    #endregion

    #region Property
    public Dictionary<ACTION_STATE, IActionState> StateDictionary { get { return stateDictionary; } }
    public IActionState MainState { get { return mainState; } }
    #endregion
}
