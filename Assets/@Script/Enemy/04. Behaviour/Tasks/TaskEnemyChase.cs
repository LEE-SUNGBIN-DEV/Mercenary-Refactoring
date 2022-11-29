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
        LookTarget();
        enemy.NavMeshAgent.SetDestination(enemy.TargetTransform.position);
        enemy.Animator.SetBool("isMove", true);
        return NODE_STATE.Running;
    }
}
