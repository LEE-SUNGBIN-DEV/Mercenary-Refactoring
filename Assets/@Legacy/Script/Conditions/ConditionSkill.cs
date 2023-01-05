using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreePackage;

public class ConditionSkill : BehaviourNode
{
    private BaseEnemy enemy;

    public ConditionSkill(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public override NODE_STATE Evaluate()
    {
        state = NODE_STATE.Failture;
        foreach(var skill in enemy.SkillDictionary.Values)
        {
            if(skill.CheckCondition(enemy.TargetDistance))
            {
                enemy.SelectSkill = skill;
                state = NODE_STATE.Success;
                return state;
            }
        }

        return state;
    }
}
