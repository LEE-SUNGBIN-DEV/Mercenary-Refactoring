using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerInteractionController
{
    [SerializeField] private IInteractableObject currentDetection;
    [SerializeField] private IInteractableObject currentInteraction;

    public void Initialize()
    {
        currentDetection = null;
        currentInteraction = null;
    }

    #region Detection
    public bool IsDetectable(IInteractableObject requestedDetection)
    {
        return IsInteractable(requestedDetection) && currentDetection != requestedDetection && !(currentDetection?.Distance < requestedDetection.Distance);
    }
    public void EnterDetection(IInteractableObject requestedDetection, PlayerCharacter character)
    {
        if (IsDetectable(requestedDetection))
        {
            currentDetection = requestedDetection;
            currentDetection.EnterDetection(character);
        }
    }
    public void UpdateDetection(IInteractableObject requestedDetection, PlayerCharacter character)
    {
        if (currentDetection == requestedDetection)
        {
            currentDetection.UpdateDetection(character);
        }
    }
    public void ExitDetection(IInteractableObject requestedDetection, PlayerCharacter character)
    {
        if (currentDetection == requestedDetection)
        {
            currentDetection?.ExitDetection(character);
            currentDetection = null;
        }
    }
    #endregion

    #region Interaction
    public bool IsInteractable(IInteractableObject requestedInteraction)
    {
        return currentInteraction == null;
    }
    public void EnterInteraction(IInteractableObject requestedInteraction, PlayerCharacter character)
    {
        if (IsInteractable(requestedInteraction))
        {
            ExitDetection(requestedInteraction, character);
            currentInteraction?.ExitInteraction(character);
            currentInteraction = requestedInteraction;
            currentInteraction?.EnterInteraction(character);
        }
    }
    public void UpdateInteraction(IInteractableObject requestedInteraction, PlayerCharacter character)
    {
        if (currentInteraction == requestedInteraction)
        {
            currentInteraction.UpdateInteraction(character);
        }
    }

    public void ExitInteraction(IInteractableObject requestedInteraction, PlayerCharacter character)
    {
        if(currentDetection == requestedInteraction)
        {
            ExitDetection(requestedInteraction, character);
        }

        if (currentInteraction == requestedInteraction)
        {
            ExitInteraction(character);
        }
    }

    public void ExitInteraction(PlayerCharacter character)
    {
        if (currentDetection != null)
        {
            currentDetection?.ExitDetection(character);
            currentDetection = null;
        }

        if (currentInteraction != null)
        {
            currentInteraction?.ExitInteraction(character);
            currentInteraction = null;
            return;
        }
    }
    #endregion
}
