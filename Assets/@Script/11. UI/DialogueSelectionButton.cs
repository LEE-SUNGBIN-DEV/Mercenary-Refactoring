using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSelectionButton : MonoBehaviour
{
    private Button selectionButton;
    private TextMeshProUGUI selectionText;

    public void Initialize()
    {
        selectionButton = GetComponent<Button>();
        selectionText = GetComponentInChildren<TextMeshProUGUI>();
        selectionText.text = null;
    }

    public void ShowButton(NPCPanel dialoguePanel, string selectionContent, string selectionTargetID)
    {
        selectionText.text = selectionContent;
        selectionButton.onClick.AddListener(() => dialoguePanel.OpenPanel(selectionTargetID));
    }
    public void HideButton()
    {
        selectionText.text = null;
        selectionButton.onClick.RemoveAllListeners();
    }
}
