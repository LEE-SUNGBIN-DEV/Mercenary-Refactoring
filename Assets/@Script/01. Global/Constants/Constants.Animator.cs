using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    #region Animation Clip Name
    // Enemy
    public static readonly string ANIMATION_NAME_EMPTY = "Empty";
    public static readonly string ANIMATION_NAME_UPPER_EMPTY = "Upper_Empty";

    public static readonly string ANIMATION_NAME_IDLE = "Idle";
    public static readonly string ANIMATION_NAME_WALK = "Walk";
    public static readonly string ANIMATION_NAME_RUN = "Run";

    public static readonly string ANIMATION_NAME_HEAVY_HIT = "Heavy_Hit";
    public static readonly string ANIMATION_NAME_LIGHT_HIT = "Light_Hit";
    public static readonly string ANIMATION_NAME_STUN = "Stun";

    public static readonly string ANIMATION_NAME_SLIDE = "Slide";
    public static readonly string ANIMATION_NAME_FALL = "Fall";
    public static readonly string ANIMATION_NAME_LANDING = "Landing";

    public static readonly string ANIMATION_NAME_SPAWN = "Spawn";
    public static readonly string ANIMATION_NAME_DIE = "Die";
    #endregion

    #region Animation Name Hash
    public static readonly int ANIMATION_HASH_UPPER_EMPTY = Animator.StringToHash("Upper_Empty");
    #endregion

    #region Animator Parameter Hash
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
