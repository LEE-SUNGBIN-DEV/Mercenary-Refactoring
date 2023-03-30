using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Animator animator;
    protected CharacterController characterController;
    protected StateController state; 
    protected FallController fallController;
    protected Dictionary<string, Material> materialDictionary;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;

    protected virtual void Awake()
    {
        TryGetComponent(out animator);
        TryGetComponent(out characterController);
        state = new StateController(animator);
        fallController = new FallController(transform, characterController, state);
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(true);
        objectPooler.Initialize(transform);

        if (materialContainers != null)
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

    #region Property
    public CharacterController CharacterController { get { return characterController; } }
    public StateController State { get { return state; } }
    public FallController FallController { get { return fallController; } }
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }
    #endregion
}
