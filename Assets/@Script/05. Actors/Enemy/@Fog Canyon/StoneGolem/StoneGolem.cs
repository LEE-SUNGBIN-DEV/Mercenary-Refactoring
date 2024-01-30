using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneGolem : BaseEnemy, IStunable
{
    protected override void Awake()
    {
        base.Awake();
        state.StateDictionary.Add(ACTION_STATE.COMMON_UPPER_EMPTY, new CommonStateUpperEmpty(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SPAWN, new EnemyStateSpawn(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_PATROL, new EnemyStatePatrol(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WAIT, new EnemyStateChaseWait(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_WALK, new EnemyStateChaseWalk(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_CHASE_RUN, new EnemyStateChaseRun(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_SKILL, new EnemyStateSkill(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_LIGHT, new EnemyStateLightHit(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_HIT_HEAVY, new EnemyStateHeavyHit(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_STUN, new EnemyStateStun(this));

        state.StateDictionary.Add(ACTION_STATE.ENEMY_SLIDE, new EnemyStateSlide(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_FALL, new EnemyStateFall(this));
        state.StateDictionary.Add(ACTION_STATE.ENEMY_LANDING, new EnemyStateLanding(this));

        // Initialize Audio Sources
        spawnAudioClipNames = new string[]
        {
            //"Audio_Small_Golem_Spawn"
        };
        attackAudioClipNames = new string[]
        {
            "Audio_Small_Golem_Attack_01",
            "Audio_Small_Golem_Attack_02",
            "Audio_Small_Golem_Attack_03",
            "Audio_Small_Golem_Attack_04",
            "Audio_Small_Golem_Attack_05",
        };
        lightHitAudioClipNames = new string[]
        {
            "Audio_Golem_Light_Hit_01",
            "Audio_Golem_Light_Hit_02",
        };
        heavyHitAudioClipNames = new string[]
        {
            "Audio_Golem_Heavy_Hit_01",
            "Audio_Golem_Heavy_Hit_02",
        };

        dieAudioClipNames = new string[]
        {
            "Audio_Small_Golem_Die"
        };

        footstepAudioClipNames = new string[]
        {
            "Audio_Small_Golem_Footstep_01",
            "Audio_Small_Golem_Footstep_02",
            "Audio_Small_Golem_Footstep_03",
            "Audio_Small_Golem_Footstep_04",
            "Audio_Small_Golem_Footstep_05",
        };
    }

    public override void Update()
    {
        base.Update();
    }

    #region Override Function   
    public override void OnLightHit()
    {
        state?.SetState(ACTION_STATE.ENEMY_HIT_LIGHT, STATE_SWITCH_BY.WEIGHT);
    }

    public override void OnHeavyHit()
    {
        state?.SetState(ACTION_STATE.ENEMY_HIT_HEAVY, STATE_SWITCH_BY.WEIGHT);
    }

    public virtual void OnStun(float duration)
    {
        state?.SetState(ACTION_STATE.ENEMY_STUN, STATE_SWITCH_BY.WEIGHT, duration);
    }
    #endregion
}
