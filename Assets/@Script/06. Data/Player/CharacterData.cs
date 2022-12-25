using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private StatusData statusData;
    [SerializeField] private LocationData locationData;
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private EquipmentSlotData equipmentSlotData;
    [SerializeField] private CharacterQuestData questData;
    
    public CharacterData()
    {
        statusData = new StatusData();
        locationData = new LocationData();
        inventoryData = new InventoryData();
        equipmentSlotData = new EquipmentSlotData();
        questData = new CharacterQuestData();
    }

    public void Initialize(CHARACTER_TYPE selectedClass)
    {
        statusData.Initialize(selectedClass);
        locationData.Initialize();
        inventoryData.Initialize();
        equipmentSlotData.Initialize();
        questData.Initialize();
    }

    public void GetQuestReward(Quest quest)
    {
        inventoryData.Money += quest.RewardMoney;
        statusData.CurrentExp += quest.RewardExperience;
    }

    #region Property
    public StatusData StatusData { get { return statusData; } set { statusData = value; } }
    public LocationData LocationData { get { return locationData; } set { locationData = value; } }
    public InventoryData InventoryData { get { return inventoryData; } set { inventoryData = value; } }
    public EquipmentSlotData EquipmentSlotData { get { return equipmentSlotData; } set { equipmentSlotData = value; } }
    public CharacterQuestData QuestData { get { return questData; } set { questData = value; } }
    #endregion
}
