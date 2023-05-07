using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private AudioSource[] audioPlayers;

    private void Awake()
    {
        if(audioPlayerAmount == 0)
            audioPlayerAmount = Constants.AUDIO_PLAYER_DEFAULT_AMOUNT;

        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            gameObject.AddComponent<AudioSource>();
        }
        audioPlayers = GetComponents<AudioSource>();
    }

    public void PlaySFX(string sfxName)
    {
        Managers.AudioManager.PlaySFX(audioPlayers, sfxName);
    }

    public AudioSource[] AudioPlayers { get { return audioPlayers; } }
}
