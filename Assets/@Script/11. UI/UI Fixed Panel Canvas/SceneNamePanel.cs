using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneNamePanel : UIPanel
{
    public enum TEXT
    {
        Scene_Name_Text
    }

    [SerializeField] private TextMeshProUGUI sceneNameText;

    [SerializeField] private Coroutine showCoroutine;
    [SerializeField] private float activeDuration;

    #region Private
    private void OnEnable()
    {
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        showCoroutine = StartCoroutine(AutoDisable(activeDuration));
    }
    private void OnDisable()
    {
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);
    }
    private IEnumerator AutoDisable(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }
    #endregion

    public void Initialize()
    {
        base.Awake();
        BindText(typeof(TEXT));

        sceneNameText = GetText((int)TEXT.Scene_Name_Text);
        activeDuration = 3f;
    }
    public void OpenPanel(string sceneName)
    {
        FadeInPanel();
        sceneNameText.text = sceneName;
    }
    public void ClosePanel()
    {
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => gameObject.SetActive(false));
    }

    public TextMeshProUGUI MapNameText { get { return sceneNameText; } }
}
