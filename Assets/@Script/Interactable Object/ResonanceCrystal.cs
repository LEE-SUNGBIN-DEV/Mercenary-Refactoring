using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceCrystal : MonoBehaviour, IInteractable
{
    [SerializeField] private ResonanceCrystalData resonanceCrystalData;
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distanceFromTarget;

    private Coroutine interactionCoroutine;

    [Header("For Shader")]
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private MaterialController materialController;

    [SerializeField] private float minLightPower;
    [SerializeField] private float maxLightPower;
    [SerializeField] private float currentLightPower;
    [SerializeField] private float transitionSpeed;

    public void Initialize(ResonanceCrystalData resonanceCrystalData)
    {
        this.resonanceCrystalData = resonanceCrystalData;

        minLightPower = 1f;
        maxLightPower = 30f;
        currentLightPower = 1f;
        float resonanceAnimationTime = 1.8f;
        transitionSpeed = (maxLightPower - minLightPower) / resonanceAnimationTime;
        interactionCoroutine = null;

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
            targetCharacter.InteractionController.InactiveDetection(this, targetCharacter);
            targetCharacter.InteractionController.InactiveInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }

    private void Update()
    {
        if (targetCharacter != null)
        {
            distanceFromTarget = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.ActiveDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateDetection(this, targetCharacter);
            targetCharacter.InteractionController.UpdateInteraction(this, targetCharacter);
        }
    }

    // Detection
    public void EnableDetection(PlayerCharacter character)
    {
        Managers.UIManager.OpenPanel(Managers.UIManager.GameSceneUI.InteractionPanel);
    }

    public void UpdateDetection(PlayerCharacter character)
    {
    }

    public void DisableDetection(PlayerCharacter character)
    {
        Managers.UIManager.ClosePanel(Managers.UIManager.GameSceneUI.InteractionPanel);
    }

    // Interaction
    public void EnableInteraction(PlayerCharacter character)
    {
        character.LocationData.EnableResonancePoint(resonanceCrystalData.id, true);

        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStartInteraction());

        character.State.SetState(ACTION_STATE.PLAYER_RESONANCE_IN, STATE_SWITCH_BY.FORCED);
        character.InventoryData.RefillResonanceWater();
    }

    public void UpdateInteraction(PlayerCharacter character)
    {
        
    }

    public void DisableInteraction(PlayerCharacter character)
    {
        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStopInteraction());
        character.State.SetState(ACTION_STATE.PLAYER_RESONANCE_OUT, STATE_SWITCH_BY.FORCED);
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
        Managers.UIManager.SwitchOrToggleUserPanel(Managers.UIManager.GameSceneUI.ResonancePointPanel);
    }

    public IEnumerator CoStopInteraction()
    {
        Managers.UIManager.SwitchOrToggleUserPanel(Managers.UIManager.GameSceneUI.ResonancePointPanel);

        while (currentLightPower > minLightPower)
        {
            currentLightPower -= transitionSpeed * Time.deltaTime;
            materialController.PropertyBlock.SetFloat("_FinalPower", currentLightPower);
            materialController.SetPropertyBlock();
            yield return null;
        }

        currentLightPower = Mathf.Clamp(currentLightPower, minLightPower, maxLightPower);
    }

    public float DistanceFromTarget { get { return distanceFromTarget; } }
    public PlayerCharacter TargetCharacter { get { return targetCharacter; } }
}
