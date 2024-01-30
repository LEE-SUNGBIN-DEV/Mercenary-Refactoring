using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResponseWaterTooltipModule : UIBase, IItemTooltipModule
{
    public enum TEXT
    {
        Response_Count_Value_Text,
        Remaining_Count_Value_Text,
        HP_Recovery_Value_Text,
        SP_Recovery_Value_Text,
    }

    private TextMeshProUGUI responseCountValueText;
    private TextMeshProUGUI remainingCountValueText;
    private TextMeshProUGUI hpRecoveryValueText;
    private TextMeshProUGUI spRecoveryValueText;

    public void Initialize()
    {
        BindText(typeof(TEXT));

        responseCountValueText = GetText((int)TEXT.Response_Count_Value_Text);
        remainingCountValueText = GetText((int)TEXT.Remaining_Count_Value_Text);
        hpRecoveryValueText = GetText((int)TEXT.HP_Recovery_Value_Text);
        spRecoveryValueText = GetText((int)TEXT.SP_Recovery_Value_Text);
    }

    public void UpdateModule<T>(T item, CharacterInventoryData inventoryData) where T : BaseItem
    {
        if(item is ResponseWaterItem responseWaterItem)
        {
            responseCountValueText.text = responseWaterItem.ResponseCount.ToString();
            remainingCountValueText.text = $"({inventoryData.ResponseWaterRemainingCount}/{responseWaterItem.MaxCount})";
            hpRecoveryValueText.text = $"{Functions.GetStatusValueString(responseWaterItem.HPRecoveryPercentage)}%";
            spRecoveryValueText.text = $"{Functions.GetStatusValueString(responseWaterItem.SPRecoveryPercentage)}%";
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
