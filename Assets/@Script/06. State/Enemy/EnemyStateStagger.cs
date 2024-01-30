using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStagger : IActionState, IDurationState
{
    private enum STAGGER_MODE
    {
        IN,
        LOOP,
        OUT
    }
    private BaseEnemy enemy;
    private int stateWeight;

    private AnimationClipInfo staggerInClipInfo;
    private AnimationClipInfo staggerLoopClipInfo;
    private AnimationClipInfo staggerOutClipInfo;

    private STAGGER_MODE staggerMode;
    private float duration;

    public EnemyStateStagger(BaseEnemy enemy)
    {
        this.enemy = enemy;
        stateWeight = (int)ACTION_STATE_WEIGHT.ENEMY_STAGGER;

        staggerInClipInfo = enemy.AnimationClipTable["Stagger_In"];
        staggerLoopClipInfo = enemy.AnimationClipTable["Stagger_Loop"];
        staggerOutClipInfo = enemy.AnimationClipTable["Stagger_Out"];

        duration = 5f;
    }

    public void Enter()
    {
        enemy.TryPlaySFXFromStringArray(enemy.StaggerAudioClipNames);
        enemy.Animator.Play(staggerInClipInfo.nameHash);
        staggerMode = STAGGER_MODE.IN;
    }

    public void Update()
    {
        duration -= Time.deltaTime;
        switch (staggerMode)
        {
            case STAGGER_MODE.IN:
                if (enemy.Animator.IsAnimationFrameUpTo(staggerInClipInfo, staggerInClipInfo.maxFrame))
                {
                    enemy.Animator.Play(staggerLoopClipInfo.nameHash);
                    staggerMode = STAGGER_MODE.LOOP;
                }
                break;

            case STAGGER_MODE.LOOP:
                if (duration <= 0)
                {
                    enemy.Animator.Play(staggerOutClipInfo.nameHash);
                    staggerMode = STAGGER_MODE.OUT;
                }
                break;

            case STAGGER_MODE.OUT:
                if (enemy.Animator.IsAnimationFrameUpTo(staggerOutClipInfo, staggerOutClipInfo.maxFrame))
                {
                    enemy.State.SetState(ACTION_STATE.ENEMY_IDLE, STATE_SWITCH_BY.FORCED);
                }
                break;
        }
    }

    public void Exit()
    {
    }

    public void SetDuration(float duration = 0)
    {
        this.duration = duration;
    }

    #region Property
    public int StateWeight { get { return stateWeight; } }

    public float Duration => throw new System.NotImplementedException();
    #endregion
}
