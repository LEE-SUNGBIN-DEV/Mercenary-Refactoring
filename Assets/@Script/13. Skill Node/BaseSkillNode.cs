using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseSkillNode : UIBase, ITooltipSkillNode, IPointerEnterHandler, IPointerExitHandler
{
    public enum TEXT
    {
        Skill_Level_Text,
    }
    public enum IMAGE
    {
        Skill_Image
    }
    public enum BUTTON
    {
        Skill_Level_Up_Button
    }

    [Header("UI Elements")]
    protected RectTransform rectTransform;
    protected TextMeshProUGUI skillLevelText;
    protected Image skillImage;
    protected Button skillLevelUpButton;
    protected SkillTooltipPanel tooltipPanel;

    [Header("Data")]
    protected NodeData nodeData;

    #region Private
    private void ClickSkillLevelUpButton()
    {
        Managers.DataManager.CurrentCharacterData.SkillData.LevelUpByNodeID(nodeData.nodeID);
    }
    private bool IsCompletePreconditions(CharacterSkillData characterSkillData, SkillData skillData)
    {
        if (skillData.nextLevelID != null)
        {
            SkillData nextSkillData = Managers.DataManager.SkillTable[skillData.nextLevelID];
            for (int i = 0; i < nextSkillData.preconditionIDs.Length; i++)
            {
                if (characterSkillData.IsLock(nextSkillData.preconditionIDs[i]))
                {
                    skillImage.color = new Color32(64, 64, 64, 255);
                    return false;
                }
            }
        }
        skillImage.color = new Color(255, 255, 255, 255);
        return true;
    }
    #endregion

    public void Initialize(NodeData nodeData, SkillTooltipPanel tooltipPanel)
    {
        this.tooltipPanel = tooltipPanel;

        TryGetComponent(out rectTransform);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        skillLevelText = GetText((int)TEXT.Skill_Level_Text);
        skillImage = GetImage((int)IMAGE.Skill_Image);

        skillLevelUpButton = GetButton((int)BUTTON.Skill_Level_Up_Button);
        skillLevelUpButton.onClick.AddListener(ClickSkillLevelUpButton);

        this.nodeData = nodeData;
        rectTransform.localPosition = nodeData.GetNodeTransform();
    }

    public void UpdateNodeBySkillData(CharacterSkillData characterSkillData)
    {
        SkillData skillData = characterSkillData.GetSkillDataFromNodeID(nodeData.nodeID);
        if (skillData != null)
        {
            // Image
            skillImage.sprite = skillData.GetSprite();

            // Text
            if (skillData.IsMaxLevel())
                skillLevelText.text = $"<color=#C8A050>{skillData.currentLevel} / {skillData.maxLevel}</color>";
            else
                skillLevelText.text = $"{skillData.currentLevel} / {skillData.maxLevel}";

            // Button
            if (!IsCompletePreconditions(characterSkillData, skillData) || characterSkillData.SkillPoint == 0 || skillData.IsMaxLevel())
                skillLevelUpButton.gameObject.SetActive(false);
            else
                skillLevelUpButton.gameObject.SetActive(true);

            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }

    // Interface Functions
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
    public void ShowTooltip()
    {
        tooltipPanel.ShowTooltip(Managers.DataManager.CurrentCharacterData.SkillData.GetSkillDataFromNodeID(nodeData.nodeID), rectTransform);
    }
    public void HideTooltip()
    {
        tooltipPanel.HideTooltip();
    }

    public TextMeshProUGUI SkillLevelText { get { return skillLevelText; } }
    public SkillTooltipPanel TooltipPanel { get { return tooltipPanel; } set { tooltipPanel = value; } }
}
