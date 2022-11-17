using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    #region Event
    public event UnityAction<CharacterData> OnChangeCharacterData;
    public event UnityAction<CharacterData> OnChangeMainQuestPrograss;
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
    [SerializeField] private List<ItemSaveData> itemDataList = new List<ItemSaveData>();
    [SerializeField] private int money;

    // Quest
    [SerializeField] private uint mainQuestPrograss;
    [SerializeField] private List<QuestSaveData> questSaveList = new List<QuestSaveData>();

    public CharacterData(CHARACTER_CLASS selectedClass)
    {
        Initialize();
        characterClass = System.Enum.GetName(typeof(CHARACTER_CLASS), selectedClass);
    }

    public void Initialize()
    {
        characterClass = null;
        characterLocation = Vector3.zero;

        level = Constants.CHARACTER_DATA_DEFALUT_LEVEL;
        currentExperience = Constants.CHARACTER_DATA_DEFALUT_EXPERIENCE;
        maxExperience = Managers.DataManager.LevelTable[level];

        statPoint = Constants.CHARACTER_DATA_DEFALUT_STATPOINT;
        strength = Constants.CHARACTER_DATA_DEFALUT_STRENGTH;
        vitality = Constants.CHARACTER_DATA_DEFALUT_VITALITY;
        dexterity = Constants.CHARACTER_DATA_DEFALUT_DEXTERITY;
        luck = Constants.CHARACTER_DATA_DEFALUT_LUCK;

        money = Constants.CHARACTER_DATA_DEFAULT_MONEY;
        mainQuestPrograss = 0;
    }

    public void LevelUp()
    {
        currentExperience -= maxExperience;
        maxExperience = Managers.DataManager.LevelTable[Level];
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
            MainQuestPrograss = quest.QuestID;
        }
    }

    public void ParseItem()
    {
        for(int i=0; i<itemDataList.Count; ++i)
        {
            switch (itemDataList[i].SlotType)
            {
                case SLOT_TYPE.Inventory:
                    {
                        break;
                    }
                case SLOT_TYPE.QuickSlot:
                    {
                        break;
                    }
                case SLOT_TYPE.WeaponSlot:
                    {
                        break;
                    }
                case SLOT_TYPE.HelmetSlot:
                    {
                        break;
                    }
                case SLOT_TYPE.ArmorSlot:
                    {
                        break;
                    }
                case SLOT_TYPE.BootsSlot:
                    {
                        break;
                    }
            }
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
            if (maxExperience < 1)
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

    public List<ItemSaveData> ItemDataList
    {
        get { return itemDataList; }
        set { itemDataList = value; }
    }
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            if (money < 0)
            {
                money = 0;
            }
            OnChangeCharacterData?.Invoke(this);
        }
    }

    public uint MainQuestPrograss
    {
        get { return mainQuestPrograss; }
        set
        {
            mainQuestPrograss = value;
            OnChangeMainQuestPrograss?.Invoke(this);
        }
    }
    public List<QuestSaveData> QuestSaveList
    {
        get { return questSaveList; }
        set { questSaveList = value; }
    }
    #endregion
}
