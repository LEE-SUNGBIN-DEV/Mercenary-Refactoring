using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFSM<T> where T: BaseActor
{
    protected IActionState<T> prevState;
    protected IActionState<T> currentState;
    protected Dictionary<ACTION_STATE, IActionState<T>> stateDictionary;
    protected T actor;

    public BaseFSM(T actor)
    {
        this.actor = actor;
        stateDictionary = new Dictionary<ACTION_STATE, IActionState<T>>();
    }

    public virtual void Update()
    {
        currentState?.Update(actor);
    }

    // 상태 전환
    public virtual void SetState(ACTION_STATE targetState)
    {
        prevState = currentState;
        currentState?.Exit(actor);
        currentState = stateDictionary[targetState];
        currentState?.Enter(actor);
    }

    // 상태 가중치에 따라 강제 전환이 필요한 경우
    public virtual void TryStateSwitchingByWeight(ACTION_STATE targetState)
    {
        if (stateDictionary[targetState].StateWeight > currentState?.StateWeight)
            SetState(targetState);
    }

    public virtual ACTION_STATE CompareStateWeight(ACTION_STATE targetStateA, ACTION_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    public virtual bool IsCurrentState(ACTION_STATE targetState)
    {
        return currentState == stateDictionary[targetState];
    }

    public virtual bool IsPrevState(ACTION_STATE targetState)
    {
        return prevState == stateDictionary[targetState];
    }

    // Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public virtual bool SetStateNotInTransition(int currentNameHash, ACTION_STATE targetState)
    {
        if (actor.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && !actor.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    // !! 해당 Animation의 특정 구간을 만족하고, Transition 되는 동안 실행을 방지하면서 상태 전환이 필요한 경우
    public virtual bool SetStateByUpperAnimationTime(int currentNameHash, ACTION_STATE targetState, float normalizedTime)
    {
        if (actor.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && actor.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime
            && !actor.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public virtual bool SetStateByLowerAnimationTime(int currentNameHash, ACTION_STATE targetState, float normalizedTime)
    {
        if (actor.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && actor.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= normalizedTime
            && !actor.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public virtual bool SetStateByBetweenAnimationTime(int currentNameHash, ACTION_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime)
    {
        if (actor.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && actor.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= lowerNormalizedTime
            && actor.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= upperNormalizedTime
            && !actor.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    #region Property
    public Dictionary<ACTION_STATE, IActionState<T>> StateDictionary { get { return stateDictionary; } }
    public IActionState<T> PrevState { get { return prevState; } }
    public IActionState<T> CurrentState { get { return currentState; } }
    #endregion
}
