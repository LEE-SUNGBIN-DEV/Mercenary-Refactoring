using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField] private BaseEnemy owner;

    private void Awake()
    {
        owner = GetComponentInParent<BaseEnemy>(true);
    }

    public BaseEnemy Owner { get { return owner; } }
}
