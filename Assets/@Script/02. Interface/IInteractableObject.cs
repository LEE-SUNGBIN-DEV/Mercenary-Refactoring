using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public PlayerCharacter TargetCharacter { get; }
    public float DistanceFromTarget { get; }

    public void EnableDetection(PlayerCharacter character);
    public void UpdateDetection(PlayerCharacter character);
    public void DisableDetection(PlayerCharacter character);

    public void EnableInteraction(PlayerCharacter character);
    public void UpdateInteraction(PlayerCharacter character);
    public void DisableInteraction(PlayerCharacter character);
}
