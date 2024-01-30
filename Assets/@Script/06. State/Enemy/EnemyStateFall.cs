using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFall : IActionState
{
    private BaseEnemy enemy;
    private int stateWeight;
    private AnimationClipInfo animationClipInfo;

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
                enemy.MoveController.FallTime += Time.deltaTime;
                return;

            default:
                break;
        }
    }

    public void Exit()
    {
        fallTime = 0f;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }
    #endregion
}
