using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyCombatController
{
    [Header("Enemy Projectile")]
    [SerializeField] private float speed;
    [SerializeField] private string[] hitVFXKeys;

    private void OnEnable()
    {
        if(combatCollider != null)
            combatCollider.enabled = true;
    }
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == 6)
        {
            OnHitVFX(other);
            Managers.SceneManagerCS.CurrentScene.ReturnObject(name, gameObject);
        }
    }
    public void SetProjectile(float speed, Vector3 direction)
    {
        this.speed = speed;
        transform.forward = direction;
    }
    public virtual void OnHitVFX(Collider other)
    {
        for(int i=0; i<hitVFXKeys.Length; ++i)
        {
            GameObject effect = Managers.SceneManagerCS.CurrentScene.RequestObject(hitVFXKeys[i]);
            effect.transform.position = other.bounds.ClosestPoint(transform.position);
            effect.transform.rotation = Quaternion.Euler(other.transform.rotation.eulerAngles);
        }
    }
}
