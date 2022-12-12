using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : AutoReturnObject
{
    [Header("Effect Object")]
    [SerializeField] private ParticleSystem[] particleSystems;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }

}
