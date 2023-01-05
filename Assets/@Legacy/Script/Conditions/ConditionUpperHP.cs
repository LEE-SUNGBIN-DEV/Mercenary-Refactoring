using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionUpperHP : BehaviourNode
{
    private BaseEnemy enemy;
    private float targetHP;

    public ConditionUpperHP(BaseEnemy enemy, float targetHP)
    {
        this.enemy = enemy;
        this.targetHP = targetHP;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;

        if (enemy.EnemyData.CurrentHP >= targetHP)
            state = NODE_STATE.Success;

        return state;
    }
}
