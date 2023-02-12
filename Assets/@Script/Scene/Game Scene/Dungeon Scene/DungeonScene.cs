using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonScene : BaseGameScene
{
    public event UnityAction<float> OnUpdateBossHPBar;

    [SerializeField] private BaseEnemy boss;
    [SerializeField] private RoomGate bossRoomGate;
    [SerializeField] private EnemySpawnController bossSpawnPoint;
    [SerializeField] private EnemySpawnController[] enemySpawnPoint;
    [SerializeField] private WarpGate warpGate;

    public override void Initialize()
    {
        base.Initialize();

        sceneType = SCENE_TYPE.Dungeon;
        //Managers.UIManager.EntrancePanel.EntranceText.text = sceneName;
        //Managers.UIManager.EntrancePanel.gameObject.SetActive(true);

        for(int i=0; i<enemySpawnPoint.Length; ++i)
        {
            enemySpawnPoint[i].SpawnEnemy();
        }
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }


    public void UpdateBossHPBar(EnemyData enemyData)
    {
        float ratio = enemyData.CurrentHP / enemyData.MaxHP;
        OnUpdateBossHPBar?.Invoke(ratio);
    }

    public void StartBossBattle()
    {
        boss.EnemyData.OnChanageEnemyData -= UpdateBossHPBar;
        boss.EnemyData.OnChanageEnemyData += UpdateBossHPBar;

        boss.OnEnemyDie -= ClearBossBattle;
        boss.OnEnemyDie += ClearBossBattle;
        //Managers.UIManager.UIGameScene.MonsterPanel.SetBossHPBar(1f);

        bossRoomGate.CloseGate();
        warpGate.DisableGate();
    }

    public void ClearBossBattle(BaseEnemy enemy)
    {
        boss.EnemyData.OnChanageEnemyData -= UpdateBossHPBar;
        boss.OnEnemyDie -= ClearBossBattle;

        bossRoomGate.OpenGate();
        warpGate.EnableGate();

        StartCoroutine(CoDungeonClear());
    }

    IEnumerator CoDungeonClear()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1f;

        Managers.UIManager.RequestNotice("Å¬¸®¾î");
    }
}
