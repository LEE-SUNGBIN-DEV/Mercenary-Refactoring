using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerObject : MonoBehaviour
{
    public event UnityAction<Collider> OnColliderEnter;
    public event UnityAction<Collider> OnColliderStay;
    public event UnityAction<Collider> OnColliderExit;

    [SerializeField] private Collider triggerCollider;

    public void Initialize()
    {
        TryGetComponent(out triggerCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnColliderEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnColliderExit?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnColliderStay?.Invoke(other);
    }

    public Collider TriggerCollider { get { return triggerCollider; } }
}
