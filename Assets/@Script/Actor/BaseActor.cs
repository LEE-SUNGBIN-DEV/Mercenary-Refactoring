using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected ObjectPoolController objectPoolController = new ObjectPoolController();
    [SerializeField] protected MaterialContainer[] materialContainers;
    protected Dictionary<string, Material> materialDictionary;
    protected Animator animator;

    public virtual void Awake()
    {
        TryGetComponent(out animator);
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);
        objectPoolController.Initialize(gameObject);
        if(materialContainers != null)
        {
            materialDictionary = new Dictionary<string, Material>();
            for (int i=0; i<materialContainers.Length; ++i)
            {
                materialDictionary.Add(materialContainers[i].key, materialContainers[i].value);
            }
        }
    }

    public void SetMaterial(string key)
    {
        if(materialDictionary.ContainsKey(key))
        {
            meshRenderer.material = materialDictionary[key];
        }
    }

    public GameObject RequestObject(string key)
    {
        return objectPoolController?.RequestObject(key);
    }
    public void ReturnObject(string key, GameObject returnObject)
    {
        objectPoolController?.ReturnObject(key, returnObject);
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    #endregion
}
