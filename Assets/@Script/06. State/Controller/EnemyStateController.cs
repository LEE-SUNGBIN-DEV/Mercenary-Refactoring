using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController
{
    protected IEnemyState prevState;
    protected IEnemyState currentState;
    protected Dictionary<ENEMY_STATE, IEnemyState> stateDictionary;
    protected BaseEnemy enemy;

    public EnemyStateController(BaseEnemy enemy)
    {
        this.enemy = enemy;

        stateDictionary = new Dictionary<ENEMY_STATE, IEnemyState>
        {
            // Common
            { ENEMY_STATE.Spawn, new EnemyStateSpawn() },

            { ENEMY_STATE.Idle, new EnemyStateIdle() },
            { ENEMY_STATE.Patrol, new EnemyStatePatrol() },
            { ENEMY_STATE.Chase, new EnemyStateChase() },

            { ENEMY_STATE.Skill, new EnemyStateSkill() },

            { ENEMY_STATE.Light_Hit, new EnemyStateLightHit() },
            { ENEMY_STATE.Heavy_Hit, new EnemyStateHeavyHit() },
            { ENEMY_STATE.Stagger, new EnemyStateStagger() },

            { ENEMY_STATE.Compete, new EnemyStateCompete() },
            { ENEMY_STATE.Die, new EnemyStateDie() }
        };

        currentState = stateDictionary[ENEMY_STATE.Idle];
    }

    public void Update()
    {
        currentState?.Update(enemy);
    }

    public void SetState(ENEMY_STATE targetState)
    {
        prevState = currentState;
        currentState?.Exit(enemy);
        currentState = stateDictionary[targetState];
        currentState?.Enter(enemy);
    }

    public void TryStateSwitchingByWeight(ENEMY_STATE targetState)
    {
        if (IsUpperStateThanCurrentState(targetState))
            SetState(targetState);
    }

    public ENEMY_STATE CompareStateWeight(ENEMY_STATE targetStateA, ENEMY_STATE targetStateB)
    {
        return stateDictionary[targetStateA].StateWeight > stateDictionary[targetStateB].StateWeight ? targetStateA : targetStateB;
    }

    public bool IsUpperStateThanCurrentState(ENEMY_STATE targetState)
    {
        return (currentState.StateWeight < stateDictionary[targetState].StateWeight);
    }

    public bool IsCurrentState(ENEMY_STATE targetState)
    {
        return currentState == stateDictionary[targetState];
    }

    public bool IsPrevState(ENEMY_STATE targetState)
    {
        return prevState == stateDictionary[targetState];
    }

    public bool SetStateNotInTransition(int currentNameHash, ENEMY_STATE targetState)
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && !enemy.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    public bool SetStateByUpperAnimationTime(int currentNameHash, ENEMY_STATE targetState, float normalizedTime)
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime
            && !enemy.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public bool SetStateByLowerAnimationTime(int currentNameHash, ENEMY_STATE targetState, float normalizedTime)
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= normalizedTime
            && !enemy.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }
    public bool SetStateByBetweenAnimationTime(int currentNameHash, ENEMY_STATE targetState, float lowerNormalizedTime, float upperNormalizedTime)
    {
        if (enemy.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentNameHash
            && enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= lowerNormalizedTime
            && enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= upperNormalizedTime
            && !enemy.Animator.IsInTransition(0))
        {
            SetState(targetState);
            return true;
        }
        return false;
    }

    #region Property
    public Dictionary<ENEMY_STATE, IEnemyState> StateDictionary { get { return stateDictionary; } }
    public IEnemyState PrevState { get { return prevState; } }
    public IEnemyState CurrentState { get { return currentState; } }
    #endregion
}
