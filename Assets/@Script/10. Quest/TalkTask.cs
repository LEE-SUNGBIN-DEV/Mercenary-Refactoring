using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkTask : QuestTask
{
    [SerializeField] private string npcID;
    [SerializeField] private string[] dialogues;

    public TalkTask(TaskData taskData) : base(taskData)
    {
    }

    public override void StartTask()
    {
        base.StartTask();
    }

    public override void EndTask()
    {
        base.EndTask();
    }

    public void Action(string dialogueID)
    {
        for (int i = 0; i < targetIDs.Length; i++)
        {
            if (dialogueID == targetIDs[i])
            {
                ++currentAmounts[i];
                Managers.UIManager.UISystemPanelCanvas.SystemMessagePanel.OpenPanel($"{dialogueID}¿Í ´ëÈ­ {currentAmounts[i] + "/" + targetAmounts[i]}");
            }
        }
    }

    #region Property
    public string NpcID
    {
        get { return npcID; }
    }
    public string[] Dialogues
    {
        get { return dialogues; }
    }
    #endregion
}
