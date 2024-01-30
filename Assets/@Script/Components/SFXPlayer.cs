using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private List<AudioSource> sfxPlayerList;

    [Header("Audio Sources Option")]
    private float volume;
    private float minDistance;
    private float maxDistance;
    private float spatialBlend;
    private bool isOnAwake;

    private void Awake()
    {
        sfxPlayerList = new List<AudioSource>();

        if(audioPlayerAmount == 0)
            audioPlayerAmount = Constants.SFX_PLAYER_DEFAULT_AMOUNT;

        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            SetOptions(4f, 25f, 1f, false);
            sfxPlayerList.Add(audioSource);
        }
    }
    private void ApplyOptions()
    {
        for (int i = 0; i < sfxPlayerList.Count; ++i)
        {
            sfxPlayerList[i].playOnAwake = isOnAwake;
            sfxPlayerList[i].volume = volume;
            sfxPlayerList[i].spatialBlend = spatialBlend;
            sfxPlayerList[i].minDistance = minDistance;
            sfxPlayerList[i].maxDistance = maxDistance;
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
        sfxPlayerList.Add(newAudioSource);
        ApplyOptions();
        newAudioSource.clip = targetClip;
        newAudioSource.Play();
    }

    public void PlaySFX(string sfxName)
    {
        if (string.IsNullOrEmpty(sfxName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(sfxName);
        PlaySFX(targetClip);
    }

    public void SetOptions(float  minDistance, float maxDistance, float spatialBlend = 1f, bool isOnAwake = false)
    {
        volume = Managers.AudioManager.SFXVolume;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.spatialBlend = spatialBlend;
        this.isOnAwake = isOnAwake;
        ApplyOptions();
    }

    public List<AudioSource> SFXPlayerList { get { return sfxPlayerList; } }
}
