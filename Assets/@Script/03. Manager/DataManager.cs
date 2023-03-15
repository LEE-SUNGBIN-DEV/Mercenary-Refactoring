#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    private Dictionary<int, float> levelTable = new Dictionary<int, float>();
    private Dictionary<int, BaseItem> itemDatabase = new Dictionary<int, BaseItem>();
    private Dictionary<uint, Quest> questDatabase = new Dictionary<uint, Quest>();
    private Dictionary<uint, EnemyData> enemyDatabase = new Dictionary<uint, EnemyData>();
    private Dictionary<BUFF_TYPE, BuffData> buffDatabase = new Dictionary<BUFF_TYPE, BuffData>();
    private Dictionary<DEBUFF_TYPE, DebuffData> debuffDatabase = new Dictionary<DEBUFF_TYPE, DebuffData>();

    private string playerDataPath;
    private string levelTablePath;
    private string questTablePath;
    private string enemyTablePath;
    private string buffTablePath;
    private string debuffTablePath;

    private string weaponTablePath;
    private string helmetTablePath;
    private string armorTablePath;
    private string bootsTablePath;

    private string hpPotionTablePath;
    private string spPotionTablePath;

    [SerializeField] private PlayerData playerData;

    public void Initialize()
    {
        playerDataPath = Application.dataPath + "Player_Data.json";
        levelTablePath = Application.dataPath + "/@Table/Level_Table.json";
        questTablePath = Application.dataPath + "/Table/Quest_Table.json";

        //
        weaponTablePath = Application.dataPath + "/Table/Weapon_Item_Table.json";
        helmetTablePath = Application.dataPath + "/Table/Helmet_Item_Table.json";
        armorTablePath = Application.dataPath + "/Table/Armor_Item_Table.json";
        bootsTablePath = Application.dataPath + "/Table/Boots_Item_Table.json";

        hpPotionTablePath = Application.dataPath + "/Table/HP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/Table/SP_Potion_Table.json";
        //

        LoadLevelTable();
        LoadItemDatabase();
        LoadQuestTable();
#if EDITOR_TEST
#else
        LoadPlayerData();
#endif
    }

    public bool CheckFile(string filePath)
    {
        FileInfo loadFile = new FileInfo(filePath);
        return loadFile.Exists;
    }
    public void LoadPlayerData()
    {
        if (CheckFile(playerDataPath))
        {
            string jsonPlayerData = File.ReadAllText(playerDataPath);
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonPlayerData);
        }
        else
        {
            playerData = new PlayerData();
            playerData.Initialize();
            SavePlayerData();
        }
    }
    public void LoadLevelTable()
    {
        if (CheckFile(levelTablePath))
        {
            string jsonLevelData = File.ReadAllText(levelTablePath);
            LevelTable levelTable = JsonConvert.DeserializeObject<LevelTable>(jsonLevelData);

            for (int i = 0; i < levelTable.levels.Length; ++i)
            {
                this.levelTable.Add(levelTable.levels[i], levelTable.maxExperiences[i]);
            }
        }
    }
    public void LoadItemTable<T>(string path) where T : BaseItem, new()
    {
        if (CheckFile(path))
        {
            string jsonItemData = File.ReadAllText(path);
            ItemTable<T> itemTable = JsonConvert.DeserializeObject<ItemTable<T>>(jsonItemData);

            for (int i = 0; i < itemTable.items.Length; ++i)
            {
                itemTable.items[i].ItemSprite = Managers.ResourceManager.LoadResourceSync<Sprite>("Sprite_Item_" + itemTable.items[i].ItemName + "_" + itemTable.items[i].ItemID);
                T item = new();
                item.Initialize(itemTable.items[i]);
                itemDatabase.Add(itemTable.items[i].ItemID, item);
            }
        }
    }
    public void LoadItemDatabase()
    {
        LoadItemTable<WeaponItem>(weaponTablePath);
        LoadItemTable<HelmetItem>(helmetTablePath);
        LoadItemTable<ArmorItem>(armorTablePath);
        LoadItemTable<BootsItem>(bootsTablePath);
        LoadItemTable<HPPotion>(hpPotionTablePath);
        LoadItemTable<SPPotion>(spPotionTablePath);
    }

    public void LoadQuestTable()
    {
        if (CheckFile(questTablePath))
        {
            string jsonLevelData = File.ReadAllText(questTablePath);
            QuestTable questTable = JsonConvert.DeserializeObject<QuestTable>(jsonLevelData);

            for (int i = 0; i < questTable.questTable.Length; ++i)
            {
                Quest quest = new Quest();
                quest.Initialize(questTable.questTable[i]);
                questDatabase.Add(quest.QuestID, quest);
            }
        }
    }

    public void SavePlayerData()
    {
        string jsonPlayerData = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        File.WriteAllText(playerDataPath, jsonPlayerData);
    }

    public void SetCurrentCharacter(int index)
    {
        playerData.SelectCharacterIndex = index;
        SavePlayerData();
    }

    #region Property
    public Dictionary<int, float> LevelTable { get { return levelTable; } }
    public Dictionary<int, BaseItem> ItemDatabase { get { return itemDatabase; } }
    public Dictionary<uint, Quest> QuestDatabase { get { return questDatabase; } }
    public Dictionary<uint, EnemyData> EnemyDatabase { get { return enemyDatabase; } }
    public Dictionary<BUFF_TYPE, BuffData> BuffDatabase { get { return buffDatabase; } }
    public Dictionary<DEBUFF_TYPE, DebuffData> DebuffDatabase { get { return debuffDatabase; } }
    public PlayerData PlayerData { get { return playerData; } }
    public CharacterData SelectCharacterData
    {
        get { return playerData?.CharacterDatas[playerData.SelectCharacterIndex]; }
    }
    #endregion
}
