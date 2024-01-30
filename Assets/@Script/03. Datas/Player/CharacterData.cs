using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private CharacterStatusData statusData;
    [SerializeField] private CharacterInventoryData inventoryData;
    [SerializeField] private CharacterSkillData skillData;
    [SerializeField] private CharacterLocationData locationData;
    [SerializeField] private CharacterSceneData sceneData;
    [SerializeField] private CharacterQuestData questData;

    public void CreateData()
    {
        statusData = new CharacterStatusData();
        statusData.CreateData();

        inventoryData = new CharacterInventoryData();
        inventoryData.CreateData();

        skillData = new CharacterSkillData();
        skillData.CreateData();

        locationData = new CharacterLocationData();
        locationData.CreateData();

        sceneData = new CharacterSceneData();
        sceneData.CreateData();

        questData = new CharacterQuestData();
        questData.CreateData();
    }

    public void LoadData()
    {
        statusData?.LoadData();
        inventoryData?.LoadData();
        skillData?.LoadData();
        locationData?.LoadData();
        sceneData?.LoadData();
        questData?.LoadData();
    }

    public void UpdateData()
    {
        statusData?.UpdateData(this);
        inventoryData?.UpdateData(this);
        skillData?.UpdateData(this);
        locationData?.UpdateData(this);
        sceneData?.UpdateData(this);
        questData?.UpdateData(this);
    }

    public void SaveData()
    {
        statusData?.SaveData();
        inventoryData?.SaveData();
        skillData?.SaveData();
        locationData?.SaveData();
        sceneData?.SaveData();
        questData?.SaveData();
    }

    #region Property
    public CharacterInventoryData InventoryData { get { return inventoryData; } set { inventoryData = value; } }
    public CharacterSkillData SkillData { get { return skillData; } set { skillData = value; } }
    public CharacterStatusData StatusData { get { return statusData; } set { statusData = value; } }
    public CharacterLocationData LocationData { get { return locationData; } set { locationData = value; } }
    public CharacterSceneData SceneData { get { return sceneData; } set { sceneData = value; } }
    public CharacterQuestData QuestData { get { return questData; } set { questData = value; } }
    #endregion
}
