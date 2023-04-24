using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInteractionController
{
    [SerializeField] private bool isInteractable;
    [SerializeField] private IInteractable detectedInteraction;
    [SerializeField] private IInteractable enabledInteraction;

    public void Initialize()
    {
        isInteractable = true;
        detectedInteraction = null;
        enabledInteraction = null;
    }

    #region Detection
    public void EnableDetection(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (!isInteractable || detectedInteraction == requestedInteraction)
            return;

        if (detectedInteraction != null && detectedInteraction.DistanceFromTarget < requestedInteraction.DistanceFromTarget)
            return;

        detectedInteraction = requestedInteraction;
        detectedInteraction.EnableDetection(character);
    }

    public void UpdateDetection(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (detectedInteraction == requestedInteraction)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                EnableInteraction(detectedInteraction, character);
                return;
            }
        }
    }

    public void DisableDetection(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (detectedInteraction == requestedInteraction)
        {
            detectedInteraction.DisableDetection(character);
            detectedInteraction = null;
        }
    }
    #endregion

    #region Interaction
    public void EnableInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if(isInteractable && enabledInteraction != requestedInteraction)
        {
            enabledInteraction?.DisableInteraction(character);
            enabledInteraction = requestedInteraction;
            enabledInteraction?.EnableInteraction(character);
            isInteractable = false;
        }
    }
    public void UpdateInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (enabledInteraction == requestedInteraction)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableInteraction(requestedInteraction, character);
                return;
            }

            enabledInteraction.UpdateInteraction(character);
            return;
        }
    }

    public void DisableInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (enabledInteraction == requestedInteraction)
        {
            DisableInteraction(character);
            return;
        }
    }

    public void DisableInteraction(PlayerCharacter character)
    {
        if (enabledInteraction != null)
        {
            if(detectedInteraction != null)
                DisableDetection(detectedInteraction, character);

            enabledInteraction?.DisableInteraction(character);
            enabledInteraction = null;
            isInteractable = true;
            return;
        }
    }
    #endregion



    public bool IsInteractable { get { return isInteractable; } }
    public IInteractable EnabledInteraction { get { return enabledInteraction; } }
}
