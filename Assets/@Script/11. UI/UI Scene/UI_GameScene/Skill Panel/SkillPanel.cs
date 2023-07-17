using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPanel : UIPanel
{
    public enum TEXT
    {
        Skill_Point_Text
    }
    public enum BUTTON
    {
        Skill_Initializing_Button
    }

    private CharacterData characterData;
    private TextMeshProUGUI skillPointText;
    private Button skillInitializingButton;
    private SkillTooltipPanel skillTooltipPanel;

    // Attack Node
    private BaseSkillNode[] skillNodes;

    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;
        this.characterData.StatusData.OnChangeStatusData += UpdateUI;

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        skillPointText = GetText((int)TEXT.Skill_Point_Text);
        skillPointText.text = characterData.StatusData.SkillPoint.ToString();

        skillTooltipPanel = GetComponentInChildren<SkillTooltipPanel>(true);
        skillTooltipPanel.Initialize();

        skillInitializingButton = GetButton((int)BUTTON.Skill_Initializing_Button);
        skillInitializingButton.onClick.AddListener(InitializeSkill);

        // Attack Nodes
        skillNodes = GetComponentsInChildren<BaseSkillNode>(true);

        for (int i = 0; i < skillNodes.Length; ++i)
            skillNodes[i].Initialize(characterData, skillTooltipPanel);
    }

    private void OnDestroy()
    {
        if(characterData != null)
            characterData.StatusData.OnChangeStatusData -= UpdateUI;
    }

    public void UpdateUI(CharacterStatusData statusData)
    {
        skillPointText.text = $"보유 스킬 포인트: {statusData.SkillPoint}";
    }

    public void InitializeSkill()
    {
        characterData.SkillData.InitializeSkillData();
        characterData.StatusData.InitializeSkillPoint();
    }
}
