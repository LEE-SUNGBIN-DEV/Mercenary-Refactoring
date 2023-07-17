using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class BaseEnemy : BaseActor
{
    [Header("Skills")]
    [SerializeField] protected EnemySkill[] skillArray;
    [SerializeField] protected EnemySkill currentSkill;

    public void InitializeSkill()
    {
        skillArray = GetComponents<EnemySkill>();
        for (int i = 0; i < skillArray.Length; ++i)
        {
            skillArray[i].Initialize(this);
            if (status.StopDistance > skillArray[i].MaxAttackDistance)
                status.StopDistance = skillArray[i].MaxAttackDistance;
        }
    }

    public bool IsReadyAnySkill()
    {
        currentSkill = null;
        for (int i = 0; i < skillArray.Length; ++i)
        {
            if (skillArray[i].IsReady(targetDistance))
            {
                if (currentSkill == null)
                    currentSkill = skillArray[i];

                else
                {
                    if (skillArray[i].Priority > currentSkill.Priority)
                        currentSkill = skillArray[i];
                }
            }
        }
        if (currentSkill != null)
            return true;

        return false;
    }

    public EnemySkill CurrentSkill { get { return currentSkill; } set { currentSkill = value; } }
}
