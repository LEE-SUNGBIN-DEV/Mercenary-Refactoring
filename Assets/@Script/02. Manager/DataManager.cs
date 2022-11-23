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

    private string weaponTablePath;
    private string helmetTablePath;
    private string armorTablePath;
    private string bootsTablePath;

    private string hpPotionTablePath;
    private string spPotionTablePath;

    [SerializeField] private PlayerData playerData;

    public void Initialize()
    {
        playerDataPath = Application.dataPath + "/Player_Data.json";
        levelTablePath = Application.dataPath + "/Level_Table.json";

        weaponTablePath = Application.dataPath + "/Weapon_Item_Table.json";
        helmetTablePath = Application.dataPath + "/Helmet_Item_Table.json";
        armorTablePath = Application.dataPath + "/Armor_Item_Table.json";
        bootsTablePath = Application.dataPath + "/Boots_Item_Table.json";

        hpPotionTablePath = Application.dataPath + "/HP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/SP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/SP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/SP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/SP_Potion_Table.json";
        spPotionTablePath = Application.dataPath + "/SP_Potion_Table.json";

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
    public void LoadItemTable<T>(string path) where T : BaseItem, new()
    {
        if (FileCheck(path))
        {
            string jsonItemData = File.ReadAllText(path);
            ItemTable<T> itemTable = JsonConvert.DeserializeObject<ItemTable<T>>(jsonItemData);

            for (int i = 0; i < itemTable.items.Length; ++i)
            {
                itemTable.items[i].ItemSprite = Managers.ResourceManager.LoadResourceSync<Sprite>("Sprite_Item_" + itemTable.items[i].ItemName + "_" + itemTable.items[i].ItemID);
                T item = new();
                item.Initialize(itemTable.items[i]);
                itemTableDictionary.Add(itemTable.items[i].ItemID, item);
            }
        }
    }
    public void LoadItemTable()
    {
        LoadItemTable<WeaponItem>(weaponTablePath);
        LoadItemTable<HelmetItem>(helmetTablePath);
        LoadItemTable<ArmorItem>(armorTablePath);
        LoadItemTable<BootsItem>(bootsTablePath);
        LoadItemTable<HPPotion>(hpPotionTablePath);
        LoadItemTable<SPPotion>(spPotionTablePath);
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
