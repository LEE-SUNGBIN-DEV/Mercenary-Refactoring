using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected BuffController buffController;
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;
    protected Dictionary<string, Material> materialDictionary;
    protected Animator animator;

    public virtual void Awake()
    {
        TryGetComponent(out animator);
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);
        if(materialContainers != null)
        {
            materialDictionary = new Dictionary<string, Material>();
            for (int i=0; i<materialContainers.Length; ++i)
            {
                materialDictionary.Add(materialContainers[i].key, materialContainers[i].value);
            }
        }
        objectPooler.Initialize(transform);
        buffController = new BuffController(this);
    }

    public void SetMaterial(string key)
    {
        if(materialDictionary.ContainsKey(key))
        {
            meshRenderer.material = materialDictionary[key];
        }
    }

    public void AddBuff(BUFF targetState, float duration)
    {
        buffController.AddBuff(targetState, duration);
    }
    public void SubBuff(BUFF targetState)
    {
        buffController.SubBuff(targetState);
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    public BuffController BuffController { get { return buffController; } }
    #endregion
}
