using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonScene : BaseGameScene
{
    [SerializeField] private EnemySpawnController[] normalMonsterSpawnPoint;
    [SerializeField] private BossRoomController bossRoomController;
    private int spawnOrder;

    public override void Initialize()
    {
        base.Initialize();

        sceneType = SCENE_TYPE.DUNGEON;
        //Managers.UIManager.EntrancePanel.EntranceText.text = sceneName;
        //Managers.UIManager.EntrancePanel.gameObject.SetActive(true);

        spawnOrder = 0;
        if (normalMonsterSpawnPoint.Length > 0)
        {
            for (int i = 0; i < normalMonsterSpawnPoint.Length; ++i)
            {
                normalMonsterSpawnPoint[i].OnSpawn += ActiveNextSpawnPoint;
            }

            normalMonsterSpawnPoint[0].gameObject.SetActive(true);
        }

        else
        {
            bossRoomController.gameObject.SetActive(true);
        }
    }

    public void ActiveNextSpawnPoint()
    {
        ++spawnOrder;
        if (spawnOrder == normalMonsterSpawnPoint.Length-1)
        {
            bossRoomController.gameObject.SetActive(true);
        }

        else
        {
            normalMonsterSpawnPoint[spawnOrder].gameObject.SetActive(true);
        }
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}
