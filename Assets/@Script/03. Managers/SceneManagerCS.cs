using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagerCS
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
        Managers.UIManager.CommonSceneUI.FadeIn(Constants.TIME_UI_SCENE_DEFAULT_FADE);
        OnSceneEnter?.Invoke();
    }

    public void SceneExit(Scene scene)
    {
        OnSceneExit?.Invoke();
        currentScene.ExitScene();
    }

    public string GetSceneName(SCENE_LIST sceneList)
    {
        return sceneList.GetEnumName();
    }

    // Load Scene Fade
    public void LoadSceneFade(string sceneName)
    {
        Managers.UIManager.CommonSceneUI.FadeOut(Constants.TIME_UI_SCENE_DEFAULT_FADE, () => { SceneManager.LoadScene(sceneName); });
    }

    public void LoadSceneFade(SCENE_LIST requestScene)
    {
        LoadSceneFade(requestScene.GetEnumName());
    }

    // Load Scene Ascyn (Loading Scene)
    public void LoadSceneAsync(string sceneName)
    {
        Managers.UIManager.CommonSceneUI.FadeOut(Constants.TIME_UI_SCENE_DEFAULT_FADE, () => { LoadingScene.LoadScene(sceneName); });
    }
    public void LoadSceneAsync(SCENE_LIST requestScene)
    {
        LoadSceneAsync(requestScene.GetEnumName());
    }

    #region Property
    public BaseScene CurrentScene { get { return currentScene; } set { currentScene = value; } }
    #endregion
}
