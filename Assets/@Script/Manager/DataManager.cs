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
    private string levelTablePath;
    private string itemTablePath;

    [SerializeField] private PlayerData playerData;

    public void Initialize()
    {
        playerDataPath = Application.dataPath + "/PlayerData.json";
        levelTablePath = Application.dataPath + "/LevelTable.json";
        itemTablePath = Application.dataPath + "/ItemTable.json";

        LoadLevelTable();
        LoadItemTable();
        LoadPlayerData();
    }

    public bool FileCheck(string filePath)
    {
        FileInfo loadFile = new FileInfo(filePath);

        return loadFile.Exists;
    }

    public void LoadLevelTable()
    {
        if(FileCheck(levelTablePath))
        {
            string jsonLevelData = File.ReadAllText(levelTablePath);
            LevelTable levelTable = JsonConvert.DeserializeObject<LevelTable>(jsonLevelData);

            for (int i = 0; i < levelTable.levels.Length; ++i)
            {
                levelTableDictionary.Add(levelTable.levels[i], levelTable.maxExperiences[i]);
            }
        }
    }
    public void LoadItemTable()
    {
        if(FileCheck(itemTablePath))
        {
            string jsonItemData = File.ReadAllText(itemTablePath);
            ItemTable itemTable = JsonConvert.DeserializeObject<ItemTable>(jsonItemData);

            for (int i = 0; i < itemTable.items.Length; ++i)
            {
                itemTable.items[i].ItemSprite = Managers.ResourceManager.LoadResourceSync<Sprite>("Sprite_Item_" + itemTable.items[i].ItemID);
                switch(itemTable.items[i].ItemType)
                {
                    case ITEM_TYPE.Normal:
                        {
                            itemTableDictionary.Add(itemTable.items[i].ItemID, itemTable.items[i]);
                            break;
                        }
                    case ITEM_TYPE.Equipment:
                        {
                            itemTableDictionary.Add(itemTable.items[i].ItemID, new EquipmentItem(itemTable.items[i]));
                            break;
                        }
                    case ITEM_TYPE.Consumption:
                        {
                            itemTableDictionary.Add(itemTable.items[i].ItemID, new ConsumptionItem(itemTable.items[i]));
                            break;
                        }
                    case ITEM_TYPE.Quest:
                        {
                            itemTableDictionary.Add(itemTable.items[i].ItemID, itemTable.items[i]);
                            break;
                        }
                }
                
            }
        }        
    }

    public void LoadPlayerData()
    {
        if (FileCheck(playerDataPath))
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

    public void SetCurrentCharacter(int index)
    {
        playerData.SelectCharacterIndex = index;
        SavePlayerData();
    }

    #region Property
    public Dictionary<int, float> LevelTable { get { return levelTableDictionary; } }
    public Dictionary<int, BaseItem> ItemTable { get { return itemTableDictionary; } }
    public PlayerData PlayerData { get { return playerData; } }
    public CharacterData SelectCharacterData
    {
        get { return playerData.CharacterDatas[playerData.SelectCharacterIndex]; }
    }
    #endregion
}
