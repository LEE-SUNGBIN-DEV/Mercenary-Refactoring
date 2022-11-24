using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private StatData statData;
    private StatusData statusData;
    [SerializeField] private LocationData locationData;
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private EquipmentSlotData equipmentSlotData;
    [SerializeField] private QuestData questData;
    
    public CharacterData(CHARACTER_CLASS selectedClass)
    {
        statData = new StatData(selectedClass);
        locationData = new LocationData();
        inventoryData = new InventoryData();
        equipmentSlotData = new EquipmentSlotData();
        questData = new QuestData();
    }

    public void GetQuestReward(Quest quest)
    {
        inventoryData.Money += quest.RewardMoney;
        statData.CurrentExperience += quest.RewardExperience;
    }

    #region Property
    public StatData StatData { get { return statData; } set { statData = value; } }
    public StatusData StatusData { get { return statusData; } set { statusData = value; } }
    public LocationData LocationData { get { return locationData; } set { locationData = value; } }
    public InventoryData InventoryData { get { return inventoryData; } set { inventoryData = value; } }
    public EquipmentSlotData EquipmentSlotData { get { return equipmentSlotData; } set { equipmentSlotData = value; } }
    public QuestData QuestData { get { return questData; } set { questData = value; } }
    #endregion
}
