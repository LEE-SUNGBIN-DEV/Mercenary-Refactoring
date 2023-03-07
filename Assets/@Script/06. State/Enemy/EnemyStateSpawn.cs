using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSpawn : IActionState<BaseEnemy>
{
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateSpawn()
    {
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_SPAWN;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SPAWN;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.Play(animationNameHash);
        enemy.IsInvincible = true;
    }

    public void Update(BaseEnemy enemy)
    {
        if (enemy.State.SetStateByAnimationTimeUpTo(animationNameHash, ACTION_STATE.ENEMY_IDLE, 1.0f))
        {
            return;
        }
    }

    public void Exit(BaseEnemy enemy)
    {
        enemy.IsInvincible = false;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
