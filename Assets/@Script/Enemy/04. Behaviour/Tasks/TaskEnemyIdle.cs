using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyIdle : BehaviourNode
{
    private Enemy enemy;

    public TaskEnemyIdle(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.SetBool("isMove", false);
        return NODE_STATE.Running;
    }
}
