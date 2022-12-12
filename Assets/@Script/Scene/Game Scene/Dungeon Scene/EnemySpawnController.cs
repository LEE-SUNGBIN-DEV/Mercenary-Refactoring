using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpawnData
{
    public int spawnAmount;
    public ObjectPool enemy;
}

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private SpawnData[] spawnDatas;
    public UnityAction OnSpawn;

    private void SpawnMonster()
    {
        for (int i = 0; i < spawnDatas.Length; ++i)
        {
            for (int j = 0; j < spawnDatas[i].spawnAmount; ++j)
            {
                GameObject poolObject = Managers.SceneManagerCS.CurrentScene.RequestObject(spawnDatas[i].enemy.key);
                BaseEnemy enemy = poolObject.GetComponent<BaseEnemy>();
                Vector3 monsterPosition = transform.position + Random.insideUnitSphere * 5f;
                enemy.NavMeshAgent.Warp(monsterPosition);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SpawnMonster();
            OnSpawn();
            gameObject.SetActive(false);
        }
    }
}
