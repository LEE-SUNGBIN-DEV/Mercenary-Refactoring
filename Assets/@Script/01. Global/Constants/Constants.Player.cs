public static partial class Constants
{
    // Inventory
    public static readonly int MAX_INVENTORY_SLOT_NUMBER = 30;
    public static readonly int MAX_QUICK_SLOT_NUMBER = 4;
    public static readonly int MAX_EQUIPMENT_SLOT_NUMBER = 4;
    public static readonly int CHARACTER_DATA_DEFAULT_STONE = 500;

    // Character
    public static readonly float PLAYER_RUN_SPEED_RATIO = 2f;
    public const float PLAYER_DEFAULT_ROTATION_SPEED = 6f;

    public static readonly float PLAYER_ROLL_SPEED = 7f;
    public static readonly float PLAYER_STAND_ROLL_SPEED = 5f;
    public static readonly float PLAYER_HEAVY_HIT_SPEED = 4f;

    public static readonly float PLAYER_STAMINA_IDLE_AUTO_RECOVERY_RATIO = 25f;
    public static readonly float PLAYER_STAMINA_WALK_AUTO_RECOVERY_RATIO = 15f;
    public static readonly float PLAYER_STAMINA_PARRYING_RECOVERY_RATIO = 20f;

    public static readonly float PLAYER_STAMINA_CONSUMPTION_RUN = 2f;

    public static readonly float PLAYER_STAMINA_CONSUMPTION_GUARD_LOOP = 2f;
    public static readonly float PLAYER_STAMINA_CONSUMPTION_GUARD_IN = 5f;
    public static readonly float PLAYER_STAMINA_CONSUMPTION_GUARD_BREAK = 20f;

    public static readonly float PLAYER_STAMINA_CONSUMPTION_ROLL = 15f;
    public static readonly float PLAYER_STAMINA_CONSUMPTION_SKILL_COUNTER = 30f;

    public static readonly float HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01 = 8f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_02 = 7f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_03 = 6f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_LIGHT_ATTACK_04 = 5f;

    public static readonly float HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_01 = 10f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_02 = 9f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_03 = 8f;
    public static readonly float HALBERD_STAMINA_CONSUMPTION_HEAVY_ATTACK_04 = 7f;

    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_01 = 8f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_02 = 7f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_03 = 6f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_LIGHT_ATTACK_04 = 5f;

    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_01 = 10f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_02 = 9f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_03 = 8f;
    public static readonly float SWORD_SHIELD_STAMINA_CONSUMPTION_HEAVY_ATTACK_04 = 7f;

    // Character Default Status
    public static readonly int CHARACTER_DATA_DEFALUT_LEVEL = 1;
    public static readonly int CHARACTER_DATA_MAX_LEVEL = 30;
    public static readonly float CHARACTER_DATA_DEFALUT_EXPERIENCE = 0f;

    public static readonly int CHARACTER_DATA_LEVEL_UP_ABILITY_POINT = 5;
    public static readonly int CHARACTER_DATA_LEVEL_UP_SKILL_POINT = 3;

    public static readonly int CHARACTER_DATA_DEFALUT_MAX_HP = 60;
    public static readonly int CHARACTER_DATA_DEFALUT_MAX_SP = 60;

    public static readonly int CHARACTER_DATA_DEFALUT_STRENGTH = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_VITALITY = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_DEXTERITY = 10;
    public static readonly int CHARACTER_DATA_DEFALUT_LUCK = 10;

    public static readonly float PLAYER_STAT_ATTACK_SPEED_MIN = 0.1f;
    public static readonly float PLAYER_STAT_ATTACK_SPEED_DEFAULT = 1f;
    public static readonly float PLAYER_STAT_ATTACK_SPEED_MAX = 2f;

    public static readonly float PLAYER_STAT_MOVE_SPEED_MIN = 0.1f;
    public static readonly float PLAYER_STAT_MOVE_SPEED_DEFAULT = 3f;
    public static readonly float PLAYER_STAT_MOVE_SPEED_MAX = 5f;

    public static readonly float PLAYER_STAT_CRITICAL_CHANCE_MIN = 0f;
    public static readonly float PLAYER_STAT_CRITICAL_CHANCE_DEFAULT = 0f;
    public static readonly float PLAYER_STAT_CRITICAL_CHANCE_MAX = 100f;

    public static readonly float PLAYER_STAT_CRITICAL_DAMAGE_MIN = 100f;
    public static readonly float PLAYER_STAT_CRITICAL_DAMAGE_DEFAULT = 100f;

    // Quest
    public static readonly uint CHARACTER_DATA_MAIN_QUEST_PROGRESS = 0;
}