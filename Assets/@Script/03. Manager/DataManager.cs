using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    // Player Data
    [SerializeField] private PlayerData playerData;
    private string playerDataPath;

    // Level Datas
    private Dictionary<int, float> levelTable = new Dictionary<int, float>();
    private string levelTablePath;

    // Item Datas
    private Dictionary<int, BaseItem> itemTable = new Dictionary<int, BaseItem>();
    private string weaponTablePath;
    private string helmetTablePath;
    private string armorTablePath;
    private string bootsTablePath; 
    private string hpPotionTablePath;
    private string spPotionTablePath;

    // Quest Datas
    private Dictionary<uint, Quest> questTable = new Dictionary<uint, Quest>();
    private string questTablePath;

    // Enemy Datas
    private Dictionary<uint, EnemyData> enemyTable = new Dictionary<uint, EnemyData>();
    private string enemyTablePath;

    // Status Effect Datas
    private Dictionary<BUFF_TYPE, BuffData> buffTable = new Dictionary<BUFF_TYPE, BuffData>();
    private string buffTablePath;
    private Dictionary<DEBUFF_TYPE, DebuffData> debuffTable = new Dictionary<DEBUFF_TYPE, DebuffData>();
    private string debuffTablePath;

    // Game Scene Datas
    private EnemySpawnData[] spawnTable;
    private string spawnTablePath;

    private ResonanceObjectData[] resonanceObjectTable;
    private string resonanceObjectTablePath;

    private Dictionary<SCENE_LIST, GameSceneData> gameSceneTable = new Dictionary<SCENE_LIST, GameSceneData>();
    private string gameSceneTablePath;

    public void InitializeDataPath()
    {
        playerDataPath = Application.dataPath + "/@UserData/Player_Data.json";

        levelTablePath = Application.dataPath + "/@Table/Level_Table.json";

        weaponTablePath = Application.dataPath + "/Table/Weapon_Item_Table.json";
        helmetTablePath = Application.dataPath + "/Table/Helmet_Item_Table.json";
        armorTablePath = Application.dataPath + "/Table/Armor_Item_Table.json";
        bootsTablePath = Application.dataPath + "/Table/Boots_Item_Table.json";
        hpPotionTablePath = Application.dataPath + "/Table/HP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/Table/SP_Potion_Table.json";

        questTablePath = Application.dataPath + "/Table/Quest_Table.json";

        spawnTablePath = Application.dataPath + "/@Table/Spawn_Table.json";
        resonanceObjectTablePath = Application.dataPath + "/@Table/Resonance_Object_Table.json";
        gameSceneTablePath = Application.dataPath + "/@Table/Game_Scene_Table.json";
    }

    public void Initialize()
    {
        InitializeDataPath();

        LoadLevelTable();
        LoadQuestTable();
        LoadGameSceneTable();

        LoadItemTable<WeaponItem>(weaponTablePath);
        LoadItemTable<HelmetItem>(helmetTablePath);
        LoadItemTable<ArmorItem>(armorTablePath);
        LoadItemTable<BootsItem>(bootsTablePath);
        LoadItemTable<HPPotion>(hpPotionTablePath);
        LoadItemTable<SPPotion>(spPotionTablePath);

        // Load Player Data Must be Last.
        LoadPlayerData();
    }

    public bool CheckFile(string filePath)
    {
        FileInfo loadFile = new FileInfo(filePath);
        return loadFile.Exists;
    }

    #region Player Datas
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
    #endregion


    #region Level Datas
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
    #endregion


    #region Game Scene Datas
    public void LoadGameSceneTable()
    {
        if (CheckFile(gameSceneTablePath))
        {
            string jsonData = File.ReadAllText(gameSceneTablePath);
            GameSceneData[] gameSceneDatas = JsonConvert.DeserializeObject<GameSceneData[]>(jsonData);

            for (int i = 0; i < gameSceneDatas.Length; ++i)
            {
                gameSceneDatas[i].Initialize();
                gameSceneTable.Add(gameSceneDatas[i].scene, gameSceneDatas[i]);
            }
        }

        if (CheckFile(spawnTablePath))
        {
            string jsonData = File.ReadAllText(spawnTablePath);
            spawnTable = JsonConvert.DeserializeObject<EnemySpawnData[]>(jsonData);

            for (int i = 0; i < spawnTable.Length; ++i)
            {
                switch(spawnTable[i].enemyType)
                {
                    case ENEMY_TYPE.Normal:
                        gameSceneTable[spawnTable[i].scene].normalSpawnDataList.Add(spawnTable[i]);
                        break;
                    case ENEMY_TYPE.Elite:
                        gameSceneTable[spawnTable[i].scene].eliteSpawnDataList.Add(spawnTable[i]);
                        break;
                    case ENEMY_TYPE.Boss:
                        gameSceneTable[spawnTable[i].scene].bossSpawnDataList.Add(spawnTable[i]);
                        break;
                }
            }
        }

        if (CheckFile(resonanceObjectTablePath))
        {
            string jsonData = File.ReadAllText(resonanceObjectTablePath);
            resonanceObjectTable = JsonConvert.DeserializeObject<ResonanceObjectData[]>(jsonData);

            for (int i = 0; i < resonanceObjectTable.Length; ++i)
            {
                resonanceObjectTable[i].index = i;
                gameSceneTable[spawnTable[i].scene].resonanceObjectDataList.Add(resonanceObjectTable[i]);
            }
        }
    }
    #endregion

    #region Item Datas
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
    #endregion

    #region Quest Datas
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
    #endregion

    #region Property
    public Dictionary<int, float> LevelTable { get { return levelTable; } }
    public Dictionary<int, BaseItem> ItemTable { get { return itemTable; } }
    public Dictionary<uint, Quest> QuestTable { get { return questTable; } }
    public Dictionary<uint, EnemyData> EnemyTable { get { return enemyTable; } }
    public Dictionary<BUFF_TYPE, BuffData> BuffTable { get { return buffTable; } }
    public Dictionary<DEBUFF_TYPE, DebuffData> DebuffTable { get { return debuffTable; } }
    public Dictionary<SCENE_LIST, GameSceneData> GameSceneTable { get { return gameSceneTable; } }
    public PlayerData PlayerData { get { return playerData; } }
    public CharacterData CurrentCharacterData { get { return playerData?.CharacterDatas[playerData.SelectCharacterIndex]; } }
    #endregion
}
