using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot
{
    public int slotIndex;
    public CharacterForSelection selectionCharacter;
    public Vector3 characterPoint;
    public TextMeshProUGUI slotText;
    public Button slotButton;

    public CharacterSlot()
    {
        slotText = null;
        slotButton = null;
    }
}

[System.Serializable]
public class MaterialContainer
{
    public string key;
    public Material value;
}

public enum SCENE_LIST
{
    // Common
    Title,
    Selection,
    Loading,

    // Chapter 01
    Forestia,
    Starlight_Forest,
    Dragon_Temple

    //
}

public enum SCENE_TYPE
{
    Unknown,
    Title,
    Selection,
    Vilage,
    Dungeon,
    Loading,

    Size
}

public enum CHARACTER_CLASS
{
    Null,
    Lancer,
    Warrior,
    Berserker,
}

public enum CHARACTER_STATE
{
    // Common Character
    Move = 1,
    Attack = 2,
    Skill = 4,
    Roll = 5,
    Hit = 6,
    HeavyHit = 7,
    Stun = 8,
    Compete = 9,
    Die = 10,

    // Lancer
    LancerDefense = 100

}

public enum CHARACTER_STATE_WEIGHT
{
    // Common Character
    Move = 1,
    Attack = 2,
    Defense = 2,
    Counter = 3,
    Roll = 4,
    Hit = 5,
    HeavyHit = 6,
    Stun = 7,
    Compete = 8,
    Die = 9,

    // Lancer
    // Berserker
}

public enum COMBAT_TYPE
{
    // Common
    StunAttack,

    // Player
    PlayerComboAttack1,
    PlayerComboAttack2,
    PlayerComboAttack3,
    PlayerComboAttack4,

    PlayerSmashAttack1,
    PlayerSmashAttack2,
    PlayerSmashAttack3,
    PlayerSmashAttack4,

    PlayerDefense,
    PlayerParrying,
    PlayerParryingAttack,
    PlayerCounterAttack,

    // Enemy
    EnemyNormalAttack,
    EnemySmashAttack,
    EnemyCompetableAttack,
    EnemyCounterableAttack
}

public enum CURSOR_MODE
{
    Lock,
    Unlock
}

public enum UI_EVENT
{
    Click,
    Press
}