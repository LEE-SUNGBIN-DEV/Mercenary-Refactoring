using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillTooltipPanel : UIBase
{
    public enum TEXT
    {
        Skill_Name_Text,
        Skill_Level_Text,
        Skill_Description_Text,
    }

    private RectTransform rectTransform;
    private TextMeshProUGUI skillNameText;
    private TextMeshProUGUI skillLevelText;
    private TextMeshProUGUI skillDescriptionText;

    public void Initialize()
    {
        TryGetComponent(out rectTransform);
        BindText(typeof(TEXT));

        skillNameText = GetText((int)TEXT.Skill_Name_Text);
        skillLevelText = GetText((int)TEXT.Skill_Level_Text);
        skillDescriptionText = GetText((int)TEXT.Skill_Description_Text);
    }

    public void ShowTooltip(BaseSkillNode skillNode)
    {
        skillNameText.text = skillNode.SkillName;
        skillLevelText.text = skillNode.SkillLevelText.text;
        skillDescriptionText.text = skillNode.SkillDescription;

        rectTransform.position = skillNode.RectTransform.position + new Vector3(skillNode.RectTransform.sizeDelta.x * 0.5f, skillNode.RectTransform.sizeDelta.y * 0.5f, 0);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
