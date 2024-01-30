using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillHeaderTooltipModule : UIBase, ISkillTooltipModule
{
    public enum TEXT
    {
        Skill_Name_Text,
        Skill_Type_Text,
        Skill_Level_Text
    }

    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillTypeText;
    [SerializeField] private TextMeshProUGUI skillLevelText;

    public void Initialize()
    {
        BindText(typeof(TEXT));

        skillNameText = GetText((int)TEXT.Skill_Name_Text);
        skillTypeText = GetText((int)TEXT.Skill_Type_Text);
        skillLevelText = GetText((int)TEXT.Skill_Level_Text);
    }

    public void UpdateModule(SkillData skillData)
    {
        if (skillData != null)
        {
            skillNameText.text = skillData.skillName;

            switch (skillData.skillType)
            {
                case SKILL_TYPE.PASSIVE:
                    skillTypeText.text = Managers.DataManager.TextTable[Constants.TEXT_SKILL_TYPE_PASSIVE].textContent;
                    break;
                case SKILL_TYPE.ACTIVE:
                    skillTypeText.text = Managers.DataManager.TextTable[Constants.TEXT_SKILL_TYPE_ACTIVE].textContent;
                    break;
            }
            
            if (skillData.currentLevel == skillData.maxLevel)
                skillLevelText.text = $"<color=#C8A050>{skillData.currentLevel} / {skillData.maxLevel}</color>";
            else
                skillLevelText.text = $"{skillData.currentLevel} / {skillData.maxLevel}";

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
