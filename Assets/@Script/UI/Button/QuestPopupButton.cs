using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestPopupButton : UIBase
{
    public enum TEXT
    {
        QuestTitleText
    }

    public enum BUTTON
    {
        QuestPopupButton
    }
    public event UnityAction<QuestPopupButton> OnClickPopupButton;

    private Quest quest;
    private Button questInformationButton;
    private TextMeshProUGUI questTitleText;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        questTitleText = GetText((int)TEXT.QuestTitleText);
        questInformationButton = GetButton((int)BUTTON.QuestPopupButton);
        questInformationButton.onClick.AddListener(OnClickButton);

        quest = null;
        questTitleText.text = null;
    }

    public void SetQuestButton(Quest targetQuest)
    {
        quest = targetQuest;
        questTitleText.text = quest.QuestTitle;
    }

    public void OnClickButton()
    {
        OnClickPopupButton?.Invoke(this);
    }

    #region Property
    public Quest Quest { get { return quest; } }
    public Button QuestInformationButton { get { return questInformationButton; } }
    public TextMeshProUGUI QuestTitleText { get { return questTitleText; } }
    #endregion
}
