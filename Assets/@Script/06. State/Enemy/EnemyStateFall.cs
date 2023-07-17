using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFall : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInformation animationClipInfo;

    private float fallTime;

    public EnemyStateFall(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_FALL;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_FALL];

        fallTime = 0f;
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInfo.nameHash);
        fallTime = 0f;
    }

    public void Update()
    {
        switch (enemy.MoveController.GroundState)
        {
            case ACTOR_GROUND_STATE.GROUNDING:
                enemy.State.SetState(ACTION_STATE.ENEMY_LANDING, STATE_SWITCH_BY.WEIGHT);
                return;

            case ACTOR_GROUND_STATE.FLOATING:
                return;

            case ACTOR_GROUND_STATE.SLIDING:
                enemy.MoveController.SlideTime += Time.deltaTime;
                return;
            case ACTOR_GROUND_STATE.FALLING:
                enemy.MoveController.FallTime += Time.deltaTime;
                return;
        }

        if (fallTime > 2.5f)
            enemy.Status.CurrentHP = 0f;
    }

    public void Exit()
    {
        fallTime = 0f;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
