using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSelectionButton : MonoBehaviour
{
    public event UnityAction OnSelectComplete;

    private Button selectionButton;
    private TextMeshProUGUI selectionText;

    public void Initialize()
    {
        selectionButton = GetComponent<Button>();
        selectionText = GetComponentInChildren<TextMeshProUGUI>();
        selectionText.text = null;
    }

    public void ShowButton(IDialogueablePanel dialogueablePanel, string selectionContent, string selectionTargetID)
    {
        selectionText.text = selectionContent;
        selectionButton.onClick.AddListener(() => OnClickSelectionButton(dialogueablePanel, selectionTargetID));
    }
    public void HideButton()
    {
        OnSelectComplete = null;
        selectionText.text = null;
        selectionButton.onClick.RemoveAllListeners();
    }

    public void OnClickSelectionButton(IDialogueablePanel dialogueablePanel, string selectionTargetID)
    {
        dialogueablePanel.SetDialogue(Managers.DataManager.DialogueTable[selectionTargetID]);
        OnSelectComplete?.Invoke();
    }
}
