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
        SceneManager.sceneLoaded += SceneEnter;
        SceneManager.sceneUnloaded += SceneExit;
    }

    public void SceneEnter(Scene scene, LoadSceneMode loadMode)
    {
        Managers.UIManager.CommonSceneUI.FadeIn(2f);
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
    public void LoadScene(string sceneName)
    {
        Managers.UIManager.CommonSceneUI.FadeOut(2f, () => { SceneManager.LoadScene(sceneName); });
    }

    public void LoadScene(SCENE_LIST requestScene)
    {
        LoadScene(requestScene.GetEnumName());
    }

    // Load Scene Async 
    public void LoadSceneAsync(string sceneName)
    {
        Managers.UIManager.CommonSceneUI.FadeOut(2f, () =>
        {
            LoadingScene.LoadScene(sceneName);
        });
    }
    public void LoadSceneAsync(SCENE_LIST requestScene)
    {
        LoadSceneAsync(requestScene.GetEnumName());
    }

    #region Property
    public BaseScene CurrentScene { get { return currentScene; } set { currentScene = value; } }
    #endregion
}
