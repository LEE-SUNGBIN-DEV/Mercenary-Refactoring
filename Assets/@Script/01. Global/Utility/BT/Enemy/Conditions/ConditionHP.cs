using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionHP : BehaviourNode
{
    private BaseEnemy enemy;

    public ConditionHP(BaseEnemy enemy)
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