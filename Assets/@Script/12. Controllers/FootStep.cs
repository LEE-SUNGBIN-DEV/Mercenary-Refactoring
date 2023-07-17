using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    private SFXPlayer sfxPlayer;
    private string[] footStepSourceNames;

    private void Awake()
    {
        TryGetComponent(out sfxPlayer);
    }

    public void InitializeFootSteps(string[] sfxNames)
    {
        footStepSourceNames = sfxNames;
    }

    public void PlayFootStep()
    {
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit terrainHit, 1.1f, 1 << Constants.LAYER_TERRAIN))
        {
            int randomIndex = Random.Range(0, footStepSourceNames.Length);
            sfxPlayer.PlaySFX(footStepSourceNames[randomIndex]);
        }
    }
}
