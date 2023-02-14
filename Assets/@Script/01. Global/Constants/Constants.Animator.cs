using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    #region Animator String Hash
    // Playerable Character
    // Movement
    public static readonly int ANIMATION_NAME_IDLE = Animator.StringToHash("Idle");
    public static readonly int ANIMATION_NAME_WALK = Animator.StringToHash("Walk");
    public static readonly int ANIMATION_NAME_RUN = Animator.StringToHash("Run");

    // Attack
    public static readonly int ANIMATION_NAME_LIGHT_ATTACK_01 = Animator.StringToHash("Light_Attack_01");
    public static readonly int ANIMATION_NAME_LIGHT_ATTACK_02 = Animator.StringToHash("Light_Attack_02");
    public static readonly int ANIMATION_NAME_LIGHT_ATTACK_03 = Animator.StringToHash("Light_Attack_03");
    public static readonly int ANIMATION_NAME_LIGHT_ATTACK_04 = Animator.StringToHash("Light_Attack_04");

    public static readonly int ANIMATION_NAME_HEAVY_ATTACK_01 = Animator.StringToHash("Heavy_Attack_01");
    public static readonly int ANIMATION_NAME_HEAVY_ATTACK_02 = Animator.StringToHash("Heavy_Attack_02");
    public static readonly int ANIMATION_NAME_HEAVY_ATTACK_03 = Animator.StringToHash("Heavy_Attack_03");
    public static readonly int ANIMATION_NAME_HEAVY_ATTACK_04 = Animator.StringToHash("Heavy_Attack_04");

    // Defense
    public static readonly int ANIMATION_NAME_DEFENSE = Animator.StringToHash("Defense");
    public static readonly int ANIMATION_NAME_DEFENSE_LOOP = Animator.StringToHash("Defense_Loop");
    public static readonly int ANIMATION_NAME_DEFENSE_END = Animator.StringToHash("Defense_End");
    public static readonly int ANIMATION_NAME_DEFENSE_BREAK = Animator.StringToHash("Defense_Break");

    public static readonly int ANIMATION_NAME_PARRYING = Animator.StringToHash("Parrying");
    public static readonly int ANIMATION_NAME_PARRYING_ATTACK = Animator.StringToHash("Parrying_Attack");

    // Skill
    public static readonly int ANIMATION_NAME_SKILL_COUNTER = Animator.StringToHash("Skill_Counter");

    // Roll
    public static readonly int ANIMATION_NAME_ROLL = Animator.StringToHash("Roll");

    // Hit
    public static readonly int ANIMATION_NAME_LIGHT_HIT = Animator.StringToHash("Light_Hit");
    public static readonly int ANIMATION_NAME_HEAVY_HIT = Animator.StringToHash("Heavy_Hit");
    public static readonly int ANIMATION_NAME_HEAVY_HIT_Loop = Animator.StringToHash("Heavy_Hit_Loop");
    public static readonly int ANIMATION_NAME_STAND_UP = Animator.StringToHash("Stand_Up");
    public static readonly int ANIMATION_NAME_STAND_ROLL = Animator.StringToHash("Stand_Roll");

    //
    public static readonly int ANIMATOR_PARAMETERS_BOOL_DEFENSE = Animator.StringToHash("isDefense");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_BREAKED = Animator.StringToHash("doBreaked");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_PARRYING = Animator.StringToHash("doParrying");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK = Animator.StringToHash("isParryingAttack");

    // Enemy
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_BIRTH = Animator.StringToHash("doBirth");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_MOVE = Animator.StringToHash("isMove");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_ATTACK = Animator.StringToHash("doAttack");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_STAGGER = Animator.StringToHash("doStagger");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_STAGGER = Animator.StringToHash("isStagger");

    // Common
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED = Animator.StringToHash("attackSpeed");
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_COMPETE = Animator.StringToHash("competeFloat");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT = Animator.StringToHash("doLightHit");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_HEAVY_HIT = Animator.StringToHash("doHeavyHit");

    public static readonly int ANIMATOR_PARAMETERS_BOOL_STUN = Animator.StringToHash("isStun");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_DOWN = Animator.StringToHash("isDown");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_DIE = Animator.StringToHash("doDie");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE = Animator.StringToHash("doCompete");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_SUCCESS = Animator.StringToHash("doCompeteSuccess");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_FAIL = Animator.StringToHash("doCompeteFail");

    // UI

    #endregion

    #region Animator State Name
    // Character
    public static readonly string ANIMATOR_STATE_NAME_MOVE_BLEND_TREE = "Move Blend Tree";

    public static readonly string ANIMATOR_STATE_NAME_COMBO_1 = "Combo 1";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_2 = "Combo 2";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_3 = "Combo 3";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_4 = "Combo 4";

    public static readonly string ANIMATOR_STATE_NAME_SMASH_1 = "Smash 1";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_2 = "Smash 2";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_3 = "Smash 3";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_4 = "Smash 4";

    public static readonly string ANIMATOR_STATE_NAME_DEFENSE_BREAKED = "Defense Breaked";
    public static readonly string ANIMATOR_STATE_NAME_PARRYING = "Parrying";
    public static readonly string ANIMATOR_STATE_NAME_PARRYING_ATTACK = "Parrying Attack";

    // Enemy
    public static readonly string ANIMATOR_STATE_NAME_IDLE = "Idle";
    #endregion

}
