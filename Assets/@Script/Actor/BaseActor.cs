using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    [Header("Base Actor")]
    protected Animator animator;
    protected CharacterController characterController;
    protected Dictionary<string, Material> materialDictionary;
    [SerializeField] protected SkinnedMeshRenderer meshRenderer;
    [SerializeField] protected MaterialContainer[] materialContainers;
    [SerializeField] protected ObjectPooler objectPooler = new ObjectPooler();
    
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool isGround;

    public virtual void Awake()
    {
        TryGetComponent(out animator);
        TryGetComponent(out characterController);
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
    public Animator Animator { get { return animator; } }
    public SkinnedMeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    public bool IsGround
    {
        get
        {
            if (characterController.isGrounded)
                isGround = true;

            else
            {
                var ray = new Ray(transform.position, Vector3.down);
                var rayDistance = 0.5f;
                isGround = Physics.Raycast(ray, rayDistance, LayerMask.GetMask("Terrain"));
            }
            return isGround;
        }
    }
    #endregion
}
