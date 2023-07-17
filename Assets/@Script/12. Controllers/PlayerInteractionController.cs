using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerInteractionController
{
    public event UnityAction<bool> OnActiveInteraction;

    [SerializeField] private bool isInteractable;
    [SerializeField] private IInteractable detectedInteraction;
    [SerializeField] private IInteractable activedInteraction;

    public void Initialize()
    {
        isInteractable = true;
        detectedInteraction = null;
        activedInteraction = null;
    }

    #region Detection
    public void ActiveDetection(IInteractable requestedInteraction, PlayerCharacter character)
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
            if (character.GetInput().InteractionDown)
            {
                ActiveInteraction(detectedInteraction, character);
                return;
            }
        }
    }

    public void InactiveDetection(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (detectedInteraction == requestedInteraction)
        {
            detectedInteraction.DisableDetection(character);
            detectedInteraction = null;
        }
    }
    #endregion

    #region Interaction
    public void ActiveInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if(isInteractable && activedInteraction != requestedInteraction)
        {
            Managers.UIManager.ClosePanel(Managers.UIManager.GameSceneUI.InteractionPanel);
            OnActiveInteraction?.Invoke(true);
            activedInteraction?.DisableInteraction(character);
            activedInteraction = requestedInteraction;
            activedInteraction?.EnableInteraction(character);
            isInteractable = false;
        }
    }
    public void UpdateInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (activedInteraction == requestedInteraction)
        {
            if (character.GetInput().EscDown)
            {
                InactiveInteraction(requestedInteraction, character);
                return;
            }

            activedInteraction.UpdateInteraction(character);
            return;
        }
    }

    public void InactiveInteraction(IInteractable requestedInteraction, PlayerCharacter character)
    {
        if (activedInteraction == requestedInteraction)
        {
            InactiveInteraction(character);
            return;
        }
    }

    public void InactiveInteraction(PlayerCharacter character)
    {
        if (activedInteraction != null)
        {
            if(detectedInteraction != null)
                InactiveDetection(detectedInteraction, character);

            OnActiveInteraction?.Invoke(false);
            activedInteraction?.DisableInteraction(character);
            activedInteraction = null;
            isInteractable = true;
            return;
        }
    }
    #endregion


    public bool IsInteractable { get { return isInteractable; } }
    public IInteractable ActivedInteraction { get { return activedInteraction; } }
}
