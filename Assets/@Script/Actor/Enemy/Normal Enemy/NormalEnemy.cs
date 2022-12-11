using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : BaseEnemy
{
    private EnemySkill[] monsterSkillArray;

    public override void Awake()
    {
        base.Awake();
        monsterSkillArray = GetComponents<EnemySkill>();

        skillDictionary = new Dictionary<int, EnemySkill>();
        for (int i = 0; i < monsterSkillArray.Length; ++i)
        {
            skillDictionary.Add(i, monsterSkillArray[i]);
        }
    }

    private void Update()
    {
    }

    #region Override Function   
    public override void OnHit()
    {
        Animator.SetTrigger("doHit");
    }

    public override void OnHeavyHit()
    {
        Animator.SetTrigger("doHit");
    }

    public override void OnStun()
    {
        Animator.SetTrigger("doHit");
    }

    public override void Die()
    {
        IsDie = true;
        Animator.SetTrigger("doDie");

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion
}
