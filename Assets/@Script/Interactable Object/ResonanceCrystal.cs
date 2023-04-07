using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceCrystal : MonoBehaviour, IInteractableObject
{
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>(true);
        if (meshRenderer != null && propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void Interact(PlayerCharacter character)
    {

    }

    public void EnableGate()
    {

    }

    public void DisableGate()
    {

    }

    public IEnumerator CoStartBlink()
    {
        float blinkAmount = 0f;
        float blinkSpeed = 0.3f;

        while (blinkAmount <= 30f)
        {
            blinkAmount += blinkSpeed * Time.deltaTime;
            blinkAmount = Mathf.Clamp(blinkAmount, 1f, 30f);
            propertyBlock.SetFloat("_DissolveAmount", blinkAmount);
            meshRenderer.SetPropertyBlock(propertyBlock);
            yield return null;
        }
    }
}
