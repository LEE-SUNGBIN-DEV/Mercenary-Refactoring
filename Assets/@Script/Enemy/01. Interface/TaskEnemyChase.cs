using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyChase : BehaviourNode
{
    private Enemy enemy;
    private float distanceFromTarget;
    private Vector3 targetDirection;

    public TaskEnemyChase(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void LookTarget()
    {
        targetDirection = (enemy.TargetTransform.position - enemy.transform.position).normalized;
        enemy.transform.rotation
                = Quaternion.Lerp(enemy.transform.rotation, Quaternion.LookRotation(targetDirection), 5f * Time.deltaTime);
    }

    public override NODE_STATE Evaluate()
    {
        // Condition 1
        if (enemy.TargetTransform == null)
        {
            state = NODE_STATE.FAILTURE;
            return state;
        }

        // Condition 2
        distanceFromTarget = (enemy.TargetTransform.position - enemy.transform.position).magnitude;
        if (distanceFromTarget <= enemy.TraceRange)
        {
            state = NODE_STATE.FAILTURE;
            return state;
        }
        else
        {
            LookTarget();
            enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
            enemy.Animator.SetBool("isMove", true);
            return NODE_STATE.RUNNING;
        }
    }
}
