using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionTarget : BehaviourNode
{
    private Enemy enemy;

    public ConditionTarget(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (enemy.TargetTransform != null)
        {
            state = NODE_STATE.Success;
        }

        return state;
    }
}
