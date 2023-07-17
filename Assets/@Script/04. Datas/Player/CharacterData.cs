using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private CharacterStatusData statusData;
    [SerializeField] private CharacterSkillData skillData;
    [SerializeField] private CharacterLocationData locationData;
    [SerializeField] private CharacterSceneData sceneData;
    [SerializeField] private CharacterInventoryData inventoryData;
    [SerializeField] private CharacterQuestData questData;

    public CharacterData()
    {
        statusData = new CharacterStatusData();
        skillData = new CharacterSkillData();
        locationData = new CharacterLocationData();
        sceneData = new CharacterSceneData();
        inventoryData = new CharacterInventoryData();
        questData = new CharacterQuestData();
    }

    public void CreateCharacterData()
    {
        statusData.CreateData();
        skillData.CreateData();
        locationData.CreateData();
        sceneData.CreateData();
        inventoryData.CreateData();
        questData.CreateData();
    }

    public void LoadData()
    {

    }

    public void GetQuestReward(Quest quest)
    {
        inventoryData.ResonanceStone += quest.RewardMoney;
        statusData.CurrentExp += quest.RewardExperience;
    }

    #region Property
    public CharacterStatusData StatusData { get { return statusData; } set { statusData = value; } }
    public CharacterSkillData SkillData { get { return skillData; } set { skillData = value; } }
    public CharacterLocationData LocationData { get { return locationData; } set { locationData = value; } }
    public CharacterSceneData SceneData { get { return sceneData; } set { sceneData = value; } }
    public CharacterInventoryData InventoryData { get { return inventoryData; } set { inventoryData = value; } }
    public CharacterQuestData QuestData { get { return questData; } set { questData = value; } }
    #endregion
}
