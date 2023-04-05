using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public enum SKY_BOX_TYPE
    {
        Sunny,
        Rainy,
        Snowing,
    }

    public enum WEATHER_TYPE
    {
        Fireflies,
        StormClouds,
        FogGround,
        Rain_Light,
        Snow,
    }

    private Dictionary<SKY_BOX_TYPE, Material> skyBoxDictionary = new Dictionary<SKY_BOX_TYPE, Material>();
    private Material currentSkyBox;
    private GameObject currentWeather;
    private Light worldLight;

    public void Initialize()
    {

    }

    public void SetWeather(WEATHER_TYPE weatherType)
    {
        if (currentWeather != null)
            currentWeather.SetActive(false);

        if (Managers.GameManager.PlayerCamera != null)
        {
            currentWeather = Managers.ResourceManager.InstantiatePrefabSync("VFX_Weather_" + weatherType.ToString());
            currentWeather.transform.SetParent(Managers.GameManager.PlayerCamera.transform, false);
            currentWeather.SetActive(true);
        }
    }

    public IEnumerator CoChangeSkybox(Material targetSkyBox)
    {
        float blendFactor = 0f;
        float blendSpeed = 0.1f;

        while(blendFactor <= 1f)
        {
            blendFactor = Mathf.Lerp(blendFactor, 1f, blendSpeed * Time.deltaTime);
            RenderSettings.skybox.Lerp(currentSkyBox, targetSkyBox, blendFactor);
            yield return null;
        }

        currentSkyBox = targetSkyBox;
    }

}
