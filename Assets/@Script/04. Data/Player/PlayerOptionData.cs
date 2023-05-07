using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOptionData
{
    public event UnityAction<PlayerOptionData> OnPlayerOptionChanged;

    [Header("Audio Options")]
    [SerializeField] private float bgmVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private float ambientVolume;

    public void Initialize()
    {
        bgmVolume = 0.5f;
        sfxVolume = 0.5f;
        ambientVolume = 0.5f;
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
}
