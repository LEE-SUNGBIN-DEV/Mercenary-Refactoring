using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionHP : BehaviourNode
{
    private Enemy enemy;

    public ConditionHP(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (enemy.EnemyData.CurrentHP <= 0)
        {
            state = NODE_STATE.Success;
        }

        return state;
    }
}