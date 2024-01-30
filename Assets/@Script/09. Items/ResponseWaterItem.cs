using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseWaterItem : BaseItem, IConsumableItem
{
    [SerializeField] private ResponseWaterData responseWaterData;

    public ResponseWaterItem(string itemID) : base(itemID)
    {
    }
    public override void LoadFromItemID(string itemID)
    {
        base.LoadFromItemID(itemID);
        Managers.DataManager.ResponseWaterTable.TryGetValue(itemData?.itemID, out responseWaterData);
    }

    public override void LoadFromSaveData(ItemSaveData itemSaveData)
    {
        base.LoadFromSaveData(itemSaveData);
        Managers.DataManager.ResponseWaterTable.TryGetValue(itemData?.itemID, out responseWaterData);
    }

    public void Consume(CharacterStatusData statusData)
    {
        statusData.RecoverHP(responseWaterData.hpRecoveryPercentage, VALUE_TYPE.PERCENTAGE);
        statusData.RecoverStamina(responseWaterData.spRecoveryPercentage, VALUE_TYPE.PERCENTAGE);
    }

    public ResponseWaterData ResponseWaterData { get { return responseWaterData; } }
    public int MaxCount { get { return responseWaterData.maxCount; } }
    public int ResponseCount { get { return responseWaterData.responseCount; } }
    public float HPRecoveryPercentage { get { return responseWaterData.hpRecoveryPercentage; } }
    public float SPRecoveryPercentage { get { return responseWaterData.spRecoveryPercentage; } }
}
