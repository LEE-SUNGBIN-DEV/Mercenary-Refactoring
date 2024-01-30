using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOptionData
{
    public event UnityAction<PlayerOptionData> OnPlayerOptionChanged;

    [Header("Audio Options")]
    [JsonProperty][SerializeField] private float bgmVolume;
    [JsonProperty][SerializeField] private float sfxVolume;
    [JsonProperty][SerializeField] private float ambientVolume;

    [Header("Graphic Options")]
    [JsonProperty][SerializeField] private bool isFullScreen;
    [JsonProperty][SerializeField] private int screenWidth;
    [JsonProperty][SerializeField] private int screenHeight;
    [JsonProperty][SerializeField] private int screenRefreshRate;

    [JsonProperty][SerializeField] private float mouseSensitivity;

    public void CreateData()
    {
        bgmVolume = 0.5f;
        sfxVolume = 0.5f;
        ambientVolume = 0.5f;

        screenWidth = Constants.RESOLUTION_DEFAULT_WIDTH;
        screenHeight = Constants.RESOLUTION_DEFAULT_HEIGHT;
        screenRefreshRate = 60;
        isFullScreen = true;

        mouseSensitivity = 10f;
    }

    public void UpdateResolution(int width, int height, int refreshRate)
    {
        if (screenWidth == width && screenHeight == height && screenRefreshRate == refreshRate)
            return;

        screenWidth = width;
        screenHeight = height;
        screenRefreshRate = refreshRate;
        Screen.SetResolution(screenWidth, screenHeight, isFullScreen, screenRefreshRate);
    }

    public void UpdateFullScreenMode(bool isEnable)
    {
        if (isFullScreen == isEnable)
            return;

        isFullScreen = isEnable;
        Screen.SetResolution(screenWidth, screenHeight, isEnable, screenRefreshRate);
    }

    public float BgmVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
    public float AmbientVolume
    { 
        get { return ambientVolume; }
        set
        {
            ambientVolume = value;
            OnPlayerOptionChanged?.Invoke(this); 
        } 
    }

    public bool IsFullScreen
    {
        get { return isFullScreen; }
        set
        {
            isFullScreen = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
    public int ScreenWidth
    {
        get { return screenWidth; }
        set
        {
            screenWidth = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
    public int ScreenHeight
    {
        get { return screenHeight; }
        set
        {
            screenHeight = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
    public int ScreenRefreshRate
    {
        get { return screenRefreshRate; }
        set
        {
            screenRefreshRate = value;
            OnPlayerOptionChanged?.Invoke(this);
        }
    }
}
