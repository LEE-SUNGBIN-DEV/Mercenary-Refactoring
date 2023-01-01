using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionUpperRange : BehaviourNode
{
    private BaseEnemy enemy;
    private float targetRange;

    public ConditionUpperRange(BaseEnemy enemy, float targetRange)
    {
        this.enemy = enemy;
        this.targetRange = targetRange;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        if (enemy.TargetDistance > targetRange)
        {
            state = NODE_STATE.Success;
        }
        return state;
    }
}
