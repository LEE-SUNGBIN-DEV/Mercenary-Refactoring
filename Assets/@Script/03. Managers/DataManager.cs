using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    [SerializeField] private PlayerData playerData;
    private string playerDataPath;

    private Dictionary<int, float> levelTable = new Dictionary<int, float>();
    private string levelTablePath;

    private Dictionary<int, BaseItem> itemTable = new Dictionary<int, BaseItem>();
    private string runeTablePath;

    // Equipment Datas
    private Dictionary<int, HalberdData> halberdTable = new Dictionary<int, HalberdData>();
    private string halberdTablePath;
    private Dictionary<int, SwordShieldData> swordShieldTable = new Dictionary<int, SwordShieldData>();
    private string swordShieldTablePath;
    private Dictionary<int, ArmorData> armorTable = new Dictionary<int, ArmorData>();
    private string armorTablePath;

    private Dictionary<int, DropData> dropTable = new Dictionary<int, DropData>();
    private string dropTablePath;

    private Dictionary<uint, Quest> questTable = new Dictionary<uint, Quest>();
    private string questTablePath;

    private Dictionary<int, EnemyData> enemyTable = new Dictionary<int, EnemyData>();
    private string enemyTablePath;

    private Dictionary<BUFF_TYPE, BuffData> buffTable = new Dictionary<BUFF_TYPE, BuffData>();
    private string buffTablePath;
    private Dictionary<DEBUFF_TYPE, DebuffData> debuffTable = new Dictionary<DEBUFF_TYPE, DebuffData>();
    private string debuffTablePath;

    // Game Scene Datas
    private Dictionary<int, EnemySpawnerData> enemySpawnerTable = new Dictionary<int, EnemySpawnerData>();
    private string enemySpawnerTablePath;

    private Dictionary<int, ResonanceCrystalData> resonanceCrystalTable = new Dictionary<int, ResonanceCrystalData>();
    private string resonanceCrystalTablePath;

    private Dictionary<int, ResonanceGateData> resonanceGateTable = new Dictionary<int, ResonanceGateData>();
    private string resonanceGateTablePath;
    
    private Dictionary<SCENE_LIST, GameSceneData> gameSceneTable = new Dictionary<SCENE_LIST, GameSceneData>();
    private string gameSceneTablePath;

    public void InitializeDataPath()
    {
        playerDataPath = $"{Application.dataPath}/@UserData/Player_Data.json";
        levelTablePath = $"{Application.dataPath}/@Table/Level_Table.json";
        questTablePath = $"{Application.dataPath}/Table/Quest_Table.json";

        runeTablePath = $"{Application.dataPath}/Table/Rune_Item_Table.json";

        halberdTablePath = $"{Application.dataPath}/@Table/Halberd_Table.json";
        swordShieldTablePath = $"{Application.dataPath}/@Table/Sword_Shield_Table.json";
        armorTablePath = $"{Application.dataPath}/@Table/Armor_Table.json";

        dropTablePath = $"{Application.dataPath}/@Table/Drop_Table.json";
        enemyTablePath = $"{Application.dataPath}/@Table/Enemy_Table.json";

        enemySpawnerTablePath = $"{Application.dataPath}/@Table/Enemy_Spawner_Table.json";
        resonanceCrystalTablePath = $"{Application.dataPath}/@Table/Resonance_Crystal_Table.json";
        resonanceGateTablePath = $"{Application.dataPath}/@Table/Resonance_Gate_Table.json";
        gameSceneTablePath = $"{Application.dataPath}/@Table/Game_Scene_Table.json";
    }

    public void Initialize()
    {
        InitializeDataPath();

        LoadEnemyTable();
        LoadLevelTable();
        LoadQuestTable();
        LoadGameSceneTable();

        LoadItemTable<RuneItem>(runeTablePath);
        LoadEquipmentTable();

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
        playerData.CurrentCharacterIndex = index;
        SavePlayerData();
    }
    #endregion

    #region Level Datas
    public void LoadLevelTable()
    {
        if (CheckFile(levelTablePath))
        {
            string jsonLevelData = File.ReadAllText(levelTablePath);
            LevelData[] levelDatas = JsonConvert.DeserializeObject<LevelData[]>(jsonLevelData);

            for (int i = 0; i < levelDatas.Length; ++i)
                levelTable.Add(levelDatas[i].level, levelDatas[i].experience);
        }
    }
    #endregion

    #region Equipment Datas
    public void LoadEquipmentTable()
    {
        if (CheckFile(halberdTablePath))
        {
            string jsonData = File.ReadAllText(halberdTablePath);
            HalberdData[] halberdDatas = JsonConvert.DeserializeObject<HalberdData[]>(jsonData);

            for (int i = 0; i < halberdDatas.Length; ++i)
                halberdTable.Add(halberdDatas[i].id, halberdDatas[i]);
        }
        if (CheckFile(swordShieldTablePath))
        {
            string jsonData = File.ReadAllText(swordShieldTablePath);
            SwordShieldData[] swordShieldDatas = JsonConvert.DeserializeObject<SwordShieldData[]>(jsonData);

            for (int i = 0; i < swordShieldDatas.Length; ++i)
                swordShieldTable.Add(swordShieldDatas[i].id, swordShieldDatas[i]);
        }
        if (CheckFile(armorTablePath))
        {
            string jsonData = File.ReadAllText(armorTablePath);
            ArmorData[] armorDatas = JsonConvert.DeserializeObject<ArmorData[]>(jsonData);

            for (int i = 0; i < armorDatas.Length; ++i)
                armorTable.Add(armorDatas[i].id, armorDatas[i]);
        }
    }
    #endregion

    public void LoadDropTable()
    {
        if (CheckFile(enemyTablePath))
        {
            string jsonData = File.ReadAllText(enemyTablePath);
            DropData[] dropDatas = JsonConvert.DeserializeObject<DropData[]>(jsonData);

            for (int i = 0; i < dropDatas.Length; ++i)
            {
                dropTable.Add(dropDatas[i].id, dropDatas[i]);
            }
        }
    }

    #region Enemy Datas
    public void LoadEnemyTable()
    {
        if (CheckFile(enemyTablePath))
        {
            string jsonData = File.ReadAllText(enemyTablePath);
            EnemyData[] enemyDatas = JsonConvert.DeserializeObject<EnemyData[]>(jsonData);

            for (int i = 0; i < enemyDatas.Length; ++i)
            {
                enemyTable.Add(enemyDatas[i].id, enemyDatas[i]);
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

        if (CheckFile(enemySpawnerTablePath))
        {
            string jsonData = File.ReadAllText(enemySpawnerTablePath);
            EnemySpawnerData[] spawnerDatas = JsonConvert.DeserializeObject<EnemySpawnerData[]>(jsonData);

            for (int i = 0; i < spawnerDatas.Length; ++i)
            {
                enemySpawnerTable.Add(spawnerDatas[i].id, spawnerDatas[i]);
            }
        }

        if (CheckFile(resonanceCrystalTablePath))
        {
            string jsonData = File.ReadAllText(resonanceCrystalTablePath);
            ResonanceCrystalData[] resonanceCrystalDatas = JsonConvert.DeserializeObject<ResonanceCrystalData[]>(jsonData);

            for (int i = 0; i < resonanceCrystalDatas.Length; ++i)
            {
                resonanceCrystalTable.Add(resonanceCrystalDatas[i].id, resonanceCrystalDatas[i]);
                gameSceneTable[resonanceCrystalDatas[i].GetLocatedScene()].resonanceCrystalDataList.Add(resonanceCrystalDatas[i]);
            }
        }

        if (CheckFile(resonanceGateTablePath))
        {
            string jsonData = File.ReadAllText(resonanceGateTablePath);
            ResonanceGateData[] resonanceGateDatas = JsonConvert.DeserializeObject<ResonanceGateData[]>(jsonData);

            for (int i = 0; i < resonanceGateDatas.Length; ++i)
            {
                resonanceGateTable.Add(resonanceGateDatas[i].id, resonanceGateDatas[i]);
                gameSceneTable[resonanceGateDatas[i].GetLocatedScene()].resonanceGateDataList.Add(resonanceGateDatas[i]);
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
    public Dictionary<int, HalberdData> HalberdTable { get { return halberdTable; } }
    public Dictionary<int, SwordShieldData> SwordShieldTable { get { return swordShieldTable; } }
    public Dictionary<int, ArmorData> ArmorTable { get { return armorTable; } }
    public Dictionary<int, DropData> DropTable { get { return dropTable; } }
    public Dictionary<uint, Quest> QuestTable { get { return questTable; } }
    public Dictionary<int, EnemyData> EnemyTable { get { return enemyTable; } }
    public Dictionary<BUFF_TYPE, BuffData> BuffTable { get { return buffTable; } }
    public Dictionary<DEBUFF_TYPE, DebuffData> DebuffTable { get { return debuffTable; } }
    public Dictionary<SCENE_LIST, GameSceneData> GameSceneTable { get { return gameSceneTable; } }
    public Dictionary<int, ResonanceCrystalData> ResonanceCrystalTable { get { return resonanceCrystalTable; } }
    public Dictionary<int, ResonanceGateData> ResonanceGateTable { get { return resonanceGateTable; } }
    public Dictionary<int, EnemySpawnerData> EnemySpawnerTable { get { return enemySpawnerTable; } }
    public PlayerData PlayerData { get { return playerData; } }
    public CharacterData CurrentCharacterData { get { return playerData?.CharacterDatas[playerData.CurrentCharacterIndex]; } }
    #endregion
}
