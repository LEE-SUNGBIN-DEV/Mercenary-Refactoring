using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenShockWave : MonoBehaviour
{
    private Material targetMaterial;

    [Tooltip("Determines the starting point of the shock wave.")]
    [SerializeField] private Vector2 centerPoint = new Vector2(0.5f, 0.5f);
    [Range(0, 1)]
    [SerializeField] private float waveSize = 0.1f;
    [Range(0, 2)]
    [SerializeField] private float waveSpeed = 1f;
    [Range(0.3f, 2.5f)][Tooltip("The shape of the shock wave is adjusted depending on the screen ratio.")]
    [SerializeField] private float screenRatio = 1.778f;
    [Range(0, 1)]
    [SerializeField] private float magnification = 0.1f;

    public void Initialize()
    {
        enabled = false;
        targetMaterial = Managers.ResourceManager.LoadResourceSync<Material>("MATERIAL_SCREEN_SHOCK_WAVE");
    }

    private void OnEnable()
    {
        Managers.PostProcessingManager.ScreenShockWave = this;
    }

    public void SetParameters()
    {
        screenRatio = (float)Screen.width / Screen.height;

        targetMaterial.SetVector("_CenterPoint", centerPoint);
        targetMaterial.SetFloat("_WaveSize", waveSize);
        targetMaterial.SetFloat("_WaveSpeed", waveSpeed);
        targetMaterial.SetFloat("_ScreenRatio", screenRatio);
        targetMaterial.SetFloat("_Magnification", magnification);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(targetMaterial != null)
        {
            SetParameters();
            Graphics.Blit(source, destination, targetMaterial, 0);
        }
    }
}
