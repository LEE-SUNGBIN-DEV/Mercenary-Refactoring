using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameScene ownerScene;
    [SerializeField] private EnemySpawnerData enemySpawnData;
    [SerializeField] private EnemyData enemyData;

    public void Initialize(GameScene ownerScene, EnemySpawnerData enemySpawnData)
    {
        this.ownerScene = ownerScene;
        this.enemySpawnData = enemySpawnData;
        this.enemyData = Managers.DataManager.EnemyTable[enemySpawnData.enemyID];
    }

    public BaseEnemy SpawnEnemy()
    {
        GameObject poolObject = ownerScene.RequestObject(enemyData.enemyID);
        if (poolObject != null && poolObject.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.Spawn(transform.position);
            return enemy;
        }
        return null;
    }

    public EnemySpawnerData EnemySpawnData { get { return enemySpawnData; } }
    public EnemyData EnemyData { get { return enemyData; } }
}
