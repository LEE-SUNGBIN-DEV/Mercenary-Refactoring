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
            selectionButtonList[i].Initialize();
        }
    }

    public void CreateSelectionButton()
    {
        DialogueSelectionButton newSelectionButton = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_DIALOGUE_SELECTION_BUTTON, buttonRootTransform).GetComponent<DialogueSelectionButton>();
        newSelectionButton.Initialize();
        selectionButtonList.Add(newSelectionButton);
    }

    public bool IsSelecting()
    {
        if (isActiveAndEnabled)
            return true;
        else
            return false;
    }
    public void OpenPanel(NPCPanel npcPanel, DialogueData dialogueData)
    {
        if (gameObject.activeSelf == false)
        {
            // If count of button is not enough, Add Buttons
            while (selectionButtonList.Count < dialogueData.selections.Length)
                CreateSelectionButton();

            for (int i = 0; i < selectionButtonList.Count; i++)
            {
                if (i < dialogueData.selections.Length)
                {
                    selectionButtonList[i].ShowButton(npcPanel, dialogueData.selections[i], dialogueData.selectionTargetIDs[i]);
                    selectionButtonList[i].OnSelectComplete -= ClosePanel;
                    selectionButtonList[i].OnSelectComplete += ClosePanel;
                }

                else
                    selectionButtonList[i].HideButton();
            }

            Functions.RebuildLayout(layoutGroups);
            Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
            Managers.UIManager.SetCursorMode(CURSOR_MODE.VISIBLE);
            gameObject.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (gameObject.activeSelf == true)
        {
            for (int i = 0; i < selectionButtonList.Count; i++)
            {
                selectionButtonList[i].HideButton();
            }
            Managers.InputManager.PopInputMode();
            Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
            gameObject.SetActive(false);
        }
    }
}
