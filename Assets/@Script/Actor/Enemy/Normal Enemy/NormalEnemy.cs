using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : BaseEnemy, ILightHitable, IHeavyHitable
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

    public virtual void Update()
    {
        UpdateTargetInformation();
        state.Update();
    }

    #region Override Function   
    public virtual void OnLightHit()
    {
        state.TryStateSwitchingByWeight(ENEMY_STATE.Light_Hit);
    }

    public virtual void OnHeavyHit()
    {
        state.TryStateSwitchingByWeight(ENEMY_STATE.Heavy_Hit);
    }

    public virtual void OnStun(float duration)
    {
    }

    public override void OnDie()
    {
        IsDie = true;
        state.TryStateSwitchingByWeight(ENEMY_STATE.Die);

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion
}
