using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemy;

    public void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public BaseEnemy Enemy { get { return enemy; } }
}
