using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] private int audioPlayerAmount;
    [SerializeField] private AudioSource[] ambientPlayers;

    private void Awake()
    {
        for (int i = 0; i < audioPlayerAmount; ++i)
        {
            gameObject.AddComponent<AudioSource>();
        }
        ambientPlayers = GetComponents<AudioSource>();
    }

    public void PlayAmbientSound(string audioClipName)
    {
        Managers.AudioManager.PlayAmbientSound(ambientPlayers, audioClipName);
    }

    public AudioSource[] SfxPlayers { get { return ambientPlayers; } }
}
