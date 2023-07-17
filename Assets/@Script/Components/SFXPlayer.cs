using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private List<AudioSource> sfxPlayerList;

    private void Awake()
    {
        sfxPlayerList = new List<AudioSource>();

        if(audioPlayerAmount == 0)
            audioPlayerAmount = Constants.SFX_PLAYER_DEFAULT_AMOUNT;

        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            SetAudioSource(audioSource);
            sfxPlayerList.Add(audioSource);
        }
    }

    public void PlaySFX(AudioClip targetClip)
    {
        for (int i = 0; i < sfxPlayerList.Count; ++i)
        {
            if (!sfxPlayerList[i].isPlaying)
            {
                sfxPlayerList[i].volume = Managers.AudioManager.SFXVolume;
                sfxPlayerList[i].clip = targetClip;
                sfxPlayerList[i].Play();
                return;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        SetAudioSource(newAudioSource);
        newAudioSource.clip = targetClip;
        newAudioSource.Play();

        sfxPlayerList.Add(newAudioSource);
    }
    public void PlaySFX(string sfxName)
    {
        if (string.IsNullOrEmpty(sfxName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(sfxName);
        PlaySFX(targetClip);
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        audioSource.playOnAwake = false;
        audioSource.volume = Managers.AudioManager.SFXVolume;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 4f;
        audioSource.maxDistance = 50f;
    }

    public List<AudioSource> SFXPlayerList { get { return sfxPlayerList; } }
}
