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
    private Button button;
    private TextMeshProUGUI titleText;

    public void Initialize()
    {
        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        titleText = GetText((int)TEXT.QuestTitleText);
        button = GetButton((int)BUTTON.QuestPopupButton);
        button.onClick.AddListener(OnClickButton);

        quest = null;
        titleText.text = null;
    }

    public void SetQuestButton(Quest targetQuest)
    {
        quest = targetQuest;
        titleText.text = quest.QuestData.questTitle;
    }

    public void OnClickButton()
    {
        OnClickPopupButton?.Invoke(this);
    }

    #region Property
    public Quest Quest { get { return quest; } }
    public Button Button { get { return button; } }
    public TextMeshProUGUI TitleText { get { return titleText; } }
    #endregion
}
