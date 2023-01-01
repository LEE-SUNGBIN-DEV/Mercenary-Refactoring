using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionUpperWeight : BehaviourNode
{
    private BaseEnemy enemy;
    private ENEMY_STATE targetState;

    public ConditionUpperWeight(BaseEnemy enemy, ENEMY_STATE targetState)
    {
        this.enemy = enemy;
        this.targetState = targetState;
    }

    public override NODE_STATE Evaluate()
    {
        if (enemy.State.IsUpperStateThanCurrentState(targetState))
            state = NODE_STATE.Success;
        else
            state = NODE_STATE.Failture;

        return state;
    }
}
