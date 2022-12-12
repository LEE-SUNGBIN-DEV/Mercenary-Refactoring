using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionEnemyState : BehaviourNode
{
    private BaseEnemy enemy;
    private ENEMY_STATE targetState;

    public ConditionEnemyState(BaseEnemy enemy, ENEMY_STATE targetState)
    {
        this.enemy = enemy;
        this.targetState = targetState;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (enemy.State == targetState)
        {
            state = NODE_STATE.Success;
        }

        return state;
    }
}
