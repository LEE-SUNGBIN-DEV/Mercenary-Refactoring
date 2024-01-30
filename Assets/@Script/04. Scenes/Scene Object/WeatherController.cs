using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private WEATHER_TYPE weatherType;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 offset;

    private void OnEnable()
    {
        Managers.AudioManager.PlayWeatherSound($"AUDIO_WEATHER_{weatherType.GetEnumName()}");
    }
    private void OnDisable()
    {
        if (Managers.AudioManager != null)
            Managers.AudioManager.StopWeatherSound();
    }

    private void LateUpdate()
    {
        if(targetTransform != null)
        {
            transform.position = targetTransform.position + offset;
        }
    }

    public void Initialize(WEATHER_TYPE weatherType, Transform targetTransform)
    {
        this.weatherType = weatherType;
        this.targetTransform = targetTransform;
    }

    public void ActiveWeather()
    {
        gameObject.SetActive(true);
    }
    public void InactiveWeather()
    {
        gameObject.SetActive(false);
    }
}
