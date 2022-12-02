using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class TaskSkill : BehaviourNode
{
    private Enemy enemy;

    public TaskSkill(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        Debug.Log("Task Skill");
        enemy.State = ENEMY_STATE.Attack;
        enemy.SkillDictionary[enemy.SkillIndex].ActiveSkill();

        return NODE_STATE.Running;
    }
}
