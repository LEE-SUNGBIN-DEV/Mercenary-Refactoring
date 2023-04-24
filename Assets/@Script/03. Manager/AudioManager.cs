using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager
{
    private AudioSource bgmPlayer;        
    private AudioSource[] sfxPlayers;
    private Slider bgmSlider;             
    private Slider sfxSlider;
    private Slider ambientSlider;

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
        for (int i = 0; i < Constants.NUMBER_SFX_PLAYER; ++i)
        {
            sfxPlayerObject.gameObject.AddComponent<AudioSource>();
        }
        sfxPlayers = sfxPlayerObject.GetComponents<AudioSource>();
    }

    public void SetBGMVolume()
    {
        bgmPlayer.volume = bgmSlider.value;
    }

    public void SetAmbientVolume()
    {

    }

    public void PlayBGM(string audioClipName)
    {
        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(audioClipName);

        bgmPlayer.Stop();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmSlider.value;
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
                audioPlayers[i].volume = sfxSlider.value;
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
                audioPlayers[i].volume = ambientSlider.value;
                audioPlayers[i].clip = targetClip;
                audioPlayers[i].Play();
                return;
            }
        }
        Debug.Log("Notice: 모든 오디오 플레이어가 동작중입니다.");
    }
}
