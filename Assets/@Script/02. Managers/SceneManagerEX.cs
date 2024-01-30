using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagerEX
{
    public event UnityAction OnSceneExit;
    public event UnityAction OnSceneEnter;

    private BaseScene currentScene;

    public void Initialize()
    {
        SceneManager.sceneLoaded -= SceneEnter;
        SceneManager.sceneLoaded += SceneEnter;

        SceneManager.sceneUnloaded -= SceneExit;
        SceneManager.sceneUnloaded += SceneExit;
    }

    public void SceneEnter(Scene scene, LoadSceneMode loadMode)
    {
        Managers.UIManager.UISystemPanelCanvas.FadePanel.FadeIn(Constants.TIME_UI_SCENE_DEFAULT_FADE);
        OnSceneEnter?.Invoke();
    }

    public void SceneExit(Scene scene)
    {
        OnSceneExit?.Invoke();
        currentScene.ExitScene();
    }

    public string GetSceneName(SCENE_ID sceneList)
    {
        return sceneList.GetEnumName();
    }

    // Load Scene Fade
    public void LoadSceneFade(string sceneName)
    {
        Managers.UIManager.UISystemPanelCanvas.FadePanel.FadeOut(Constants.TIME_UI_SCENE_DEFAULT_FADE, () => { SceneManager.LoadScene(sceneName); });
    }

    public void LoadSceneFade(SCENE_ID requestScene)
    {
        LoadSceneFade(requestScene.GetEnumName());
    }

    // Load Scene Ascyn (Loading Scene)
    public void LoadSceneAsync(string sceneName)
    {
        Managers.UIManager.UISystemPanelCanvas.FadePanel.FadeOut(Constants.TIME_UI_SCENE_DEFAULT_FADE, () => { LoadingScene.LoadScene(sceneName); });
    }
    public void LoadSceneAsync(SCENE_ID requestScene)
    {
        LoadSceneAsync(requestScene.GetEnumName());
    }

    #region Property
    public BaseScene CurrentScene { get { return currentScene; } set { currentScene = value; } }
    #endregion
}
