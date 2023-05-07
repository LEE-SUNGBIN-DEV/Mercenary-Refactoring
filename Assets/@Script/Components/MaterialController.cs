using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MaterialController
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material[] originalMaterials;
    [SerializeField] private MaterialPropertyBlock propertyBlock;

    public void Initialize(Renderer[] renderers)
    {
        this.renderers = renderers;
        if (renderers != null && propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; ++i)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    public void ChangeMaterials(MATERIAL_TYPE targetMaterial)
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            if(Managers.ResourceManager.CheckResource<Material>(originalMaterials[i].name.Replace(" (Instance)", "_") + targetMaterial.GetEnumName()))
            {
                Material resultMaterial = Managers.ResourceManager.LoadResourceSync<Material>(originalMaterials[i].name.Replace(" (Instance)", "_") + targetMaterial.GetEnumName());
                renderers[i].material = resultMaterial;
            }
        }
    }

    public void SetOriginalMaterials()
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].material = originalMaterials[i];
        }
    }

    public void SetPropertyBlock()
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].SetPropertyBlock(propertyBlock);
        }
    }

    public IEnumerator CoDissolve(float startAmount, float targetAmount, float duration = 0.5f, UnityAction callback = null)
    {
        float elapsedTime = 0f;
        float dissolveAmount = startAmount;

        ChangeMaterials(MATERIAL_TYPE.Dissolve);

        propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_AMOUNT, 0f);
        propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_GLOW_SIZE, 0.1f);
        SetPropertyBlock();

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            dissolveAmount = Mathf.Lerp(startAmount, targetAmount, elapsedTime / duration);

            propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_AMOUNT, dissolveAmount);
            SetPropertyBlock();
            yield return null;
        }

        propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_AMOUNT, 1f);
        propertyBlock.SetFloat(Constants.SHADER_PROPERTY_HASH_DISSOLVE_GLOW_SIZE, 0f);
        SetPropertyBlock();

        callback?.Invoke();
    }

    public Renderer[] Renderers { get { return renderers; } }
    public Material[] OriginalMaterials { get { return originalMaterials; } }
    public MaterialPropertyBlock PropertyBlock { get { return propertyBlock; } }
}
