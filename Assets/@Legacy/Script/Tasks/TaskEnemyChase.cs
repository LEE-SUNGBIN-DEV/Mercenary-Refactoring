using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskEnemyChase : BehaviourNode
{
    private BaseEnemy enemy;

    public TaskEnemyChase(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        enemy.State.TrySwitchState(ENEMY_STATE.Move);
        state = NODE_STATE.Running;

        return state;
    }
}
