using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPanel : UIPanel
{
    public enum TEXT
    {
        Quest_Title_Text,
        Quest_Tooltip_Text,
        Resonance_Stone_Reward_Amount_Text,
        Exp_Reward_Amount_Text
    }

    public enum BUTTON
    {
        Progress_Button,
        Completion_Button
    }

    private GameObject buttonRoot;

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
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        progressButton = GetButton((int)BUTTON.Progress_Button);
        completionButton = GetButton((int)BUTTON.Completion_Button);

        progressButton.onClick.AddListener(OnClickProgressButton);
        completionButton.onClick.AddListener(OnClickCompletionButton);

        questTitleText = GetText((int)TEXT.Quest_Title_Text);
        questTooltipText = GetText((int)TEXT.Quest_Tooltip_Text);
        moneyRewardText = GetText((int)TEXT.Resonance_Stone_Reward_Amount_Text);
        expRewardText = GetText((int)TEXT.Exp_Reward_Amount_Text);

        buttonRoot = Functions.FindChild<GameObject>(gameObject, "Content", true);
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
            questTooltipText.text = questPopUpButton.Quest.QuestTasks[questPopUpButton.Quest.TaskIndex].TaskTooltip;

        questTitleText.text = questPopUpButton.Quest.QuestTitle;
        moneyRewardText.text = questPopUpButton.Quest.RewardMoney.ToString();
        expRewardText.text = questPopUpButton.Quest.RewardExperience.ToString();
    }

    #region Button Event Function
    public void OnClickProgressButton()
    {
        Managers.QuestManager?.RequestAcceptList(this);
    }
    public void OnClickCompletionButton()
    {
        Managers.QuestManager?.RequestCompleteList(this);
    }
    #endregion

    #region Property
    public List<QuestPopupButton> QuestPopUpButtonList
    {
        get { return questPopUpButtonList; }
    }
    #endregion
}
