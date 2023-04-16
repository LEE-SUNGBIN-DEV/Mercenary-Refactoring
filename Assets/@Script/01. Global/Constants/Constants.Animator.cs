using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    #region Animator String Hash
    // Enemy
    public static readonly int ANIMATION_NAME_HASH_EMPTY = Animator.StringToHash("Empty");

    public static readonly int ANIMATION_NAME_HASH_IDLE = Animator.StringToHash("Idle");
    public static readonly int ANIMATION_NAME_HASH_WALK = Animator.StringToHash("Walk");
    public static readonly int ANIMATION_NAME_HASH_RUN = Animator.StringToHash("Run");


    public static readonly int ANIMATION_NAME_HASH_ROLL = Animator.StringToHash("Roll");

    public static readonly int ANIMATION_NAME_HASH_SLIDE = Animator.StringToHash("Slide");
    public static readonly int ANIMATION_NAME_HASH_FALL = Animator.StringToHash("Fall");
    public static readonly int ANIMATION_NAME_HASH_LANDING = Animator.StringToHash("Landing");

    public static readonly int ANIMATION_NAME_HASH_LIGHT_HIT = Animator.StringToHash("Light_Hit");
    public static readonly int ANIMATION_NAME_HASH_HEAVY_HIT = Animator.StringToHash("Heavy_Hit");
    public static readonly int ANIMATION_NAME_HASH_HEAVY_HIT_LOOP = Animator.StringToHash("Heavy_Hit_Loop");
    public static readonly int ANIMATION_NAME_HASH_STAND_UP = Animator.StringToHash("Stand_Up");
    public static readonly int ANIMATION_NAME_HASH_STAND_ROLL = Animator.StringToHash("Stand_Roll");

    public static readonly int ANIMATION_NAME_HASH_STUN = Animator.StringToHash("Stun");
    public static readonly int ANIMATION_NAME_HASH_DIE = Animator.StringToHash("Die");
    public static readonly int ANIMATION_NAME_HASH_SPAWN = Animator.StringToHash("Spawn");

    // Skill
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_STAGGER = Animator.StringToHash("doStagger");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_STAGGER = Animator.StringToHash("isStagger");

    // Common
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED = Animator.StringToHash("attackSpeed");
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_COMPETE = Animator.StringToHash("competeFloat");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE = Animator.StringToHash("doCompete");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_SUCCESS = Animator.StringToHash("doCompeteSuccess");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_FAIL = Animator.StringToHash("doCompeteFail");

    #endregion

    // Character
    public static readonly string ANIMATOR_STATE_NAME_MOVE_BLEND_TREE = "Move Blend Tree";
}
