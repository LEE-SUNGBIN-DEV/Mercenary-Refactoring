using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour, IInteractable
{
    public float DistanceFromTarget => throw new System.NotImplementedException();

    public PlayerCharacter TargetCharacter => throw new System.NotImplementedException();

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

    public void OpenGate()
    {

    }

    public void CloseGate()
    {

    }
}
