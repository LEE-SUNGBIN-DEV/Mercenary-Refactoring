using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SkillNodePanel : UIPanel, IFocusPanel
{
    public enum TEXT
    {
        Skill_Point_Text
    }
    public enum BUTTON
    {
        Skill_Initializing_Button
    }

    public enum RECT_TRANSFORM
    {
        Passive_Attack_Node_Contents,
        Passive_Defense_Node_Contents,
        Passive_Utility_Node_Contents,
    }
    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private bool isOpen;

    [Header("UI Elements")]
    private Dictionary<string, BaseSkillNode> nodeDict;
    private TextMeshProUGUI skillPointText;
    private Button skillInitializingButton;
    private SkillTooltipPanel skillTooltipPanel;
    private RectTransform passiveAttackNodeRectTransform;
    private RectTransform passiveDefenseNodeRectTransform;
    private RectTransform passiveUtilityNodeRectTransform;

    [Header("Data")]
    private CharacterSkillData characterSkillData;


    #region Private
    private void OnEnable()
    {
        ConnectData();
    }
    private void OnDisable()
    {
        DisconnectData();
    }

    private void ConnectData()
    {
        characterSkillData = Managers.DataManager.CurrentCharacterData.SkillData;
        if (characterSkillData != null)
        {
            characterSkillData.OnChangeSkillData -= UpdatePanelBySkillData;
            characterSkillData.OnChangeSkillData += UpdatePanelBySkillData;
            UpdatePanelBySkillData(characterSkillData);
        }
    }
    private void DisconnectData()
    {
        if (characterSkillData != null)
        {
            characterSkillData.OnChangeSkillData -= UpdatePanelBySkillData;
            characterSkillData = null;
        }
    }
    private void OnClickInitializeSkillButton()
    {
        if (characterSkillData != null)
        {
            characterSkillData.InitializeSkillData();
        }
    }
    #endregion

    public void Initialize()
    {
        isOpen = false;
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));
        BindObject<RectTransform>(typeof(RECT_TRANSFORM));

        skillTooltipPanel = GetComponentInChildren<SkillTooltipPanel>(true);
        skillTooltipPanel.Initialize();

        skillPointText = GetText((int)TEXT.Skill_Point_Text);

        skillInitializingButton = GetButton((int)BUTTON.Skill_Initializing_Button);
        skillInitializingButton.onClick.AddListener(OnClickInitializeSkillButton);

        passiveAttackNodeRectTransform = GetObject<RectTransform>((int)RECT_TRANSFORM.Passive_Attack_Node_Contents);
        passiveDefenseNodeRectTransform = GetObject<RectTransform>((int)RECT_TRANSFORM.Passive_Defense_Node_Contents);
        passiveUtilityNodeRectTransform = GetObject<RectTransform>((int)RECT_TRANSFORM.Passive_Utility_Node_Contents);

        nodeDict = new Dictionary<string, BaseSkillNode>();
        foreach(NodeData nodeData in Managers.DataManager.NodeTable.Values)
        {
            RectTransform parentRectTransform = null;
            switch(nodeData.nodeType)
            {
                case NODE_TYPE.PASSIVE_ATTACK:
                    parentRectTransform = passiveAttackNodeRectTransform;
                    break;
                case NODE_TYPE.PASSIVE_DEFENSE:
                    parentRectTransform = passiveDefenseNodeRectTransform;
                    break;
                case NODE_TYPE.PASSIVE_UTILITY:
                    parentRectTransform = passiveUtilityNodeRectTransform;
                    break;

                default:
                    parentRectTransform = null;
                    break;
            }
            BaseSkillNode newSkillNode = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_SKILL_NODE, parentRectTransform).GetComponent<BaseSkillNode>();
            newSkillNode.Initialize(nodeData, skillTooltipPanel);
            newSkillNode.gameObject.SetActive(false);

            nodeDict.Add(nodeData.nodeID, newSkillNode);
        }
    }

    public void UpdatePanelBySkillData(CharacterSkillData characterSkillData)
    {
        skillPointText.text = $"{Managers.DataManager.TextTable[Constants.TEXT_SKILL_POINT].textContent}: {characterSkillData.SkillPoint}";
        foreach (var nodeSkillDict in characterSkillData.NodeSkillDict)
        {
            nodeDict[nodeSkillDict.Key].UpdateNodeBySkillData(characterSkillData);
        }
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }
    public void OpenPanel()
    {
        isOpen = true;
        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.UI);
        Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
        FadeInPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE);
    }
    public void ClosePanel()
    {
        isOpen = false;
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
        Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
}

