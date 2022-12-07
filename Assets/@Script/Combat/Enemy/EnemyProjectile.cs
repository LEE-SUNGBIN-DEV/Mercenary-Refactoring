using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyCombatController
{
    [Header("Enemy Projectile")]
    [SerializeField] private float speed;
    [SerializeField] private float returnTime;
    [SerializeField] private string[] effectKeys;
    [SerializeField] private Vector3[] effectRotationOffset;

    private void OnEnable()
    {
        StartCoroutine(AutoReturn(gameObject.name, returnTime));
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            OnVFX(other);
        }
    }

    public virtual void OnVFX(Collider other)
    {
        for(int i=0; i<effectKeys.Length; ++i)
        {
            GameObject effect = owner.ObjectPoolController.RequestObject(effectKeys[i]);
            effect.transform.position = other.bounds.ClosestPoint(transform.position);
            effect.transform.rotation = Quaternion.Euler(other.transform.rotation.eulerAngles + effectRotationOffset[i]);
        }
    }

    private IEnumerator AutoReturn(string key, float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        owner.ObjectPoolController.ReturnObject(key, gameObject);
    }
}
