using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyWait : BehaviourNode
{
    private Enemy enemy;

    public TaskEnemyWait(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        Debug.Log("Task Wait");
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.velocity = Vector3.zero;
        enemy.Animator.SetBool("isMove", false);

        return NODE_STATE.Running;
    }
}