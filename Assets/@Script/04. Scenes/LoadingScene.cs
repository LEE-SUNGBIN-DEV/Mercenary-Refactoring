using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : BaseScene
{
    [SerializeField] private Image loadingBackgroundImage;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingBar;

    static private string nextSceneName; // 전환 요청이 들어온 씬

    private void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    public override void Initialize()
    {
        base.Initialize();

        sceneName = "Loading";
        sceneType = SCENE_TYPE.Loading;
        scene = SCENE_ID.Loading;

        loadingBackgroundImage = Functions.FindChild<Image>(gameObject, "Loading_Background_Image", true);
        loadingText = Functions.FindChild<TextMeshProUGUI>(gameObject, "Loading_Text", true);
        loadingBar = Functions.FindChild<Slider>(gameObject, "Loading_Bar", true);

        Managers.UIManager.SetCursorMode(CURSOR_MODE.INVISIBLE);
        Managers.InputManager.InitializeInputMode(CHARACTER_INPUT_MODE.UI);
    }

    private IEnumerator LoadSceneProgress()
    {
        AsyncOperation loadingProgress = SceneManager.LoadSceneAsync(nextSceneName);
        loadingProgress.allowSceneActivation = false;

        float timer = 0.0f;
        while (loadingProgress.isDone == false)
        {
            if (loadingProgress.progress < 0.9f)
            {
                loadingBar.value = loadingProgress.progress;
            }

            else
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1f, timer);

                if (loadingBar.value >= 1.0f)
                {
                    loadingProgress.allowSceneActivation = true;
                    yield break;
                }
            }

            yield return null;
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
