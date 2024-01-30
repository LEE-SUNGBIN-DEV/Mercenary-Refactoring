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

    private Dictionary<SKY_BOX_TYPE, Material> skyBoxDictionary = new Dictionary<SKY_BOX_TYPE, Material>();
    private Material currentSkyBox;
    private WeatherController activedWeather;
    private Light worldLight;

    public void Initialize()
    {

    }

    public void SetWeather(WEATHER_TYPE weatherType)
    {
        if (activedWeather != null)
            activedWeather.InactiveWeather();

        activedWeather = Managers.ResourceManager.InstantiatePrefabSync($"WEATHER_{weatherType.GetEnumName()}").GetComponent<WeatherController>();
        activedWeather.Initialize(weatherType, Managers.GameManager.ActivedCamera.transform);
        activedWeather.ActiveWeather();
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
