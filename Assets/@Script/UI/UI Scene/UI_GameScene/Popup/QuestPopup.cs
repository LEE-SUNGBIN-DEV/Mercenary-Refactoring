using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPopup : UIPopup
{
    public static event UnityAction<QuestPopup> OnClickAcceptButton;
    public static event UnityAction<QuestPopup> OnClickCompleteButton;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonCanvas;

    [SerializeField] private int questCount;
    [SerializeField] private List<QuestPopupButton> questPopUpButtonList;

    [Header("Select Quest Information")]
    [SerializeField] private TextMeshProUGUI selectQuestTitleText;
    [SerializeField] private TextMeshProUGUI selectQuestDescription;
    [SerializeField] private TextMeshProUGUI selectMoneyRewardText;
    [SerializeField] private TextMeshProUGUI selectExperienceRewardText;

    [System.Serializable]
    public class QuestPopupButton
    {
        public static event UnityAction<QuestPopupButton> onClickButton;

        public bool isActive;
        public Quest quest;
        public Button button;
        public TextMeshProUGUI buttonText;

        public void OnClickButton()
        {
            onClickButton(this);
        }
    }

    public void Initialize()
    {
        QuestPopupButton.onClickButton -= SetQuestInformation;
        QuestPopupButton.onClickButton += SetQuestInformation;

        for (int i = 0; i < questCount; ++i)
        {
            CreateQuestButton();
        }

        SetAcceptList();
    }

    public void SetQuestInformation(QuestPopupButton questPopUpButton)
    {
        if (questPopUpButton.quest.questState == QUEST_STATE.COMPLETE)
        {
            selectQuestDescription.text = "완료한 퀘스트입니다.";
        }
        else
        {
            selectQuestDescription.text = questPopUpButton.quest.QuestTasks[questPopUpButton.quest.TaskIndex].TaskDescription;
        }

        selectQuestTitleText.text = questPopUpButton.quest.QuestTitle;
        selectMoneyRewardText.text = questPopUpButton.quest.RewardMoney.ToString();
        selectExperienceRewardText.text = questPopUpButton.quest.RewardExperience.ToString();
    }

    public void CreateQuestButton()
    {
        Button newButton = Instantiate(buttonPrefab).GetComponent<Button>();
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.SetParent(buttonCanvas.transform);
        rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newButton.gameObject.SetActive(false);

        QuestPopupButton newQuestPopUpButton = new QuestPopupButton();
        newQuestPopUpButton.isActive = false;
        newQuestPopUpButton.quest = null;
        newQuestPopUpButton.button = newButton;
        newQuestPopUpButton.buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        questPopUpButtonList.Add(newQuestPopUpButton);
    }

    public void InitializeQuestPopup()
    {
        for(int i=0; i<questPopUpButtonList.Count; ++i)
        {
            questPopUpButtonList[i].isActive = false;
            questPopUpButtonList[i].quest = null;
            questPopUpButtonList[i].buttonText.text = null;
            questPopUpButtonList[i].button.gameObject.SetActive(false);
        }

        selectQuestTitleText.text = null;
        selectQuestDescription.text = null;
        selectMoneyRewardText.text = null;
        selectExperienceRewardText.text = null;
    }

    public void SetAcceptList()
    {
        InitializeQuestPopup();
        OnClickAcceptButton(this);
    }
    public void SetCompleteList()
    {
        InitializeQuestPopup();
        OnClickCompleteButton(this);
    }

    #region Property
    public List<QuestPopupButton> QuestPopUpButtonList
    {
        get { return questPopUpButtonList; }
    }
    #endregion
}
