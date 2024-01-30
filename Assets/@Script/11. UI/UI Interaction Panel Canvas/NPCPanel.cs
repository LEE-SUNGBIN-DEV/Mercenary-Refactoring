using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class NPCPanel : UIPanel, IInteractionPanel
{
    public enum TEXT
    {
        NPC_Name_Text,
        NPC_Contents_Text
    }

    public enum BUTTON
    {
        NPC_Exit_Button
    }

    public event UnityAction<IInteractionPanel> OnOpenPanel;
    public event UnityAction<IInteractionPanel> OnClosePanel;

    private LayoutGroup[] layoutGroups;

    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcContentText;
    [SerializeField] private Button exitButton;

    [SerializeField] private List<QuestData> questDatas;
    [SerializeField] private List<NPCQuestButton> npcQuestButtonList;
    [SerializeField] private List<Button> npcButtonList;

    public void Initialize()
    {
        layoutGroups = GetComponentsInChildren<LayoutGroup>(true);

        BindText(typeof(TEXT));
        BindButton(typeof(BUTTON));

        npcNameText = GetText((int)TEXT.NPC_Name_Text);
        npcContentText = GetText((int)TEXT.NPC_Contents_Text);
        exitButton = GetButton((int)BUTTON.NPC_Exit_Button);

        exitButton.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        UpdateNPCQuestList();
        UpdateFunctionButtonList();
        Functions.RebuildLayout(layoutGroups);
    }

    private void OnDisable()
    {
        npcButtonList.Clear();
        npcQuestButtonList.Clear();
    }

    public void OpenPanel(string dialogueID)
    {
        OpenPanel(Managers.DataManager.DialogueTable[dialogueID]);
    }
    public void OpenPanel(DialogueData dialogueData)
    {
        if (gameObject.activeSelf == false)
        {
            OnOpenPanel?.Invoke(this);
            gameObject.SetActive(true);
        }
        npcNameText.text = dialogueData.speaker;
        npcContentText.text = dialogueData.content;

    }
    public void ClosePanel()
    {
        OnClosePanel?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void UpdateNPCQuestList()
    {
        for (int i = 0; i < npcQuestButtonList.Count; ++i)
        {
            npcQuestButtonList[i].HideQuestButton();
            npcQuestButtonList[i].ShowQuestButton(questDatas[i]);
        }
    }

    public void UpdateFunctionButtonList()
    {

    }

    #region Property
    public TextMeshProUGUI NPCNameText { get { return npcNameText; } }
    public TextMeshProUGUI NPCContentText { get { return npcContentText; } }
    public List<Button> NPCButtonList { get { return npcButtonList; } }
    #endregion
}
