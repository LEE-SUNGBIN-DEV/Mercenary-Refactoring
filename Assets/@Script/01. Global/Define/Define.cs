using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSlot
{
    public int slotIndex;
    public SelectionCharacter selectionCharacter;
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
public class ObjectPool
{
    public string key;
    public GameObject value;
    public int amount;
    public Queue<GameObject> queue = new Queue<GameObject>();

    public void Initialize(GameObject parent)
    {
        for (int i = 0; i < amount; ++i)
        {
            GameObject poolObject = GameObject.Instantiate(value, parent.transform);
            poolObject.SetActive(false);
            queue.Enqueue(poolObject);
        }
    }
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
    Skill = 3,
    Roll = 4,
    Hit = 5,
    HeavyHit = 6,
    Stun = 7,
    Compete = 8,
    Die = 9,

    // Lancer
    LancerDefense = 2
}

public enum ATTACK_TYPE
{
    COMBO1,
    COMBO2,
    COMBO3,
    COMBO4,

    SMASH1,
    SMASH2,
    SMASH3,
    SMASH4,

    SKILL
}

public enum COMBAT_TYPE
{
    // Common
    DefaultAttack,
    SmashAttack,
    StunAttack,

    // Player
    Defense,
    Parrying,
    ParryingAttack,
    Counter,

    // Enemy
    CompetableAttack,
    CounterableAttack
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