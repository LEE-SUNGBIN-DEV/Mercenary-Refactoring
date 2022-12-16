using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();

    [SerializeField] protected HIT_STATE hitState;
    [SerializeField] protected int crowdControlState;
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

    public void AddCrowdControlState(CROWD_CONTROL_STATE crowdControlState)
    {
        this.crowdControlState |= (int)crowdControlState; 
    }
    public void SubCrowdControlState(CROWD_CONTROL_STATE crowdControlState)
    {
        this.crowdControlState &= ~(int)crowdControlState;
    }

    #region Property
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public HIT_STATE HitState { get { return hitState; } set { hitState = value; } }
    public int CrowdControlState { get { return crowdControlState; } set { crowdControlState = value; } }
    #endregion
}
