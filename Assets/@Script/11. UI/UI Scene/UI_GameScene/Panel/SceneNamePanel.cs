using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneNamePanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private float activeTime;

    public void Initialize()
    {
        mapNameText = GetComponentInChildren<TextMeshProUGUI>();
        activeTime = 3f;
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    IEnumerator AutoDisable()
    {
        float time = 0f;

        while(time <= activeTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    #region Property
    public TextMeshProUGUI MapNameText
    {
        get { return mapNameText; }
        private set { mapNameText = value; }
    }
    #endregion
}
