using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSelectionPanel : UIPanel
{
    private Transform buttonRootTransform;
    private List<DialogueSelectionButton> selectionButtonList;
    private LayoutGroup[] layoutGroups;

    public void Initialize()
    {
        buttonRootTransform = Functions.FindChild<Transform>(gameObject, "Button_Container", true);
        layoutGroups = GetComponentsInChildren<LayoutGroup>();
        selectionButtonList = new List<DialogueSelectionButton>();
        DialogueSelectionButton[] selectionButtons = GetComponentsInChildren<DialogueSelectionButton>();
        for (int i = 0; i < selectionButtons.Length; ++i)
        {
            selectionButtonList.Add(selectionButtons[i]);
        }
    }

    public void CreateSelectionButton()
    {
        DialogueSelectionButton newSelectionButton = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_DIALOGUE_SELECTION_BUTTON, buttonRootTransform).GetComponent<DialogueSelectionButton>();
        newSelectionButton.Initialize();
        selectionButtonList.Add(newSelectionButton);
    }

    public void OpenPanel(NPCPanel dialoguePanel, DialogueData dialogueData)
    {
        // If count of button is not enough, Add Buttons
        while (selectionButtonList.Count < dialogueData.selections.Length)
            CreateSelectionButton();

        for (int i = 0; i < selectionButtonList.Count; i++)
        {
            if (i < dialogueData.selections.Length)
                selectionButtonList[i].ShowButton(dialoguePanel, dialogueData.selections[i], dialogueData.selectionTargetIDs[i]);

            else
                selectionButtonList[i].HideButton();
        }

        Functions.RebuildLayout(layoutGroups);
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
