using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSlide : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

    public EnemyStateSlide(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.PLAYER_SLIDE;
        animationClipInfo = enemy.AnimationClipTable[Constants.ANIMATION_NAME_SLIDE];
    }

    public void Enter()
    {
        enemy.Animator.Play(animationClipInfo.nameHash);
    }

    public void Update()
    {
        switch (enemy.MoveController.MoveState)
        {
            case MOVE_STATE.GROUNDING:
            case MOVE_STATE.FLOATING:
                enemy.State.SetState(ACTION_STATE.ENEMY_LANDING, STATE_SWITCH_BY.WEIGHT);
                return;

            case MOVE_STATE.SLIDING:
                enemy.MoveController.SlideTime += Time.deltaTime;
                return;

            case MOVE_STATE.FALLING:
                enemy.State.SetState(ACTION_STATE.ENEMY_FALL, STATE_SWITCH_BY.WEIGHT);
                return;

            default:
                break;
        }
    }

    public void Exit()
    {
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
