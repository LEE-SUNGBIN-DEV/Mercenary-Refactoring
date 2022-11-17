using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class DataManager
{
    private Dictionary<int, float> levelTableDictionary = new Dictionary<int, float>();
    private Dictionary<int, BaseItem> itemTableDictionary = new Dictionary<int, BaseItem>();

    private string playerDataPath;
    private string levelDataPath;
    private string itemDataPath;

    [SerializeField] private PlayerData playerData = null;
    private CharacterData currentCharacterData = null;
    private Character currentCharacter = null;

    public void Initialize()
    {
        playerDataPath = Application.dataPath + "/PlayerData.json";
        levelDataPath = Application.dataPath + "/LevelTable.json";
        itemDataPath = Application.dataPath + "/ItemTable.json";

        LoadLevelTable();
        LoadItemTable();
        LoadPlayerData();
    }

    public bool FileCheck()
    {
        FileInfo loadFile = new FileInfo(playerDataPath);

        return loadFile.Exists;
    }

    public void LoadLevelTable()
    {
        string jsonLevelData = File.ReadAllText(levelDataPath);
        LevelTable levelTable = JsonUtility.FromJson<LevelTable>(jsonLevelData);

        for (int i = 0; i < levelTable.levels.Length; ++i)
        {
            LevelTable.Add(levelTable.levels[i], levelTable.maxExperiences[i]);
        }
    }
    public void LoadItemTable()
    {
        string jsonItemData = File.ReadAllText(itemDataPath);
        ItemTable itemTable = JsonUtility.FromJson<ItemTable>(jsonItemData);

        for (int i = 0; i < itemTable.items.Length; ++i)
        {
            itemTable.items[i].ItemSprite = Managers.ResourceManager.LoadResourceSync<Sprite>("Sprite_Item_" + itemTable.items[i].ItemName);
            itemTableDictionary.Add(itemTable.items[i].ItemID, itemTable.items[i]);
        }
    }

    // Save & Load Player Data
    public void LoadPlayerData()
    {
        if (FileCheck())
        {
            string jsonPlayerData = File.ReadAllText(playerDataPath);
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonPlayerData);
        }
        else
        {
            playerData = new PlayerData(true);
            SavePlayerData();
        }
    }

    public void SavePlayerData()
    {
        string jsonPlayerData = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        File.WriteAllText(playerDataPath, jsonPlayerData);
    }

    #region Property
    public Dictionary<int, float> LevelTable { get { return levelTableDictionary; } }
    public Dictionary<int, BaseItem> ItemTable { get { return itemTableDictionary; } }
    public PlayerData PlayerData { get { return playerData; } }
    public Character CurrentCharacter { get { return currentCharacter; } set { currentCharacter = value; } }
    public CharacterData CurrentCharacterData { get { return currentCharacterData; } set { currentCharacterData = value; } }
    #endregion
}
