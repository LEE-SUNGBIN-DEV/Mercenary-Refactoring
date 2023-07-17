using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceGate : MonoBehaviour, IInteractable
{
    [Header("For Function")]
    [SerializeField] private ResonanceGateData resonanceGateData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distanceFromTarget;

    private Coroutine interactionCoroutine;

    public void Initialize(ResonanceGateData resonanceGateData)
    {
        this.resonanceGateData = resonanceGateData;
    }

    #region Detection
    public void EnableDetection(PlayerCharacter character)
    {
    }

    public void UpdateDetection(PlayerCharacter character)
    {
    }

    public void DisableDetection(PlayerCharacter character)
    {
    }
    #endregion

    #region Interaction
    public void EnableInteraction(PlayerCharacter character)
    {

    }
    public void UpdateInteraction(PlayerCharacter character)
    {

    }
    public void DisableInteraction(PlayerCharacter character)
    {

    }
    #endregion

    public void EnableGate()
    {

    }

    public void DisableGate()
    {

    }

    public float DistanceFromTarget { get { return distanceFromTarget; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
}
