using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private PlayerStatusData statusData;
    [SerializeField] private PlayerLocationData locationData;
    [SerializeField] private PlayerInventoryData inventoryData;
    [SerializeField] private PlayerEquipmentSlotData equipmentSlotData;
    [SerializeField] private PlayerQuestData questData;
    [SerializeField] private PlayerWayPointData wayPointData;

    public CharacterData()
    {
        statusData = new PlayerStatusData();
        locationData = new PlayerLocationData();
        inventoryData = new PlayerInventoryData();
        equipmentSlotData = new PlayerEquipmentSlotData();
        questData = new PlayerQuestData();
        wayPointData = new PlayerWayPointData();
    }

    public void Initialize()
    {
        statusData.Initialize();
        locationData.Initialize();
        inventoryData.Initialize();
        equipmentSlotData.Initialize();
        questData.Initialize();
        wayPointData.Initialize();
    }

    public void GetQuestReward(Quest quest)
    {
        inventoryData.Money += quest.RewardMoney;
        statusData.CurrentExp += quest.RewardExperience;
    }

    #region Property
    public PlayerStatusData StatusData { get { return statusData; } set { statusData = value; } }
    public PlayerLocationData LocationData { get { return locationData; } set { locationData = value; } }
    public PlayerInventoryData InventoryData { get { return inventoryData; } set { inventoryData = value; } }
    public PlayerEquipmentSlotData EquipmentSlotData { get { return equipmentSlotData; } set { equipmentSlotData = value; } }
    public PlayerQuestData QuestData { get { return questData; } set { questData = value; } }
    public PlayerWayPointData WayPointData { get { return wayPointData; } set { wayPointData = value; } }
    #endregion
}
