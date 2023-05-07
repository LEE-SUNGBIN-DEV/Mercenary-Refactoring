using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceCrystal : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private MaterialController materialController;

    [Header("For Function")]
    [SerializeField] private PlayerCharacter targetCharacter;
    [SerializeField] private float distanceFromTarget;
    [SerializeField] private ResonanceObjectData resonanceObjectData;

    [Header("For Shader")]
    [SerializeField] private float minLightPower;
    [SerializeField] private float maxLightPower;
    [SerializeField] private float currentLightPower;
    [SerializeField] private float transitionSpeed;

    private Coroutine interactionCoroutine;

    public void Initialize(ResonanceObjectData resonanceObjectData)
    {
        this.resonanceObjectData = resonanceObjectData;

        transform.position = resonanceObjectData.GetPosition();

        minLightPower = 1f;
        maxLightPower = 30f;
        currentLightPower = 1f;
        transitionSpeed = (maxLightPower - minLightPower) / 2.5f;
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
            targetCharacter.InteractionController.DisableDetection(this, targetCharacter);
            targetCharacter.InteractionController.DisableInteraction(this, targetCharacter);
            targetCharacter = null;
        }
    }

    private void Update()
    {
        if (targetCharacter != null)
        {
            distanceFromTarget = Vector3.SqrMagnitude(targetCharacter.transform.position - transform.position);

            targetCharacter.InteractionController.EnableDetection(this, targetCharacter);
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
        character.LocationData.EnableResonancePoint(resonanceObjectData.scene, resonanceObjectData.index);

        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStartInteraction());

        Managers.UIManager.ClosePanel(Managers.UIManager.GameSceneUI.InteractionPanel);
        character.State.SetState(ACTION_STATE.PLAYER_RESONANCE_IN, STATE_SWITCH_BY.FORCED);
    }

    public void UpdateInteraction(PlayerCharacter character)
    {
        
    }

    public void DisableInteraction(PlayerCharacter character)
    {
        if (interactionCoroutine != null)
            StopCoroutine(interactionCoroutine);

        interactionCoroutine = StartCoroutine(CoStopInteraction());

        Managers.UIManager.ClosePanel(Managers.UIManager.GameSceneUI.ResonancePointPanel);
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
        Managers.UIManager.OpenPanel(Managers.UIManager.GameSceneUI.ResonancePointPanel);
    }

    public IEnumerator CoStopInteraction()
    {
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
