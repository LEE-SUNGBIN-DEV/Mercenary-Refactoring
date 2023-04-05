using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private string spawnEnemyKey;

    public void SpawnEnemy()
    {
        GameObject poolObject = Managers.SceneManagerCS.CurrentScene.RequestObject(spawnEnemyKey);
        if (poolObject != null && poolObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.NavMeshAgent.Warp(transform.position);
        }
    }
}
