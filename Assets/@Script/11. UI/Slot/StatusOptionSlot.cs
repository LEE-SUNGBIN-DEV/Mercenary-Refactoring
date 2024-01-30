using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusOptionSlot : UIBase
{
    public enum TEXT
    {
        Status_Name_Text,
        Status_Value_Text
    }

    public enum IMAGE
    {
        Status_Icon
    }

    [SerializeField] private Image statusIcon;
    [SerializeField] private TextMeshProUGUI statusNameText;
    [SerializeField] private TextMeshProUGUI statusValueText;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));

        statusIcon = GetImage((int)IMAGE.Status_Icon);
        statusNameText = GetText((int)TEXT.Status_Name_Text);
        statusValueText = GetText((int)TEXT.Status_Value_Text);
    }

    public void ShowSlot(StatOption statOption)
    {
        statusNameText.text = statOption.StatOptionName;
        switch(statOption.statData.statUnitType)
        {
            case STAT_UNIT_TYPE.NORMAL:
                statusValueText.text = $"{Functions.GetStatusValueString(statOption.value)}{statOption.GetStatValueUnit()}";
                break;
            case STAT_UNIT_TYPE.RATE:
                statusValueText.text = $"{Functions.GetStatusValueString(statOption.value)}%";
                break;
        }
        gameObject.SetActive(true);
    }
    public void HideSlot()
    {
        gameObject.SetActive(false);
    }
}
