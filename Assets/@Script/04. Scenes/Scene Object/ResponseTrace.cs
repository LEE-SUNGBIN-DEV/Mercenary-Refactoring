using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseTrace : MonoBehaviour, IInteractableObject
{
    [SerializeField] private ResponseTraceData responseTraceData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distance;

    public void Initialize(ResponseTraceData responseTraceData)
    {
        this.responseTraceData = responseTraceData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetCharacter == null && other.TryGetComponent(out PlayerCharacter character))
        {
            targetCharacter = character;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter character) && character == targetCharacter)
        {
            targetCharacter.InteractionController.ExitInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }

    private void Update()
    {
        if (targetCharacter != null)
        {
            distance = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.EnterDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateInteraction(this, targetCharacter);
        }
    }

    // Detection
    public void EnterDetection(PlayerCharacter character)
    {
        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.OpenPanel(Managers.DataManager.TextTable["TEXT_RESPONSE"].textContent);
    }
    public void UpdateDetection(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionEnterButton.WasPressedThisFrame())
        {
            character.InteractionController.EnterInteraction(this, character);
        }
    }
    public void ExitDetection(PlayerCharacter character)
    {
        Managers.UIManager.UIFixedPanelCanvas.InteractionPanel.ClosePanel();
    }

    // Interaction
    public void EnterInteraction(PlayerCharacter character)
    {
        Managers.UIManager.UIInteractionPanelCanvas.ResponseTracePanel.OpenPanel(responseTraceData.title, responseTraceData.content);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);
    }
    public void UpdateInteraction(PlayerCharacter character)
    {
        if (Managers.InputManager.InteractionExitButton.WasPressedThisFrame())
        {
            character.InteractionController.ExitInteraction(this, character);
        }
    }
    public void ExitInteraction(PlayerCharacter character)
    {
        Managers.UIManager.UIInteractionPanelCanvas.ResponseTracePanel.ClosePanel();
        Managers.InputManager.PopInputMode();
    }


    public float Distance { get { return distance; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
    public ResponseTraceData ResponseTraceData { get { return responseTraceData; } }
}
