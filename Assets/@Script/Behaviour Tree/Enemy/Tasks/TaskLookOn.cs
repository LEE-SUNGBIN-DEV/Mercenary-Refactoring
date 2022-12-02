using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskLookOn : BehaviourNode
{
    private Enemy enemy;

    public TaskLookOn(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        Debug.Log("Task Look On");
        CalculateTargetDistance();
        LookTarget();

        return NODE_STATE.Success;
    }

    public void CalculateTargetDistance()
    {
        enemy.TargetDistance = (enemy.TargetTransform.position - enemy.transform.position).magnitude;
    }
    public void LookTarget()
    {
        enemy.TargetDirection = (enemy.TargetTransform.position - enemy.transform.position).normalized;
        enemy.transform.rotation
                = Quaternion.Lerp(enemy.transform.rotation, Quaternion.LookRotation(enemy.TargetDirection), 2f * Time.deltaTime);
    }

    
}
