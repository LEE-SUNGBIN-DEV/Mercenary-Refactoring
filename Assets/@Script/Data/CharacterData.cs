using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    #region Event
    public event UnityAction<CharacterData> OnChangeCharacterData;
    public event UnityAction<CharacterData> OnChangeMainQuestProcedure;
    public event UnityAction<CharacterData> OnSaveCharacterData;
    public event UnityAction<CharacterData> OnLoadCharacterData;
    #endregion

    [SerializeField] private string characterClass;
    [SerializeField] private Vector3 characterLocation;

    // Level
    [SerializeField] private int level;
    [SerializeField] private float currentExperience;
    [SerializeField] private float maxExperience;

    // Stats
    [SerializeField] private int statPoint;
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int dexterity;
    [SerializeField] private int luck;

    // Inventory
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private int money;

    // Quick Slots
    [SerializeField] private QuickSlot[] quickSlots;

    // Equipment Slots
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    // Main Quest
    [SerializeField] private uint mainQuestProcedure;

    // Quest Save List
    [SerializeField] private List<QuestSaveData> questSaveList = new List<QuestSaveData>();

    public CharacterData(CHARACTER_CLASS selectedClass)
    {
        Initialize();
        characterClass = System.Enum.GetName(typeof(CHARACTER_CLASS), selectedClass);
    }

    public void Initialize()
    {
        QuestManager.onCompleteQuest -= UpdateMainQuestProcedure;
        QuestManager.onCompleteQuest += UpdateMainQuestProcedure;

        Quest.onReward -= GetQuestReward;
        Quest.onReward += GetQuestReward;

        Enemy.onDie -= GetExperience;
        Enemy.onDie += GetExperience;

        characterClass = null;
        characterLocation = Vector3.zero;

        // Level
        level = Constants.CHARACTER_DATA_DEFALUT_LEVEL;
        currentExperience = Constants.CHARACTER_DATA_DEFALUT_EXPERIENCE;
        maxExperience = Managers.DataManager.LevelDataDictionary[level];

        // Stats
        statPoint = Constants.CHARACTER_DATA_DEFALUT_STATPOINT;
        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        luck = Constants.CHARACTER_DATA_DEFALUT_LUCK;

        InventorySlots = new InventorySlot[Constants.MAX_INVENTORY_SLOT_NUMBER];
        quickSlots = new QuickSlot[Constants.MAX_QUICK_SLOT_NUMBER];

        mainQuestProcedure = 0;
    }

    public void RefreshData()
    {
        CharacterClass = characterClass;
        Level = level;
        CurrentExperience = currentExperience;
        MaxExperience = maxExperience;
        StatPoint = statPoint;
        Strength = strength;
        Vitality = vitality;
        Dexterity = dexterity;
        Luck = luck;
        MainQuestProcedure = mainQuestProcedure;
        Money = money;
        QuestSaveList = questSaveList;
    }

    public void LevelUp()
    {
        currentExperience -= MaxExperience;
        MaxExperience = Managers.DataManager.LevelDataDictionary[Level];
        ++Level;
        StatPoint += 5;
    }

    public void GetExperience(Enemy monster)
    {
        CurrentExperience += monster.ExperienceAmount;
    }
    public void GetQuestReward(Quest quest)
    {
        Money += quest.RewardMoney;
        CurrentExperience += quest.RewardExperience;
    }

    public void UpdateMainQuestProcedure(Quest quest)
    {
        if (quest.questCategory == QUEST_CATEGORY.MAIN)
        {
            MainQuestProcedure = quest.QuestID;
        }
    }

    // Save & Load
    public void SaveCharacterData()
    {
        OnSaveCharacterData(this);
    }

    public void LoadCharacterData()
    {
        OnLoadCharacterData(this);
    }

    #region Property
    public string CharacterClass
    {
        get { return characterClass; }
        set
        {
            characterClass = value;
            OnChangeCharacterData?.Invoke(this);
        }
    }
    
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (level < 1)
            {
                level = 1;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public float CurrentExperience
    {
        get { return currentExperience; }
        set
        {
            currentExperience = value;
            if (currentExperience < 0)
            {
                currentExperience = 0;
            }

            while(currentExperience >= MaxExperience)
            {
                LevelUp();
            }

            OnChangeCharacterData?.Invoke(this);
        }
    }

    public float MaxExperience
    {
        get { return maxExperience; }
        set
        {
            maxExperience = value;
            if (maxExperience <= 0)
            {
                maxExperience = 1;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public int StatPoint
    {
        get { return statPoint; }
        set
        {
            statPoint = value;
            if (statPoint < 0)
            {
                statPoint = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
            if (strength < 0)
            {
                strength = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public int Vitality
    {
        get { return vitality; }
        set
        {
            vitality = value;
            if (vitality < 0)
            {
                vitality = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public int Dexterity
    {
        get { return dexterity; }
        set
        {
            dexterity = value;

            if (dexterity < 0)
            {
                dexterity = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }
    public int Luck
    {
        get { return luck; }
        set
        {
            luck = value;
            if (luck < 0)
            {
                luck = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }
    public uint MainQuestProcedure
    {
        get { return mainQuestProcedure; }
        set
        {
            mainQuestProcedure = value;
            OnChangeMainQuestProcedure?.Invoke(this);
        }
    }
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            if(money < 0)
            {
                money = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public InventorySlot[] InventorySlots { get { return inventorySlots; } set { inventorySlots = value; } }
    public QuickSlot[] QuickSlots { get { return quickSlots; } set { quickSlots = value; } }
    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { HelmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }
    public List<QuestSaveData> QuestSaveList
    {
        get { return questSaveList; }
        set { questSaveList = value; }
    }
    #endregion
}
