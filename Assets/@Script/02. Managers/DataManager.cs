using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto};

    [SerializeField] private PlayerSaveData playerData;
    private string playerDataPath;

    private Dictionary<int, LevelData> levelTable;
    private Dictionary<string, TextData> textTable;
    private Dictionary<string, StatData> statTable;
    private Dictionary<string, RandomOptionData> randomOptionTable;
    private Dictionary<string, FixedOptionData> fixedOptionTable;

    private Dictionary<string, NodeData> nodeTable;
    private Dictionary<string, SkillData> skillTable;
    private Dictionary<BUFF_TYPE, BuffData> buffTable;
    private Dictionary<DEBUFF_TYPE, DebuffData> debuffTable;

    private Dictionary<string, ItemData> itemTable;
    private Dictionary<string, ResponseWaterData> responseWaterTable;

    private Dictionary<string, DropTableData> dropTable;
    private Dictionary<string, EnemyData> enemyTable;

    private Dictionary<string, DialogueData> dialogueTable;
    private Dictionary<string, TaskData> taskTable;
    private Dictionary<string, QuestData> questTable;

    private Dictionary<SCENE_ID, GameSceneData> gameSceneTable;
    private Dictionary<string, EnemySpawnerData> enemySpawnerTable;
    private Dictionary<string, ResponseCrystalData> responseCrystalTable;
    private Dictionary<string, ResponseGateData> responseGateTable;
    private Dictionary<string, ResponseTraceData> responseTraceTable;
    private Dictionary<string, TreasureBoxData> treasureBoxTable;
    private Dictionary<string, NPCData> npcTable;

    public void LoadTableFromJson<Key, Value>(out Dictionary<Key, Value> tableDictionary, string tablePath) where Value : ITableData<Key>
    {
        tableDictionary = new Dictionary<Key, Value>();
        if (CheckTableFile(tablePath))
        {
            string jsonData = File.ReadAllText(tablePath);
            Value[] tableDatas = JsonConvert.DeserializeObject<Value[]>(jsonData);

            for (int i = 0; i < tableDatas.Length; ++i)
            {
                tableDatas[i].OnDataLoaded();
                tableDictionary.Add(tableDatas[i].GetPrimaryKey(), tableDatas[i]);
            }
        }
    }
    public Dictionary<Key, Value> GetTable<Key, Value>(Dictionary<Key, Value> tableDictionary, string tablePath) where Value : ITableData<Key>
    {
        if(tableDictionary == null)
            LoadTableFromJson(out tableDictionary, tablePath);

        return tableDictionary;
    }
    public void Initialize()
    {
        LoadTableFromJson(out levelTable, $"{Application.dataPath}/@Table/LEVEL_TABLE.json");
        LoadTableFromJson(out textTable, $"{Application.dataPath}/@Table/TEXT_TABLE.json");
        LoadTableFromJson(out statTable, $"{Application.dataPath}/@Table/STAT_TABLE.json");
        LoadTableFromJson(out fixedOptionTable, $"{Application.dataPath}/@Table/FIXED_OPTION_TABLE.json");
        LoadTableFromJson(out randomOptionTable, $"{Application.dataPath}/@Table/RANDOM_OPTION_TABLE.json");
        LoadTableFromJson(out nodeTable, $"{Application.dataPath}/@Table/NODE_TABLE.json");
        LoadTableFromJson(out skillTable, $"{Application.dataPath}/@Table/SKILL_TABLE.json");

        // Item Tables
        LoadTableFromJson(out itemTable, $"{Application.dataPath}/@Table/ITEM_TABLE.json");
        LoadTableFromJson(out responseWaterTable, $"{Application.dataPath}/@Table/RESPONSE_WATER_TABLE.json");

        LoadTableFromJson(out dropTable, $"{Application.dataPath}/@Table/DROP_TABLE.json");
        LoadTableFromJson(out enemyTable, $"{Application.dataPath}/@Table/ENEMY_TABLE.json");

        LoadTableFromJson(out dialogueTable, $"{Application.dataPath}/@Table/Dialogue_Table.json");
        LoadTableFromJson(out taskTable, $"{Application.dataPath}/@Table/Task_Table.json");
        LoadTableFromJson(out questTable, $"{Application.dataPath}/Table/Quest_Table.json");

        // Scene Tables
        LoadTableFromJson(out gameSceneTable, $"{Application.dataPath}/@Table/GAME_SCENE_TABLE.json");
        LoadTableFromJson(out enemySpawnerTable, $"{Application.dataPath}/@Table/ENEMY_SPAWNER_TABLE.json");
        LoadTableFromJson(out responseCrystalTable, $"{Application.dataPath}/@Table/RESPONSE_CRYSTAL_TABLE.json");
        LoadTableFromJson(out responseGateTable, $"{Application.dataPath}/@Table/RESPONSE_GATE_TABLE.json");
        LoadTableFromJson(out responseTraceTable, $"{Application.dataPath}/@Table/RESPONSE_TRACE_TABLE.json");
        LoadTableFromJson(out treasureBoxTable, $"{Application.dataPath}/@Table/TREASURE_BOX_TABLE.json");
        LoadTableFromJson(out npcTable, $"{Application.dataPath}/@Table/NPC_TABLE.json");

        playerDataPath = $"{Application.dataPath}/@UserData/PLAYER_DATA.json";
        LoadPlayerData();
    }

    public bool CheckTableFile(string filePath)
    {
        FileInfo loadFile = new FileInfo(filePath);
        return loadFile.Exists;
    }

    public void LoadPlayerData()
    {
        if (CheckTableFile(playerDataPath))
        {
            string jsonPlayerData = File.ReadAllText(playerDataPath);
            playerData = JsonConvert.DeserializeObject<PlayerSaveData>(jsonPlayerData, serializerSettings);
        }
        else
        {
            playerData = new PlayerSaveData();
            playerData.CreateData();
        }
        playerData?.LoadData();
        playerData?.UpdateData();
        SavePlayerData();
    }
    public void SavePlayerData()
    {
        playerData?.SaveData();
        string jsonPlayerData = JsonConvert.SerializeObject(playerData, Formatting.Indented, serializerSettings);
        File.WriteAllText(playerDataPath, jsonPlayerData);
    }

    #region Property
    public Dictionary<int, LevelData> LevelTable { get { return GetTable(levelTable, $"{Application.dataPath}/@Table/LEVEL_TABLE.json"); } }
    public Dictionary<string, TextData> TextTable { get { return textTable; } }
    public Dictionary<string, StatData> StatTable { get { return statTable; } }
    public Dictionary<string, FixedOptionData> FixedOptionTable { get { return fixedOptionTable; } }
    public Dictionary<string, RandomOptionData> RandomOptionTable { get { return randomOptionTable; } }
    public Dictionary<string, ItemData> ItemTable { get { return itemTable; } }
    public Dictionary<string, NodeData> NodeTable { get { return nodeTable; } }
    public Dictionary<string, SkillData> SkillTable { get { return skillTable; } }
    public Dictionary<string, ResponseWaterData> ResponseWaterTable { get { return responseWaterTable; } }
    public Dictionary<string, DialogueData> DialogueTable { get { return dialogueTable; } }
    public Dictionary<string, DropTableData> DropTable { get { return dropTable; } }
    public Dictionary<string, TaskData> TaskTable { get { return GetTable(taskTable, $"{Application.dataPath}/@Table/TASK_TABLE.json"); } }
    public Dictionary<string, QuestData> QuestTable { get { return questTable; } }
    public Dictionary<string, EnemyData> EnemyTable { get { return enemyTable; } }
    public Dictionary<BUFF_TYPE, BuffData> BuffTable { get { return buffTable; } }
    public Dictionary<DEBUFF_TYPE, DebuffData> DebuffTable { get { return debuffTable; } }
    public Dictionary<SCENE_ID, GameSceneData> GameSceneTable { get { return gameSceneTable; } }
    public Dictionary<string, ResponseCrystalData> ResponseCrystalTable { get { return responseCrystalTable; } }
    public Dictionary<string, ResponseGateData> ResponseGateTable { get { return responseGateTable; } }
    public Dictionary<string, ResponseTraceData> ResponseTraceTable { get { return responseTraceTable; } }
    public Dictionary<string, EnemySpawnerData> EnemySpawnerTable { get { return enemySpawnerTable; } }
    public Dictionary<string, TreasureBoxData> TreasureBoxTable { get { return treasureBoxTable; } }
    public Dictionary<string, NPCData> NPCTable { get { return npcTable; } }
    public PlayerSaveData PlayerData { get { return playerData; } }
    public CharacterData CurrentCharacterData { get { return playerData?.CurrentCharacterData; } }
    #endregion
}
