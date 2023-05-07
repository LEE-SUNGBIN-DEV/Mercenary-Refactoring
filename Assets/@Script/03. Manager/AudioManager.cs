using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager
{
    private AudioSource bgmPlayer;        
    private AudioSource[] sfxPlayers;
    private float bgmVolume;
    private float sfxVolume;
    private float ambientVolume;

    public void Initialize(Transform rootTransform)
    {
        // BGM Player
        GameObject bgmPlayerObject = GameObject.Find("BGM_Player");
        if(bgmPlayerObject == null)
        {
            bgmPlayerObject = new GameObject("BGM_Player");
        }
        bgmPlayerObject.transform.SetParent(rootTransform);
        bgmPlayer = Functions.GetOrAddComponent<AudioSource>(bgmPlayerObject);

        // SFX Player
        GameObject sfxPlayerObject = GameObject.Find("SFX_Player");
        if (sfxPlayerObject == null)
        {
            sfxPlayerObject = new GameObject("SFX_Player");
        }
        sfxPlayerObject.transform.SetParent(rootTransform);
        for (int i = 0; i < Constants.AUDIO_PLAYER_DEFAULT_AMOUNT; ++i)
        {
            sfxPlayerObject.gameObject.AddComponent<AudioSource>();
        }
        sfxPlayers = sfxPlayerObject.GetComponents<AudioSource>();

        Managers.DataManager.PlayerData.OptionData.OnPlayerOptionChanged += SetVolume;
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

    public void PlaySFX(AudioSource[] audioPlayers, string audioClipName)
    {
        if (string.IsNullOrEmpty(audioClipName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(audioClipName);

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            if (!audioPlayers[i].isPlaying)
            {
                audioPlayers[i].volume = sfxVolume;
                audioPlayers[i].clip = targetClip;
                audioPlayers[i].Play();
                return;
            }
        }
        Debug.Log("Notice: 모든 오디오 플레이어가 동작중입니다.");
    }

    public void PlaySFX(string sfxClip)
    {
        PlaySFX(sfxPlayers, sfxClip);
    }

    public void PlayAmbientSound(AudioSource[] audioPlayers, string audioClipName)
    {
        if (string.IsNullOrEmpty(audioClipName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(audioClipName);

        for (int i = 0; i < audioPlayers.Length; ++i)
        {
            if (!audioPlayers[i].isPlaying)
            {
                audioPlayers[i].volume = ambientVolume;
                audioPlayers[i].clip = targetClip;
                audioPlayers[i].Play();
                return;
            }
        }
        Debug.Log("Notice: 모든 오디오 플레이어가 동작중입니다.");
    }
}
