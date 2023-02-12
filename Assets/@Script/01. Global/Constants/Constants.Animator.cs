using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    #region Animator String Hash
    // Character
    public static readonly int ANIMATION_NAME_IDLE = Animator.StringToHash("Idle");
    public static readonly int ANIMATION_NAME_WALK = Animator.StringToHash("Walk");
    public static readonly int ANIMATION_NAME_RUN = Animator.StringToHash("Run");
    public static readonly int ANIMATION_NAME_SKILL_COUNTER = Animator.StringToHash("Skill_Counter");
    public static readonly int ANIMATION_NAME_ROLL = Animator.StringToHash("Roll");

    public static readonly int ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK = Animator.StringToHash("isComboAttack");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK = Animator.StringToHash("isSmashAttack");

    public static readonly int ANIMATOR_PARAMETERS_BOOL_DEFENSE = Animator.StringToHash("isDefense");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_BREAKED = Animator.StringToHash("doBreaked");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_PARRYING = Animator.StringToHash("doParrying");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK = Animator.StringToHash("isParryingAttack");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_STAND_ROLL = Animator.StringToHash("doStandRoll");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COUNTER = Animator.StringToHash("doCounter");

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
