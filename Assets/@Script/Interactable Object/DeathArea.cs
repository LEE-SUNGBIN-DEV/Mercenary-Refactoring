using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    private Collider areaCollider;

    private void Awake()
    {
        TryGetComponent(out areaCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerCharacter player))
        {
            player.OnDie();
        }

        if (other.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.OnDie();
        }
    }
}
