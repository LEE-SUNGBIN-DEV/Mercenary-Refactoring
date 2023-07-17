using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillPrecondition
{
    private int skillID;
    private int conditionLevel;

    public SkillPrecondition(int skillID, int conditionLevel)
    {
        this.skillID = skillID;
        this.conditionLevel = conditionLevel;
    }

    public bool IsComplete(CharacterSkillData skillData)
    {
        return skillData.GetSkillLevel(skillID) >= conditionLevel;
    }
}

public abstract class BaseSkillNode : UIBase, IPointerEnterHandler, IPointerExitHandler
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
        Skill_Level_Plus_Button
    }

    protected CharacterData characterData;

    protected RectTransform rectTransform;
    protected SkillTooltipPanel tooltipPanel;
    protected SkillPrecondition skillPrecondition;

    [Header("Skill Information")]
    [SerializeField] protected int skillID;
    [SerializeField] protected string skillName;
    [SerializeField] protected int maxSkillLevel;
    [SerializeField] protected int currentSkillLevel;
    [SerializeField] protected string skillDescription;

    protected TextMeshProUGUI skillLevelText;

    protected Image skillImage;
    protected Button skillLevelPlusButton;

    public abstract void ApplySkillAbility();
    public abstract void ReleaseSkillAbility();
    public abstract string GetSkillDescription();

    public virtual void Initialize(CharacterData characterData, SkillTooltipPanel tooltipPanel)
    {
        this.characterData = characterData;
        this.tooltipPanel = tooltipPanel;

        TryGetComponent(out rectTransform);

        BindText(typeof(TEXT));
        BindImage(typeof(IMAGE));
        BindButton(typeof(BUTTON));

        skillLevelText = GetText((int)TEXT.Skill_Level_Text);

        skillImage = GetImage((int)IMAGE.Skill_Image);

        skillLevelPlusButton = GetButton((int)BUTTON.Skill_Level_Plus_Button);
        skillLevelPlusButton.onClick.AddListener(LevelUpSkill);

        characterData.SkillData.OnChangeSkillData += UpdateSkillNode;
    }

    private void OnDestroy()
    {
        if(characterData != null)
            characterData.SkillData.OnChangeSkillData -= UpdateSkillNode;
    }

    public void LevelUpSkill()
    {
        if (characterData.StatusData.SkillPoint > 0)
        {
            characterData.StatusData.SkillPoint--;
            characterData.SkillData.LevelUpSkill(skillID);
        }
    }

    public virtual void UpdateSkillNode(CharacterSkillData skillData)
    {
        ReleaseSkillAbility();
        currentSkillLevel = skillData.GetSkillLevel(skillID);
        ApplySkillAbility();

        if (skillPrecondition == null || skillPrecondition.IsComplete(skillData))
            ActiveNode();
        else
            InActiveNode();

        if (IsMaster())
            skillLevelText.text = $"<color=#C8A050>{currentSkillLevel}/{maxSkillLevel}</color>";
        else
            skillLevelText.text = $"{currentSkillLevel}/{maxSkillLevel}";

        GetSkillDescription();
    }

    public bool IsMaster()
    {
        if (currentSkillLevel == maxSkillLevel)
            return true;

        return false;
    }

    public void ActiveNode()
    {
        skillImage.color = Functions.SetColor(Color.white, 1f);

        if (characterData.StatusData.SkillPoint == 0 || IsMaster())
            skillLevelPlusButton.gameObject.SetActive(false);
        else
            skillLevelPlusButton.gameObject.SetActive(true);
    }

    public void InActiveNode()
    {
        skillImage.color = Functions.SetColor(new Color(64, 64, 64, 1f));
        skillLevelPlusButton.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.HideTooltip();
    }

    public RectTransform RectTransform { get { return rectTransform; } }
    public string SkillName { get { return skillName; } }
    public string SkillDescription { get { return skillDescription; } }
    public TextMeshProUGUI SkillLevelText { get { return skillLevelText; } }
}
