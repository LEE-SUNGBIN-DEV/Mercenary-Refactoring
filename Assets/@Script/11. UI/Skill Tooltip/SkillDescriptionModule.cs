using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillDescriptionModule : UIBase, ISkillTooltipModule
{
    public enum TEXT
    {
        Current_Level_Description_Text,
        Next_Level_Description_Text
    }

    [SerializeField] private TextMeshProUGUI currentLevelDescriptionText;
    [SerializeField] private TextMeshProUGUI nextLevelDescriptionText;

    public void Initialize()
    {
        BindText(typeof(TEXT));

        currentLevelDescriptionText = GetText((int)TEXT.Current_Level_Description_Text);
        nextLevelDescriptionText = GetText((int)TEXT.Next_Level_Description_Text);
    }

    public void UpdateModule(SkillData skillData)
    {
        if (skillData != null)
        {
            currentLevelDescriptionText.text = $"- {Managers.DataManager.TextTable[Constants.TEXT_SKILL_CURRENT_LEVEL].textContent} \n{skillData.skillDescription}";
            nextLevelDescriptionText.text = $"- {Managers.DataManager.TextTable[Constants.TEXT_SKILL_NEXT_LEVEL].textContent} \n{skillData.GetNextSkillData()?.skillDescription}";

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
