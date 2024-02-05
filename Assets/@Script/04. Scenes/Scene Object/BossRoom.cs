using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BossRoom : MonoBehaviour
{
    private PlayerCharacter roomPlayerCharacter;

    [Header("Boss Room")]
    private Transform respawnTransform;
    private TriggerObject triggerObject;
    [SerializeField] private EnemySpawner bossSpawner;
    [SerializeField] private RoomGate[] bossRoomGates;
    [SerializeField] private BaseEnemy currentBoss;
    [SerializeField] private PlayableDirector playerableDirector;

    public void Initialize(GameScene gameScene)
    {
        roomPlayerCharacter = gameScene.ScenePlayerCharacter;

        respawnTransform = Functions.FindChild<Transform>(gameObject, "Respawn_Point", true);
        triggerObject = GetComponentInChildren<TriggerObject>(true);
        triggerObject.Initialize();
        triggerObject.OnColliderEnter += StartEventScene;

        bossSpawner = GetComponentInChildren<EnemySpawner>(true);
        if (Managers.DataManager.EnemySpawnerTable.TryGetValue(bossSpawner.name, out EnemySpawnerData enemySpawnData))
        {
            bossSpawner.Initialize(gameScene, enemySpawnData);
        }
        else
        {
            Debug.LogAssertion("ID Parse Error: " + bossSpawner.name);
        }

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
            roomPlayerCharacter = playerCharacter;
            roomPlayerCharacter.CharacterData.LocationData.SetLastBossRoom(gameObject.name);
            StartBossBattle();

            if(playerableDirector != null)
                playerableDirector.Play();
        }
    }

    public void StartBossBattle()
    {
        currentBoss = bossSpawner.SpawnEnemy();
        currentBoss.OnEnemyDie -= ClearBoss;
        currentBoss.OnEnemyDie += ClearBoss;

        for (int i = 0; i < bossRoomGates.Length; ++i)
        {
            bossRoomGates[i].CloseGate();
        }

        Managers.UIManager.UIFixedPanelCanvas.EnemyPanel.OpenPanel(currentBoss);
    }

    public void ClearBoss(BaseEnemy enemy)
    {
        currentBoss.OnEnemyDie -= ClearBoss;
        for (int i = 0; i < bossRoomGates.Length; ++i)
        {
            bossRoomGates[i].OpenGate();
        }

        if (!roomPlayerCharacter.CharacterData.SceneData.IsClearedBoss(currentBoss.Status.EnemyID))
        {
            roomPlayerCharacter.CharacterData.SceneData.ModifyBossClearInformation(currentBoss.Status.EnemyID, true);
            StartCoroutine(CoClearBoss());
        }
    }

    public IEnumerator CoClearBoss()
    {
        yield return new WaitForSeconds(5f);
        Managers.UIManager.UIFixedPanelCanvas.CenterNoticePanel.OpenPanel("관문 돌파");
    }

    #region Property
    public Transform RespawnTransform { get { return respawnTransform; } }
    #endregion
}
