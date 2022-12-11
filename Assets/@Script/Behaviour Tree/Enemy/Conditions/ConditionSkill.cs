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
        enemy.SkillIndex = Random.Range(0, enemy.SkillDictionary.Count);
        if (enemy.SkillDictionary[enemy.SkillIndex].CheckCondition(enemy.TargetDistance))
        {
            state = NODE_STATE.Success;
        }
        return state;
    }
}
