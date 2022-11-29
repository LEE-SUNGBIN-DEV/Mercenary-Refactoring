using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class CheckMoveRange : BehaviourNode
{
    private Enemy enemy;
    private float distanceFromTarget;

    public CheckMoveRange(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        distanceFromTarget = (enemy.TargetTransform.position - enemy.transform.position).magnitude;
        if (distanceFromTarget > enemy.EnemyData.MoveRange)
        {
            state = NODE_STATE.Success;
        }

        return state;
    }
}
