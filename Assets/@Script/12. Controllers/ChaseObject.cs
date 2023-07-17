using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseObject : MonoBehaviour, IPoolObject
{
    private CharacterController characterController;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float chaseSpeed;

    public void Initialize(Vector3 spawnPosition, Transform targetTransform, float chaseSpeed)
    {
        TryGetComponent(out characterController);
        transform.position = spawnPosition;
        this.targetTransform = targetTransform;
        this.chaseSpeed = chaseSpeed;
    }

    private void Update()
    {
        if (targetTransform == null)
            return;

        Vector3 targetDirection = Functions.GetZeroYDirection(transform.position, targetTransform.position);
        targetDirection.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 6f * Time.deltaTime);
        characterController.SimpleMove(targetDirection * chaseSpeed);
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        ObjectPooler = owner;
    }

    public void ActionBeforeReturn()
    {
    }

    public void ReturnOrDestoryObject()
    {
        if(ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get; set; }
    #endregion
}
