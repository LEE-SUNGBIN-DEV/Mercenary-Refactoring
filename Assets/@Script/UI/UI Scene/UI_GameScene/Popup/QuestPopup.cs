using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPopup : UIPopup
{
    public enum TEXT
    {
        QuestTitleText,
        QuestTooltipText,
        MoneyRewardText,
        ExpRewardText
    }

    public enum BUTTON
    {
        ProgressButton,
        CompletionButton
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

        progressButton = GetButton((int)BUTTON.ProgressButton);
        completionButton = GetButton((int)BUTTON.CompletionButton);

        progressButton.onClick.AddListener(OnClickProgressButton);
        completionButton.onClick.AddListener(OnClickCompletionButton);

        questTitleText = GetText((int)TEXT.QuestTitleText);
        questTooltipText = GetText((int)TEXT.QuestTooltipText);
        moneyRewardText = GetText((int)TEXT.MoneyRewardText);
        expRewardText = GetText((int)TEXT.ExpRewardText);

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
