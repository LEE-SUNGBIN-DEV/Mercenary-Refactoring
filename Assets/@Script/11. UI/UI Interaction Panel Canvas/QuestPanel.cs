using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPanel : UIPanel, IFocusPanel
{
    public enum TEXT
    {
        Quest_Title_Text,
        Quest_Description_Text,
        Response_Stone_Reward_Value_Text,
        Exp_Reward_Value_Text
    }

    public enum BUTTON
    {
        Progress_Button,
        Completion_Button
    }
    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    private bool isOpen;
    private Transform buttonRoot;

    [Header("Popup Button List")]
    [SerializeField] private List<QuestPopupButton> questPopUpButtonList;

    //Buttons
    private Button progressButton;
    private Button completionButton;

    //Select Quest Texts
    private TextMeshProUGUI questTitleText;
    private TextMeshProUGUI questTooltipText;
    private TextMeshProUGUI moneyRewardText;
    private TextMeshProUGUI expRewardText;

    public void Initialize()
    {
        isOpen = false;
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        progressButton = GetButton((int)BUTTON.Progress_Button);
        completionButton = GetButton((int)BUTTON.Completion_Button);

        progressButton.onClick.AddListener(OnClickProgressButton);
        completionButton.onClick.AddListener(OnClickCompletionButton);

        questTitleText = GetText((int)TEXT.Quest_Title_Text);
        questTooltipText = GetText((int)TEXT.Quest_Description_Text);
        moneyRewardText = GetText((int)TEXT.Response_Stone_Reward_Value_Text);
        expRewardText = GetText((int)TEXT.Exp_Reward_Value_Text);

        buttonRoot = Functions.FindChild<Transform>(gameObject, "Quest_Buttons", true);
    }

    public void OnClickProgressButton()
    {

    }
    public void OnClickCompletionButton()
    {

    }

    public void CreateQuestButton()
    {
        QuestPopupButton newQuestPopupButton = Managers.ResourceManager?.InstantiatePrefabSync("Prefab_Quest_Popup_Button", buttonRoot.transform).GetComponent<QuestPopupButton>();
        newQuestPopupButton.Initialize();
        newQuestPopupButton.OnClickPopupButton -= ShowQuestInformation;
        newQuestPopupButton.OnClickPopupButton += ShowQuestInformation;
        questPopUpButtonList.Add(newQuestPopupButton);
    }

    public void ShowQuestInformation(QuestPopupButton questPopUpButton)
    {
        if (questPopUpButton.Quest.QuestState == QUEST_STATE.COMPLETE)
            questTooltipText.text = "This is a completed quest.";
        else
            questTooltipText.text = questPopUpButton.Quest.QuestData.questDescription;

        questTitleText.text = questPopUpButton.Quest.QuestData.questTitle;
        moneyRewardText.text = questPopUpButton.Quest.QuestData.rewardResponseStone.ToString();
        expRewardText.text = questPopUpButton.Quest.QuestData.rewardExperience.ToString();
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

    #region Property
    public List<QuestPopupButton> QuestPopUpButtonList { get { return questPopUpButtonList; } }
    #endregion
}
