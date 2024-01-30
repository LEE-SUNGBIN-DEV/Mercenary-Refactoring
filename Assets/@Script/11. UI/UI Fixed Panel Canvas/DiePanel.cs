using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePanel : UIPanel
{
    [SerializeField] private float activeDuration;
    private Coroutine dieActiveCoroutine;

    #region Private
    private void OnEnable()
    {
        if (dieActiveCoroutine != null)
            StopCoroutine(dieActiveCoroutine);

        dieActiveCoroutine = StartCoroutine(CoShowDiePanel());
    }
    private void OnDisable()
    {
        if (dieActiveCoroutine != null)
            StopCoroutine(dieActiveCoroutine);
    }
    private IEnumerator CoShowDiePanel()
    {
        yield return new WaitForSeconds(activeDuration);
        ClosePanel();
    }
    private void ReturnLastPoint()
    {
        Managers.SceneManagerEX.LoadSceneAsync(Managers.DataManager.CurrentCharacterData.LocationData.LastScene);
    }
    #endregion

    public void Initialize()
    {
        activeDuration = 3f;
    }
    public void OpenPanel()
    {
        FadeInPanel();
    }
    public void ClosePanel()
    {
        FadeOutPanel(Constants.TIME_UI_PANEL_DEFAULT_FADE, () => { gameObject.SetActive(false); ReturnLastPoint(); });
    }
}
