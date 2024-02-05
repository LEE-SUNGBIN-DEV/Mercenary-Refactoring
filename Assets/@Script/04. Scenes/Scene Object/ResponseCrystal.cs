using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCrystal : MonoBehaviour, IInteractableObject, IWarpObject
{
    [SerializeField] private ResponseCrystalData responseCrystalData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distance;

    private Coroutine interactionCoroutine;

    [SerializeField] private ParticleSystem glowParticle;
    private Transform warpPointTransform;

    [Header("For Shader")]
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private MaterialController materialController;

    [SerializeField] private float minLightPower;
    [SerializeField] private float maxLightPower;
    [SerializeField] private float currentLightPower;
    [SerializeField] private float transitionSpeed;

    public void Initialize(ResponseCrystalData responseCrystalData)
    {
        warpPointTransform = Functions.FindChild<Transform>(gameObject, "Warp_Point", true);
        this.responseCrystalData = responseCrystalData;

        minLightPower = 1f;
        maxLightPower = 30f;
        currentLightPower = 1f;
        float responseAnimationTime = 1.8f;
        transitionSpeed = (maxLightPower - minLightPower) / responseAnimationTime;
        interactionCoroutine = null;

        TryGetComponent(out glowParticle);
        if(glowParticle != null)
            glowParticle.Stop();

        meshRenderer = GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderer != null)
        {
            materialController = new MaterialController();
            materialController.Initialize(meshRenderer);

            materialController.PropertyBlock.SetFloat("_FinalPower", currentLightPower);
            materialController.SetPropertyBlock();
        }
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
        character.LocationData.EnableResponsePoint(responseCrystalData.responseCrystalID, true);

        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStartInteraction());

        character.State.SetState(ACTION_STATE.PLAYER_RESPONSE_IN, STATE_SWITCH_BY.FORCED);
        character.InventoryData.RefillResponseWater();
        Managers.AudioManager.PlaySFX(Constants.Audio_Response_Object_Enable);
        Managers.InputManager.PushInputMode(CHARACTER_INPUT_MODE.INTERACTION);

        if (glowParticle != null)
            glowParticle.Play();
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
        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStopInteraction());
        character.State.SetState(ACTION_STATE.PLAYER_RESPONSE_OUT, STATE_SWITCH_BY.FORCED);
        Managers.AudioManager.PlaySFX(Constants.Audio_Response_Object_Disable);
        Managers.InputManager.PopInputMode();

        if (glowParticle != null)
            glowParticle.Stop();
    }

    public IEnumerator CoStartInteraction()
    {
        while (currentLightPower < maxLightPower)
        {
            currentLightPower += transitionSpeed * Time.deltaTime;
            materialController.PropertyBlock.SetFloat("_FinalPower", currentLightPower);
            materialController.SetPropertyBlock();
            yield return null;
        }

        currentLightPower = Mathf.Clamp(currentLightPower, minLightPower, maxLightPower);
        Managers.UIManager.UIInteractionPanelCanvas.ResponsePointPanel.OpenPanel();
    }

    public IEnumerator CoStopInteraction()
    {
        Managers.UIManager.UIInteractionPanelCanvas.ResponsePointPanel.ClosePanel();

        while (currentLightPower > minLightPower)
        {
            currentLightPower -= transitionSpeed * Time.deltaTime;
            materialController.PropertyBlock.SetFloat("_FinalPower", currentLightPower);
            materialController.SetPropertyBlock();
            yield return null;
        }

        currentLightPower = Mathf.Clamp(currentLightPower, minLightPower, maxLightPower);
    }

    public float Distance { get { return distance; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
    public Transform WarpPointTransform { get { return warpPointTransform; } }
    public ResponseCrystalData ResponseCrystalData { get { return responseCrystalData; } }
}
