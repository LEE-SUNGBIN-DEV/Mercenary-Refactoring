using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private UITitleScene titleSceneUI;
    public override void Initialize()
    {
        base.Initialize();
        sceneType = SCENE_TYPE.Title;
        scene = SCENE_LIST.Title;

        titleSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Scene_Title).GetComponent<UITitleScene>();
        titleSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        titleSceneUI.transform.SetAsFirstSibling();
        titleSceneUI.Initialize();
        if (titleSceneUI.gameObject.activeSelf == false)
        {
            titleSceneUI.gameObject.SetActive(true);
        }

        Managers.GameManager.SetCursorMode(true);
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }

    public void PlayButtonClickSound()
    {
        Managers.AudioManager.PlaySFX("Button Click");
    }

    public void StartGame()
    {
        PlayButtonClickSound();
    }

    public void QuitGame()
    {
        PlayButtonClickSound();
        Application.Quit();
    }
}
