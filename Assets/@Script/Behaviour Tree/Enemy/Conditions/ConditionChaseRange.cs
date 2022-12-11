using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionChaseRange : BehaviourNode
{
    private BaseEnemy enemy;

    public ConditionChaseRange(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (enemy.TargetDistance > enemy.EnemyData.ChaseRange)
        {
            state = NODE_STATE.Success;
        }
        return state;
    }
}
