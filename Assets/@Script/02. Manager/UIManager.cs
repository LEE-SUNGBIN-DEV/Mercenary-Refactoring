using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    public event UnityAction<bool> InteractPlayer;
    public event UnityAction<string> OnRequestNotice;

    private UICommonScene commonSceneUI;

    public void Initialize(Transform rootTransform)
    {
        commonSceneUI = Managers.ResourceManager.InstantiatePrefabSync("Prefab_UI_Common_Scene").GetComponent<UICommonScene>();
        commonSceneUI.transform.SetParent(rootTransform);
        commonSceneUI.Initialize();
        if (commonSceneUI.gameObject.activeSelf == false)
        {
            commonSceneUI.gameObject.SetActive(true);
        }
    }

    public void RequestNotice(string content)
    {
        OnRequestNotice(content);
    }

    public void RequestConfirm(string content, UnityAction action)
    {
        commonSceneUI.RequestConfirm(content, action);
    }

    public void NoticeQuestState(Quest quest)
    {
        if (quest.TaskIndex == 1)
        {
            RequestNotice("Äù½ºÆ® ¼ö¶ô");
        }

        if (quest.TaskIndex == quest.QuestTasks.Length)
        {
            RequestNotice("Äù½ºÆ® ¿Ï·á");
        }
    }

    #region Property
    public GameObject RootObject
    {
        get
        {
            GameObject rootObject = GameObject.Find("@UI Root");
            if (rootObject == null)
            {
                rootObject = new GameObject { name = "@UI Root" };
            }
            return rootObject;
        }
    }
    public UICommonScene CommonSceneUI { get { return commonSceneUI; } }
    #endregion
}