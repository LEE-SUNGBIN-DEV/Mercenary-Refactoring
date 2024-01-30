using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCQuestButton : UIBase
{
    public enum TEXT
    {
        Quest_Name_Text
    }
    public QuestData questData;
    public Button button;
    public TextMeshProUGUI questNameText;

    public void Initialize()
    {
        button = GetComponent<Button>();

        BindText(typeof(TEXT));
        questNameText = GetText((int)TEXT.Quest_Name_Text);
    }

    public void ShowQuestButton(QuestData questData)
    {
        this.questData = questData;
        questNameText.text = questData.questTitle;
        gameObject.SetActive(true);
    }
    public void HideQuestButton()
    {
        questData = null;
        gameObject.SetActive(false);
    }
}
