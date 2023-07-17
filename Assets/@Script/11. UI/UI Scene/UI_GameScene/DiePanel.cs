using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiePanel : UIPanel
{
    private float waitDuration;
    private Coroutine dieCoroutine;

    public void Initialize()
    {
        waitDuration = 3f;
    }

    private void OnEnable()
    {
        if (dieCoroutine != null)
            StopCoroutine(dieCoroutine);

        dieCoroutine = StartCoroutine(CoDie());
    }

    public IEnumerator CoDie()
    {
        yield return new WaitForSeconds(waitDuration);
        FadeOut(0.4f, ReturnLastPoint);
    }

    public void ReturnLastPoint()
    {
        Managers.SceneManagerCS.LoadSceneAsync(Managers.DataManager.CurrentCharacterData.LocationData.LastScene);
    }
}
