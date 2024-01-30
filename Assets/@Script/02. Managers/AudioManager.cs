using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager
{
    private GameObject bgmPlayerObject;
    private GameObject sfxPlayerObject;
    private GameObject weatherPlayerObject;

    private AudioSource bgmPlayer;
    private AudioSource weatherPlayer;
    private List<AudioSource> sfxPlayerList;

    private float bgmVolume;
    private float sfxVolume;
    private float ambientVolume;

    public void Initialize(GameObject rootObject)
    {
        Managers.DataManager.PlayerData.OptionData.OnPlayerOptionChanged += SetVolume;

        // BGM Player
        bgmPlayerObject = Functions.FindObjectFromChild(rootObject, "@BGM_Player");

        if(bgmPlayerObject == null)
            bgmPlayerObject = new GameObject("@BGM_Player");

        bgmPlayerObject.transform.SetParent(rootObject.transform);
        bgmPlayer = Functions.GetOrAddComponent<AudioSource>(bgmPlayerObject);

        // Ambient Player
        weatherPlayerObject = Functions.FindObjectFromChild(rootObject, "@Weather_Player");

        if (weatherPlayerObject == null)
            weatherPlayerObject = new GameObject("@Weather_Player");

        weatherPlayerObject.transform.SetParent(rootObject.transform);
        weatherPlayer = Functions.GetOrAddComponent<AudioSource>(weatherPlayerObject);

        // SFX Player (UI)
        sfxPlayerList = new List<AudioSource>();
        sfxPlayerObject = Functions.FindObjectFromChild(rootObject, "@SFX_Player");

        if (sfxPlayerObject == null)
            sfxPlayerObject = new GameObject("@SFX_Player");

        sfxPlayerObject.transform.SetParent(rootObject.transform);

        for (int i = 0; i < Constants.SFX_PLAYER_DEFAULT_AMOUNT; ++i)
            sfxPlayerList.Add(sfxPlayerObject.AddComponent<AudioSource>());

        // Set Volume
        SetVolume(Managers.DataManager.PlayerData.OptionData);
    }

    public void SetVolume(PlayerOptionData optionData)
    {
        bgmVolume = optionData.BgmVolume;
        sfxVolume = optionData.SfxVolume;
        ambientVolume = optionData.AmbientVolume;
    }

    public void PlayBGM(string audioClipName)
    {
        if (string.IsNullOrEmpty(audioClipName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(audioClipName);

        bgmPlayer.Stop();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = targetClip;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlayWeatherSound(string audioClipName)
    {
        if (string.IsNullOrEmpty(audioClipName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(audioClipName);

        weatherPlayer.Stop();
        weatherPlayer.loop = true;
        weatherPlayer.volume = ambientVolume;
        weatherPlayer.clip = targetClip;
        weatherPlayer.Play();
    }

    public void StopWeatherSound()
    {
        weatherPlayer.Stop();
    }

    public void PlaySFX(string sfxClipName)
    {
        if (string.IsNullOrEmpty(sfxClipName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(sfxClipName);

        for (int i = 0; i < sfxPlayerList.Count; ++i)
        {
            if (!sfxPlayerList[i].isPlaying)
            {
                sfxPlayerList[i].volume = sfxVolume;
                sfxPlayerList[i].clip = targetClip;
                sfxPlayerList[i].Play();
                return;
            }
        }

        AudioSource newAudioSource = sfxPlayerObject.AddComponent<AudioSource>();
        newAudioSource.volume = Managers.AudioManager.SFXVolume;
        newAudioSource.clip = targetClip;
        newAudioSource.Play();

        sfxPlayerList.Add(newAudioSource);
    }

    public float BGMVolume { get { return bgmVolume; } }
    public float SFXVolume { get { return sfxVolume; } }
    public float AmbientVolume { get { return ambientVolume; } }
}
