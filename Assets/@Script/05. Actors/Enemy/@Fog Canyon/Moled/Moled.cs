using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moled : BaseEnemy, ICompetable
{
    public override void InitializeEnemy(int enemyID)
    {
        base.InitializeEnemy(enemyID);

        // Common
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));

        // Default
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new EnemyStateSpawn(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WAIT, new EnemyStateChaseWait(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WALK, new EnemyStateChaseWalk(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_RUN, new EnemyStateChaseRun(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SLIDE, new EnemyStateSlide(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_FALL, new EnemyStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_LANDING, new EnemyStateLanding(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_TURN, new EnemyStateTurn(this));

        // Stagger
        state.StateDictionary.Add(ACTION_STATE.ENEMY_STAGGER, new EnemyStateStagger(this));

        // Compete State
        state.StateDictionary.Add(ACTION_STATE.ENEMY_COMPETE, new EnemyStateCompete(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_COMPETE_SUCCESS, new EnemyStateCompeteSuccess(this));

        // Initialize Audio Sources
        spawnAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Spawn"
        };

        dieAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Die"
        };

        attackAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Attack_01",
            "Audio_Big_Golem_Attack_02",
            "Audio_Big_Golem_Attack_03",
            "Audio_Big_Golem_Attack_04",
            "Audio_Big_Golem_Attack_05",
        };

        footstepAudioClipNames = new string[]
        {
            "Audio_Big_Golem_Footstep_01",
            "Audio_Big_Golem_Footstep_02",
            "Audio_Big_Golem_Footstep_03",
            "Audio_Big_Golem_Footstep_04",
            "Audio_Big_Golem_Footstep_05",
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
