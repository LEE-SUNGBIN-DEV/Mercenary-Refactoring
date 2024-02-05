using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialoguePanel : UIPanel, IFocusPanel, IDialogueablePanel
{
    public enum TEXT
    {
        Dialogue_Name_Text,
        Dialogue_Contents_Text
    }

    public event UnityAction<IFocusPanel> OnOpenFocusPanel;
    public event UnityAction<IFocusPanel> OnCloseFocusPanel;

    [SerializeField] private TextMeshProUGUI dialogueNameText;
    [SerializeField] private TextMeshProUGUI dialogueContentText;

    [SerializeField] private string dialogueID;


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

    public void SetDialogue(DialogueData dialogueData)
    {
        dialogueID = dialogueData.dialogueID;
        dialogueNameText.text = dialogueData.speaker;
        dialogueContentText.text = dialogueData.content;
    }
    public void OpenPanel(string dialogueID)
    {
        OpenPanel(Managers.DataManager.DialogueTable[dialogueID]);
    }
    public void OpenPanel(DialogueData dialogueData)
    {
        dialogueNameText.text = dialogueData.speaker;
        dialogueContentText.text = dialogueData.content;

        OnOpenFocusPanel?.Invoke(this);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        OnCloseFocusPanel?.Invoke(this);
        Managers.InputManager.PopInputMode();
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
