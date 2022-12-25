using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    // Inventory
    public static readonly int MAX_INVENTORY_SLOT_NUMBER = 30;
    public static readonly int MAX_QUICK_SLOT_NUMBER = 4;
    public static readonly int MAX_EQUIPMENT_SLOT_NUMBER = 4;

    // Character
    public static readonly float CHARACTER_STAMINA_AUTO_RECOVERY = 15f;
    public static readonly float CHARACTER_STAMINA_CONSUMPTION_RUN = 2f;
    public static readonly float CHARACTER_STAMINA_CONSUMPTION_DEFEND = 2f;
    public static readonly float CHARACTER_STAMINA_CONSUMPTION_ROLL = 15f;
    public static readonly float CHARACTER_STAMINA_CONSUMPTION_COUNTER = 30f;

    // Character Stat
    public static readonly float CHARACTER_STAT_ATTACK_SPEED_MIN = 0.1f;
    public static readonly float CHARACTER_STAT_ATTACK_SPEED_DEFAULT = 1f;
    public static readonly float CHARACTER_STAT_ATTACK_SPEED_MAX = 2f;

    public static readonly float CHARACTER_STAT_MOVE_SPEED_MIN = 0.1f;
    public static readonly float CHARACTER_STAT_MOVE_SPEED_DEFAULT = 3f;
    public static readonly float CHARACTER_STAT_MOVE_SPEED_MAX = 5f;

    public static readonly float CHARACTER_STAT_CRITICAL_CHANCE_MIN = 0f;
    public static readonly float CHARACTER_STAT_CRITICAL_CHANCE_DEFAULT = 0f;
    public static readonly float CHARACTER_STAT_CRITICAL_CHANCE_MAX = 100f;

    public static readonly float CHARACTER_STAT_CRITICAL_DAMAGE_MIN = 100f;
    public static readonly float CHARACTER_STAT_CRITICAL_DAMAGE_DEFAULT = 100f;

    // Character Data
    public static readonly int CHARACTER_DATA_DEFALUT_LEVEL = 1;
    public static readonly int CHARACTER_DATA_MAX_LEVEL = 30;
    public static readonly float CHARACTER_DATA_DEFALUT_EXPERIENCE = 0f;

    public static readonly int CHARACTER_DATA_DEFALUT_STATPOINT = 0;
    public static readonly int CHARACTER_DATA_DEFALUT_STRENGTH = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_VITALITY = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_DEXTERITY = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_LUCK = 10;

    public static readonly int CHARACTER_DATA_DEFAULT_MONEY = 500;
    public static readonly uint CHARACTER_DATA_MAIN_QUEST_PROGRESS = 0;

    // Animator
    public static readonly int ANIMATOR_PARAMETERS_FLOAT_MOVE = Animator.StringToHash("moveFloat");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_ATTACK = Animator.StringToHash("doAttack");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_COMBO_ATTACK = Animator.StringToHash("isComboAttack");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_SMASH_ATTACK = Animator.StringToHash("isSmashAttack");

    public static readonly int ANIMATOR_PARAMETERS_BOOL_DEFENSE = Animator.StringToHash("isDefense");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_BREAKED = Animator.StringToHash("isBreaked");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_PARRYING = Animator.StringToHash("isParrying");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_PARRYING_ATTACK = Animator.StringToHash("isParryingAttack");

    public static readonly int ANIMATOR_PARAMETERS_BOOL_STUN = Animator.StringToHash("isStun");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_LIGHT_HIT = Animator.StringToHash("doLightHit");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_HEAVY_HIT = Animator.StringToHash("doHeavyHit");
    public static readonly int ANIMATOR_PARAMETERS_BOOL_DOWN = Animator.StringToHash("isDown");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_ROLL = Animator.StringToHash("doRoll");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_STAND_ROLL = Animator.StringToHash("doStandRoll");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COUNTER = Animator.StringToHash("doCounter");

    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_COMPETE = Animator.StringToHash("doCompete");
    public static readonly int ANIMATOR_PARAMETERS_TRIGGER_DIE = Animator.StringToHash("doDie");

    public static readonly string ANIMATOR_STATE_NAME_MOVE_BLEND_TREE = "Move Blend Tree";

    public static readonly string ANIMATOR_STATE_NAME_COMBO_1 = "Combo 1";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_2 = "Combo 2";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_3 = "Combo 3";
    public static readonly string ANIMATOR_STATE_NAME_COMBO_4 = "Combo 4";

    public static readonly string ANIMATOR_STATE_NAME_SMASH_1 = "Smash 1";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_2 = "Smash 2";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_3 = "Smash 3";
    public static readonly string ANIMATOR_STATE_NAME_SMASH_4 = "Smash 4";
}