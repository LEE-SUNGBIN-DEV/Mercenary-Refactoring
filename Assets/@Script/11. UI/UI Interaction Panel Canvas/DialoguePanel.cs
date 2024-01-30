using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialoguePanel : UIPanel, IInteractionPanel
{
    public enum TEXT
    {
        Dialogue_Name_Text,
        Dialogue_Contents_Text
    }

    public event UnityAction<IInteractionPanel> OnOpenPanel;
    public event UnityAction<IInteractionPanel> OnClosePanel;

    [SerializeField] private TextMeshProUGUI dialogueNameText;
    [SerializeField] private TextMeshProUGUI dialogueContentText;

    protected override void Awake()
    {
        base.Awake();

        BindText(typeof(TEXT));

        dialogueNameText = GetText((int)TEXT.Dialogue_Name_Text);
        dialogueContentText = GetText((int)TEXT.Dialogue_Contents_Text);
    }

    private void OnEnable()
    {
        SetQuestList();
        SetFunctionButtonList();
    }

    private void OnDisable()
    {
    }

    public void OpenPanel(string dialogueID)
    {
        OpenPanel(Managers.DataManager.DialogueTable[dialogueID]);
    }
    public void OpenPanel(DialogueData dialogueData)
    {
        dialogueNameText.text = dialogueData.speaker;
        dialogueContentText.text = dialogueData.content;

        OnOpenPanel?.Invoke(this);
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        OnClosePanel?.Invoke(this);
        gameObject.SetActive(false);
    }
    public void SetQuestList()
    {

    }

    public void SetFunctionButtonList()
    {

    }

    #region Property
    public TextMeshProUGUI DialogueNameText { get { return dialogueNameText; } }
    public TextMeshProUGUI DialogueContentText { get { return dialogueContentText; } }
    #endregion
}
