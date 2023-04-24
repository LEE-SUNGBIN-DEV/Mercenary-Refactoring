using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Material resultMaterial = Managers.ResourceManager.LoadResourceSync<Material>(originalMaterials[i].name.Replace(" (Instance)", "_") + targetMaterial.GetEnumName());
            renderers[i].material = resultMaterial;
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

    public Renderer[] Renderers { get { return renderers; } }
    public Material[] OriginalMaterials { get { return originalMaterials; } }
    public MaterialPropertyBlock PropertyBlock { get { return propertyBlock; } }
}
