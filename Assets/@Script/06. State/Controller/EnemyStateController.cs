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
            { ENEMY_STATE.Idle, new EnemyStateIdle() },
            { ENEMY_STATE.Move, new EnemyStateMove() },
            { ENEMY_STATE.Skill, new EnemyStateSkill() },

            { ENEMY_STATE.LightHit, new EnemyStateLightHit() },
            { ENEMY_STATE.HeavyHit, new EnemyStateHeavyHit() },
            { ENEMY_STATE.Stagger, new EnemyStateStagger() },

            { ENEMY_STATE.Compete, new EnemyStateCompete() },
            { ENEMY_STATE.Birth, new EnemyStateBirth() },
            { ENEMY_STATE.Die, new EnemyStateDie() }
        };

        currentState = stateDictionary[ENEMY_STATE.Idle];
    }

    public void Update()
    {
        currentState?.Update(enemy);
    }

    public void SwitchState(ENEMY_STATE targetState)
    {
        prevState = currentState;
        currentState?.Exit(enemy);
        currentState = stateDictionary[targetState];
        currentState?.Enter(enemy);
    }

    public void TrySwitchState(ENEMY_STATE targetState)
    {
        if (IsUpperStateThanCurrentState(targetState))
            SwitchState(targetState);
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

    #region Property
    public Dictionary<ENEMY_STATE, IEnemyState> StateDictionary { get { return stateDictionary; } }
    public IEnemyState PrevState { get { return prevState; } }
    public IEnemyState CurrentState { get { return currentState; } }
    #endregion
}
