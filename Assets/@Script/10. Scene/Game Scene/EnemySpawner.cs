using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnData enemySpawnData;

    public void Initialize(EnemySpawnData enemySpawnData)
    {
        this.enemySpawnData = enemySpawnData;
        transform.position = enemySpawnData.GetPosition();
    }

    public void SpawnEnemy()
    {
        GameObject poolObject = Managers.SceneManagerCS.CurrentScene.RequestObject(enemySpawnData.enemyName);
        if (poolObject != null && poolObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.CharacterController.enabled = false;
            enemy.transform.position = transform.position;
            enemy.CharacterController.enabled = true;

            enemy.Spawn();
        }
    }
}
