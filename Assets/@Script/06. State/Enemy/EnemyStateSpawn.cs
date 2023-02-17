using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSpawn : IEnemyState
{
    private int stateWeight;
    private int animationNameHash;

    public EnemyStateSpawn()
    {
        stateWeight = (int)ENEMY_STATE_WEIGHT.Spawn;
        animationNameHash = Constants.ANIMATION_NAME_HASH_SPAWN;
    }

    public void Enter(BaseEnemy enemy)
    {
        enemy.Animator.Play(animationNameHash);
        enemy.IsInvincible = true;
    }

    public void Update(BaseEnemy enemy)
    {
        if (enemy.State.SetStateByUpperAnimationTime(animationNameHash, ENEMY_STATE.Idle, 1.0f))
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
