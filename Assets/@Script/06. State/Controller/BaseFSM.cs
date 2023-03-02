using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFSM<T> where T: BaseActor
{
    protected T actor;
    protected IActionState<T> prevState;
    protected IActionState<T> currentState;
    protected Dictionary<ACTION_STATE, IActionState<T>> stateDictionary;

    public BaseFSM(T actor)
    {
        this.actor = actor;
        stateDictionary = new Dictionary<ACTION_STATE, IActionState<T>>();
    }

    public virtual void Update()
    {
        currentState?.Update(actor);
    }

    #region State Functions
    // ���� ��ȯ
    public virtual void SetState(ACTION_STATE targetState, float duration = 0f)
    {
        prevState = currentState;
        currentState?.Exit(actor);
        currentState = stateDictionary[targetState];
        if(currentState is IDurationState lifetimeState)
        {
            lifetimeState.SetDuration(duration);
        }
        currentState?.Enter(actor);
    }

    // ���� ����ġ�� ���� ��ȯ�� �ʿ��� ���
    public virtual bool TryStateSwitchingByWeight(ACTION_STATE targetState, float duration = 0f)
    {
        if (stateDictionary[targetState].StateWeight > currentState?.StateWeight)
        {
            SetState(targetState, duration);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual ACTION_STATE CompareStateWeight(ACTION_STATE targetStateA, ACTION_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }


    // Transition �Ǵ� ���� ������ �����ϸ鼭 ���� ��ȯ�� �ʿ��� ���
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

    // !! �ش� Animation�� Ư�� ������ �����ϰ�, Transition �Ǵ� ���� ������ �����ϸ鼭 ���� ��ȯ�� �ʿ��� ���
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
    #endregion


    #region Property
    public BaseActor Actor { get { return actor; } }
    public Dictionary<ACTION_STATE, IActionState<T>> StateDictionary { get { return stateDictionary; } }
    public IActionState<T> PrevState { get { return prevState; } }
    public IActionState<T> CurrentState { get { return currentState; } }
    #endregion
}