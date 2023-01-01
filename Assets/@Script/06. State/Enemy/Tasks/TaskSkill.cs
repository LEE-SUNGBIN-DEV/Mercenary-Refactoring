using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskSkill : BehaviourNode
{
    private BaseEnemy enemy;

    public TaskSkill(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        enemy.State.TrySwitchState(ENEMY_STATE.Skill);
        state = NODE_STATE.Success;

        return state;
    }
}
