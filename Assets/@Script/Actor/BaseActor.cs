using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();

    [SerializeField] protected AbnormalStateController abnormalStateController;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool isInvincible;
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
    }

    public void SetMaterial(string key)
    {
        if(materialDictionary.ContainsKey(key))
        {
            meshRenderer.material = materialDictionary[key];
        }
    }

    public void AddAbnormalState(ABNORMAL_TYPE targetState, float duration)
    {
        abnormalStateController.AddState(targetState, duration);
    }
    public void SubAbnormalState(ABNORMAL_TYPE targetState)
    {
        abnormalStateController.SubtractState(targetState);
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public AbnormalStateController AbnormalStateController { get { return abnormalStateController; } }
    #endregion
}
