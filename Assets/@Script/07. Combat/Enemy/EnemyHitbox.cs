using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemy;
    [SerializeField] private Collider hitboxCollider;

    public void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
        TryGetComponent(out hitboxCollider);
    }

    public BaseEnemy Enemy { get { return enemy; } }
    public Collider HitboxCollider { get { return hitboxCollider; } }
}
