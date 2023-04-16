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
    private Dictionary<int, BaseItem> itemTable = new Dictionary<int, BaseItem>();
    private Dictionary<uint, Quest> questTable = new Dictionary<uint, Quest>();
    private Dictionary<uint, EnemyData> enemyTable = new Dictionary<uint, EnemyData>();
    private Dictionary<BUFF_TYPE, BuffData> buffTable = new Dictionary<BUFF_TYPE, BuffData>();
    private Dictionary<DEBUFF_TYPE, DebuffData> debuffTable = new Dictionary<DEBUFF_TYPE, DebuffData>();
    private Dictionary<CHAPTER_LIST, WayPointData> wayPointTable = new Dictionary<CHAPTER_LIST, WayPointData>();

    private string playerDataPath;
    private string levelTablePath;
    private string questTablePath;
    private string wayPointTablePath;
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
        playerDataPath = Application.dataPath + "/@UserData/Player_Data.json";
        levelTablePath = Application.dataPath + "/@Table/Level_Table.json";
        questTablePath = Application.dataPath + "/Table/Quest_Table.json";
        wayPointTablePath = Application.dataPath + "/Table/Way_Point_Table.json";

        //
        weaponTablePath = Application.dataPath + "/Table/Weapon_Item_Table.json";
        helmetTablePath = Application.dataPath + "/Table/Helmet_Item_Table.json";
        armorTablePath = Application.dataPath + "/Table/Armor_Item_Table.json";
        bootsTablePath = Application.dataPath + "/Table/Boots_Item_Table.json";

        hpPotionTablePath = Application.dataPath + "/Table/HP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/Table/SP_Potion_Table.json";
        //

        LoadLevelTable();
        LoadQuestTable();
        LoadWayPointTable();

        LoadItemTable<WeaponItem>(weaponTablePath);
        LoadItemTable<HelmetItem>(helmetTablePath);
        LoadItemTable<ArmorItem>(armorTablePath);
        LoadItemTable<BootsItem>(bootsTablePath);
        LoadItemTable<HPPotion>(hpPotionTablePath);
        LoadItemTable<SPPotion>(spPotionTablePath);

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

    public void LoadWayPointTable()
    {
        if (CheckFile(wayPointTablePath))
        {
            string jsonWayPointData = File.ReadAllText(wayPointTablePath);
            WayPointData[] wayPointDatas = JsonConvert.DeserializeObject<WayPointData[]>(jsonWayPointData);

            for (int i = 0; i < wayPointDatas.Length; ++i)
            {
                wayPointTable.Add((CHAPTER_LIST)i, wayPointDatas[i]);
            }
        }
    }

    public void LoadItemTable<T>(string path) where T : BaseItem, new()
    {
        if (CheckFile(path))
        {
            string jsonItemData = File.ReadAllText(path);
            T[] itemDatas = JsonConvert.DeserializeObject<T[]>(jsonItemData);

            for (int i = 0; i < itemDatas.Length; ++i)
            {
                itemDatas[i].ItemSprite = Managers.ResourceManager.LoadResourceSync<Sprite>("Sprite_Item_" + itemDatas[i].ItemName + "_" + itemDatas[i].ItemID);
                itemDatas[i].Initialize(itemDatas[i]);
                itemTable.Add(itemDatas[i].ItemID, itemDatas[i]);
            }
        }
    }

    public void LoadQuestTable()
    {
        if (CheckFile(questTablePath))
        {
            string jsonLevelData = File.ReadAllText(questTablePath);
            Quest[] quests = JsonConvert.DeserializeObject<Quest[]>(jsonLevelData);

            for (int i = 0; i < quests.Length; ++i)
            {
                quests[i].Initialize();
                questTable.Add(quests[i].QuestID, quests[i]);
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
    public Dictionary<int, BaseItem> ItemTable { get { return itemTable; } }
    public Dictionary<uint, Quest> QuestTable { get { return questTable; } }
    public Dictionary<uint, EnemyData> EnemyTable { get { return enemyTable; } }
    public Dictionary<BUFF_TYPE, BuffData> BuffTable { get { return buffTable; } }
    public Dictionary<DEBUFF_TYPE, DebuffData> DebuffTable { get { return debuffTable; } }
    public Dictionary<CHAPTER_LIST, WayPointData> WayPointTable { get { return wayPointTable; } }
    public PlayerData PlayerData { get { return playerData; } }
    public CharacterData CurrentCharacterData { get { return playerData?.CharacterDatas[playerData.SelectCharacterIndex]; } }
    #endregion
}
