using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BossRoom : MonoBehaviour
{
    [Header("Boss Room")]
    protected GameScene gameScene;
    protected TriggerObject triggerObject;
    [SerializeField] protected EnemySpawner bossSpawner;
    [SerializeField] protected RoomGate[] bossRoomGates;
    [SerializeField] protected BaseEnemy currentBoss;
    [SerializeField] protected PlayableDirector playerableDirector;

    public void Initialize(GameScene gameScene)
    {
        this.gameScene = gameScene;

        triggerObject = GetComponentInChildren<TriggerObject>(true);
        triggerObject.Initialize();
        triggerObject.OnColliderEnter += StartEventScene;

        bossSpawner = GetComponentInChildren<EnemySpawner>(true);
        if (int.TryParse(bossSpawner.name.Replace("Prefab_Enemy_Spawner_", ""), out int spawnerID))
        {
            EnemySpawnerData enemySpawnData = Managers.DataManager.EnemySpawnerTable[spawnerID];
            bossSpawner.Initialize(gameScene, enemySpawnData);
        }
        else
            Debug.LogAssertion("ID Parse Error: " + bossSpawner.name);

        bossRoomGates = GetComponentsInChildren<RoomGate>(true);
        for (int i = 0; i < bossRoomGates.Length; ++i)
        {
            bossRoomGates[i].Initialize();
            bossRoomGates[i].OpenGate();
        }

        playerableDirector = GetComponentInChildren<PlayableDirector>(true);
    }

    public void StartEventScene(Collider other)
    {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            triggerObject.OnColliderEnter -= StartEventScene;
            StartBossBattle();

            if(playerableDirector != null)
                playerableDirector.Play();
        }
    }

    public void StartBossBattle()
    {
        currentBoss = bossSpawner.SpawnEnemy();
        currentBoss.OnEnemyDie += ClearBossBattle;
        gameScene.GameSceneUI.EnemyPanel.SetTargetEnemy(currentBoss);

        for (int i = 0; i < bossRoomGates.Length; ++i)
        {
            bossRoomGates[i].CloseGate();
        }

        Managers.UIManager.OpenPanel(gameScene.GameSceneUI.EnemyPanel);
    }

    public void ClearBossBattle(BaseEnemy enemy)
    {
        for (int i = 0; i < bossRoomGates.Length; ++i)
        {
            bossRoomGates[i].OpenGate();
        }

        gameScene.GameSceneUI.CenterNoticePanel.RequestNotice("관문 돌파");
        currentBoss.OnEnemyDie -= ClearBossBattle;
    }
}
