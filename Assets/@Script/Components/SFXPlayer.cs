using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private AudioSource[] sfxPlayers;

    private void Awake()
    {
        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            gameObject.AddComponent<AudioSource>();
        }
        sfxPlayers = GetComponents<AudioSource>();
    }

    public void PlaySFX(string sfxName)
    {
        Managers.AudioManager.PlaySFX(sfxPlayers, sfxName);
    }

    public AudioSource[] SfxPlayers { get { return sfxPlayers; } }
}
