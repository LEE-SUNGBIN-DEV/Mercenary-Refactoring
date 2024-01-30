using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moled : BaseEnemy, ICompetable
{
    protected override void Awake()
    {
        base.Awake();

        // Moled State
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new MoledStateSpawn(this));

        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WAIT, new EnemyStateChaseWait(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WALK, new EnemyStateChaseWalk(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_RUN, new EnemyStateChaseRun(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SLIDE, new EnemyStateSlide(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_FALL, new EnemyStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_LANDING, new EnemyStateLanding(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_TURN, new EnemyStateTurn(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_STAGGER, new EnemyStateStagger(this));

        // Compete State
        state.StateDictionary.Add(ACTION_STATE.ENEMY_COMPETE, new EnemyStateCompete(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_COMPETE_SUCCESS, new EnemyStateCompeteSuccess(this));

        // Initialize Audio Sources
        spawnAudioClipNames = new string[]
        {
            Constants.Audio_Big_Golem_Roar
        };

        dieAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Die"
        };

        footstepAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Footstep_01",
            "Audio_Big_Golem_Footstep_02",
            "Audio_Big_Golem_Footstep_03",
            "Audio_Big_Golem_Footstep_04",
            "Audio_Big_Golem_Footstep_05",
        };

        staggerAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Stagger"
        };
    }

    public override void Update()
    {
        base.Update();
    }

    #region Override Function   
    public override void OnLightHit()
    {
    }

    public override void OnHeavyHit()
    {
    }

    public virtual void OnStun(float duration)
    {
    }

    public void OnCompete()
    {
        state.SetState(ACTION_STATE.ENEMY_COMPETE, STATE_SWITCH_BY.WEIGHT);
    }
    #endregion

}
