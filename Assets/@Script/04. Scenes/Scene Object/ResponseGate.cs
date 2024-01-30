using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseGate : MonoBehaviour, IWarpObject
{
    [Header("For Function")]
    [SerializeField] private bool isActivated;
    [SerializeField] private ResponseGateData responseGateData;
    [SerializeField] private ParticleSystem gateParticles;
    [SerializeField] private Light gatePointLight;
    private SFXPlayer sfxPlayer;
    private Transform warpPointTransform;

    [SerializeField] private ResponseGateWarp gateWarp;

    public void Initialize(ResponseGateData responseGateData)
    {
        isActivated = false;
        this.responseGateData = responseGateData;

        gateParticles = Functions.FindChild<ParticleSystem>(gameObject, "Gate_Particle_System", true);
        gatePointLight = Functions.FindChild<Light>(gameObject, "Gate_Point_Light", true);
        warpPointTransform = Functions.FindChild<Transform>(gameObject, "Warp_Point", true);
        
        sfxPlayer = Functions.GetOrAddComponent<SFXPlayer>(gameObject);
        sfxPlayer.SetOptions(3, 10);

        gateWarp = GetComponentInChildren<ResponseGateWarp>(true);
        if (gateWarp != null)
        {
            gateWarp.Initialize(responseGateData);
            EnableGate(false);
        }

        if (responseGateData != null && responseGateData.conditionBossIDs.Length > 0)
        {
            UpdateResponseGateState(Managers.DataManager.CurrentCharacterData.SceneData);
        }
    }

    public void UpdateResponseGateState(CharacterSceneData sceneData)
    {
        if (sceneData != null && responseGateData != null)
        {
            for (int i = 0; i < responseGateData.conditionBossIDs.Length; ++i)
            {
                if (!sceneData.IsClearedBoss(responseGateData.conditionBossIDs[i]))
                {
                    isActivated = false;
                }
            }
            isActivated = true;
        }
    }

    public void EnableGate(bool isEnable)
    {
        switch(isEnable)
        {
            case true:
                sfxPlayer.PlaySFX(Constants.Audio_Response_Object_Enable);
                gateWarp.gameObject.SetActive(true);
                if (gateParticles != null)
                    gateParticles.Play();
                break;

            case false:
                sfxPlayer.PlaySFX(Constants.Audio_Response_Object_Disable);
                gateWarp.gameObject.SetActive(false);
                if (gateParticles != null)
                    gateParticles.Stop();
                break;
        }

        if (gatePointLight != null)
            gatePointLight.enabled = isEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character))
        {
            if(isActivated)
            {
                EnableGate(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character))
        {
            if(isActivated)
            {
                EnableGate(false);
            }
        }
    }

    public Transform WarpPointTransform { get { return warpPointTransform; } }
    public ResponseGateData ResponseGateData { get { return responseGateData; } }
}
