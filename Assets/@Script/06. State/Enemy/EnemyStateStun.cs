using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStun : IActionState, IDurationState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private float duration;
    private int animationNameHash;

    public EnemyStateStun(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_STUN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_STUN;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        if (duration <= 0f)
            enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);

        duration -= Time.deltaTime;
    }

    public void Exit()
    {
        duration = 0f;
    }

    public void SetDuration(float duration = 0)
    {
        this.duration = duration;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    public float Duration { get { return duration; } }
    #endregion
}
