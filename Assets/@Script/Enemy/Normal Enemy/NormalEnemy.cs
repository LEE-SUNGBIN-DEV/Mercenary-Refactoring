using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : Enemy
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
    public override void Hit()
    {
        if (IsStun || IsDie)
            return;

        IsHit = true;
        Animator.SetTrigger("doHit");
    }

    public override void HeavyHit()
    {
        if (IsStun || IsDie)
            return;
        IsHit = true;
        Animator.SetTrigger("doHit");
    }

    public override void Stun()
    {
        if (IsDie)
            return;
        IsHit = true;
        Animator.SetTrigger("doHit");
    }

    public override void Die()
    {
        InitializeAllState();

        IsDie = true;
        Animator.SetTrigger("doDie");

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    public override void InitializeAllState()
    {
        // Initialize Previous State
        IsHit = false;
        IsHeavyHit = false;
        IsStun = false;

        Animator.SetBool("isMove", false);
    }
    #endregion
}
