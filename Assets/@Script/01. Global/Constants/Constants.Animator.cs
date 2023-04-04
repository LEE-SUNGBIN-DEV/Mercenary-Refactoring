using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    #region Animator String Hash
    // - Common
    public static readonly int ANIMATION_NAME_HASH_EMPTY = Animator.StringToHash("Empty");
    public static readonly int ANIMATION_NAME_HASH_SPAWN = Animator.StringToHash("Spawn");

    public static readonly int ANIMATION_NAME_HASH_ROLL = Animator.StringToHash("Roll");

    public static readonly int ANIMATION_NAME_HASH_FALL = Animator.StringToHash("Fall");
    public static readonly int ANIMATION_NAME_HASH_LANDING = Animator.StringToHash("Landing");

    public static readonly int ANIMATION_NAME_HASH_LIGHT_HIT = Animator.StringToHash("Light_Hit");
    public static readonly int ANIMATION_NAME_HASH_HEAVY_HIT = Animator.StringToHash("Heavy_Hit");
    public static readonly int ANIMATION_NAME_HASH_HEAVY_HIT_Loop = Animator.StringToHash("Heavy_Hit_Loop");
    public static readonly int ANIMATION_NAME_HASH_STAND_UP = Animator.StringToHash("Stand_Up");
    public static readonly int ANIMATION_NAME_HASH_STAND_ROLL = Animator.StringToHash("Stand_Roll");

    public static readonly int ANIMATION_NAME_HASH_STUN = Animator.StringToHash("Stun");
    public static readonly int ANIMATION_NAME_HASH_DIE = Animator.StringToHash("Die");

    // Player
    public static readonly int ANIMATION_NAME_HASH_PLAYER_DRINK = Animator.StringToHash("Player_Drink");

    // - Halberd
    public static readonly int ANIMATION_NAME_HASH_HALBERD_EQUIP = Animator.StringToHash("Halberd_Equip");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_IDLE =   Animator.StringToHash("Halberd_Idle");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_WALK =   Animator.StringToHash("Halberd_Walk");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_RUN =    Animator.StringToHash("Halberd_Run");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_LIGHT_ATTACK_01 = Animator.StringToHash("Halberd_Light_Attack_01");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_LIGHT_ATTACK_02 = Animator.StringToHash("Halberd_Light_Attack_02");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_LIGHT_ATTACK_03 = Animator.StringToHash("Halberd_Light_Attack_03");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_LIGHT_ATTACK_04 = Animator.StringToHash("Halberd_Light_Attack_04");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_HEAVY_ATTACK_01 = Animator.StringToHash("Halberd_Heavy_Attack_01");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_HEAVY_ATTACK_02 = Animator.StringToHash("Halberd_Heavy_Attack_02");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_HEAVY_ATTACK_03 = Animator.StringToHash("Halberd_Heavy_Attack_03");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_HEAVY_ATTACK_04 = Animator.StringToHash("Halberd_Heavy_Attack_04");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_COUNTER =        Animator.StringToHash("Halberd_Counter");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_GUARD_IN =       Animator.StringToHash("Halberd_Guard_In");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_GUARD_LOOP =     Animator.StringToHash("Halberd_Guard_Loop");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_GUARD_OUT =      Animator.StringToHash("Halberd_Guard_Out");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_GUARD_BREAK =    Animator.StringToHash("Halberd_Guard_Break");

    public static readonly int ANIMATION_NAME_HASH_HALBERD_PARRYING =           Animator.StringToHash("Halberd_Parrying");
    public static readonly int ANIMATION_NAME_HASH_HALBERD_PARRYING_ATTACK =    Animator.StringToHash("Halberd_Parrying_Attack");

    // - Sword Shield
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_EQUIP = Animator.StringToHash("Sword_Shield_Equip");

    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_IDLE =  Animator.StringToHash("Sword_Shield_Idle");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_WALK =  Animator.StringToHash("Sword_Shield_Walk");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_RUN =   Animator.StringToHash("Sword_Shield_Run");

    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_LIGHT_ATTACK_01 = Animator.StringToHash("Sword_Shield_Light_Attack_01");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_LIGHT_ATTACK_02 = Animator.StringToHash("Sword_Shield_Light_Attack_02");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_LIGHT_ATTACK_03 = Animator.StringToHash("Sword_Shield_Light_Attack_03");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_LIGHT_ATTACK_04 = Animator.StringToHash("Sword_Shield_Light_Attack_04");

    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_HEAVY_ATTACK_01 = Animator.StringToHash("Sword_Shield_Heavy_Attack_01");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_HEAVY_ATTACK_02 = Animator.StringToHash("Sword_Shield_Heavy_Attack_02");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_HEAVY_ATTACK_03 = Animator.StringToHash("Sword_Shield_Heavy_Attack_03");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_HEAVY_ATTACK_04 = Animator.StringToHash("Sword_Shield_Heavy_Attack_04");

    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_IN =      Animator.StringToHash("Sword_Shield_Guard_In");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_LOOP =    Animator.StringToHash("Sword_Shield_Guard_Loop");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_OUT =     Animator.StringToHash("Sword_Shield_Guard_Out");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_GUARD_BREAK =   Animator.StringToHash("Sword_Shield_Guard_Break");

    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_PARRYING =          Animator.StringToHash("Sword_Shield_Parrying");
    public static readonly int ANIMATION_NAME_HASH_SWORD_SHIELD_PARRYING_ATTACK =   Animator.StringToHash("Sword_Shield_Parrying_Attack");

    // Enemy

    // Skill
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_STAGGER = Animator.StringToHash("doStagger");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_STAGGER = Animator.StringToHash("isStagger");

    // Common
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_ATTACK_SPEED = Animator.StringToHash("attackSpeed");
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_COMPETE = Animator.StringToHash("competeFloat");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT = Animator.StringToHash("doLightHit");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_HEAVY_HIT = Animator.StringToHash("doHeavyHit");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE = Animator.StringToHash("doCompete");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_SUCCESS = Animator.StringToHash("doCompeteSuccess");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE_FAIL = Animator.StringToHash("doCompeteFail");

    #endregion

    // Character
    public static readonly string ANIMATOR_STATE_NAME_MOVE_BLEND_TREE = "Move Blend Tree";
}
