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
        Debug.Log("Task Idle");
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.SetBool("isMove", false);
        enemy.State = ENEMY_STATE.Idle;

        return NODE_STATE.Running;
    }
}
