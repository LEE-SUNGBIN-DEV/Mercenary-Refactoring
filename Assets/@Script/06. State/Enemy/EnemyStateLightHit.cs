using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLightHit : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateLightHit(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_HIT_LIGHT;
        animationNameHash = Constants.ANIMATION_NAME_HASH_LIGHT_HIT;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationNameHash);
    }

    public void Update()
    {
        if (enemy.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.ENEMY_IDLE, 1.0f))
        {
            return;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
