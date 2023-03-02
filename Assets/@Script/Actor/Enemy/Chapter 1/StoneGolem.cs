using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneGolem : BaseEnemy, IStunable
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    #region Override Function   
    public override void OnLightHit()
    {
        state?.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_HIT_LIGHT);
    }

    public override void OnHeavyHit()
    {
        state?.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_HIT_HEAVY);
    }

    public virtual void OnStun(float duration)
    {
        state?.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_STUN, duration);
    }

    public override void OnDie()
    {
        IsDie = true;
        state?.TryStateSwitchingByWeight(ACTION_STATE.ENEMY_DIE);

        StartCoroutine(WaitForDisapear(Constants.TIME_NORMAL_MONSTER_DISAPEAR));
    }
    #endregion
}
