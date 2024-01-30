using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResponseWater : MonoBehaviour
{
    private Material liquidMaterial;
    private string fillAmount = "_FillAmount";

    public void Initialize()
    {
        if (TryGetComponent(out Renderer responseWaterRenderer))
            liquidMaterial = responseWaterRenderer.sharedMaterials[2];
    }

    public void ShowResponseWater()
    {
        gameObject.SetActive(true);
    }
    public void HideResponseWater()
    {
        gameObject.SetActive(false);
    }

    public void SetFillRatio(float remainingRatio)
    {
        liquidMaterial.SetFloat(fillAmount, remainingRatio);
    }
}
