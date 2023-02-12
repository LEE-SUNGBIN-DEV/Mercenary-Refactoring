using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public enum DAY_TIME
    {
        Morning,
        Evening,
        Night
    }
    public enum WEATHER_TYPE
    {
        Sunny,
        Rainy,
        Snowing,
    }
    private Dictionary<WEATHER_TYPE, Material> skyBoxDictionary = new Dictionary<WEATHER_TYPE, Material>();
    private Material currentSkyBox;
    private Light worldLight;

    public void Initialize()
    {

    }

    void Update()
    {

    }

    public IEnumerator CoChangeWeather(Material targetSkyBox)
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
