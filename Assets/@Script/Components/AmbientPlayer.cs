using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private List<AudioSource> ambientPlayerList;

    private void Awake()
    {
        ambientPlayerList = new List<AudioSource>();

        if (audioPlayerAmount == 0)
            audioPlayerAmount = Constants.AMBIENT_PLAYER_DEFAULT_AMOUNT;

        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            SetAudioSource(audioSource);
            ambientPlayerList.Add(audioSource);
        }
    }


    public void PlayAmbientSound(string sfxName, float minDistance = 3f, float maxDistance = 50f, bool isLoop = true)
    {
        if (string.IsNullOrEmpty(sfxName))
            return;

        AudioClip targetClip = Managers.ResourceManager.LoadResourceSync<AudioClip>(sfxName);

        for (int i = 0; i < ambientPlayerList.Count; ++i)
        {
            if (!ambientPlayerList[i].isPlaying)
            {
                SetAudioSource(ambientPlayerList[i], minDistance, maxDistance, isLoop);
                ambientPlayerList[i].clip = targetClip;
                ambientPlayerList[i].Play();
                return;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        SetAudioSource(newAudioSource, minDistance, maxDistance, isLoop);
        newAudioSource.clip = targetClip;
        newAudioSource.Play();

        ambientPlayerList.Add(newAudioSource);
    }

    public void SetAudioSource(AudioSource audioSource, float minDistance = 3f, float maxDistance = 50f, bool isLoop = true)
    {
        audioSource.volume = Managers.AudioManager.AmbientVolume;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.loop = isLoop;
    }

    public List<AudioSource> AmbientPlayerList { get { return ambientPlayerList; } }
}
