#define EDITOR_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseGameScene : BaseScene
{
    public event UnityAction<float> OnUpdateBossHPBar;

    [SerializeField] protected UIGameScene gameSceneUI;
    [SerializeField] protected PlayerCharacter character;
    [SerializeField] protected List<NPC> npcList = new List<NPC>();
    [SerializeField] protected Vector3 playerSpawnPosition;

    [SerializeField] protected BaseEnemy boss;
    [SerializeField] protected RoomGate bossRoomGate;
    [SerializeField] protected EnemySpawnController bossSpawnPoint;
    [SerializeField] protected EnemySpawnController[] enemySpawnPoint;
    [SerializeField] protected ResonanceGate warpGate;

    public override void Initialize()
    {
        base.Initialize();
        sceneType = SCENE_TYPE.Game;

        // Creat Player Character
        if (Managers.DataManager.SelectCharacterData != null)
            character = Functions.CreateCharacterWithCamera(playerSpawnPosition);

        // Initialize NPC
        for(int i=0; i<npcList.Count; ++i)
        {
            Managers.NPCManager.NPCDictionary.Add(npcList[i].NpcID, npcList[i]);
            npcList[i].Initialize();
        }

#if EDITOR_TEST
        character.CharacterData.EquipmentSlotData.Initialize();
#endif

        // Initialize Game Scene UI
        gameSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Game_Scene).GetComponent<UIGameScene>();
        gameSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        gameSceneUI.transform.SetAsFirstSibling();
        gameSceneUI.Initialize(character.CharacterData);

        if (gameSceneUI.gameObject.activeSelf == false)
            gameSceneUI.gameObject.SetActive(true);

        RegisterObject(Constants.Prefab_Floating_Damage_Text, 16);
    }

    public override void ExitScene()
    {
        base.ExitScene();
        Managers.NPCManager.NPCDictionary.Clear();
        character = null;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < enemySpawnPoint.Length; ++i)
        {
            enemySpawnPoint[i].SpawnEnemy();
        }
    }

    public void UpdateBossHPBar(EnemyData enemyData)
    {
        float ratio = enemyData.CurrentHP / enemyData.MaxHP;
        OnUpdateBossHPBar?.Invoke(ratio);
    }

    public void StartBossBattle()
    {
        boss.Status.OnChanageEnemyData -= UpdateBossHPBar;
        boss.Status.OnChanageEnemyData += UpdateBossHPBar;

        boss.OnEnemyDie -= ClearBossBattle;
        boss.OnEnemyDie += ClearBossBattle;
        //Managers.UIManager.UIGameScene.MonsterPanel.SetBossHPBar(1f);

        bossRoomGate.CloseGate();
        warpGate.DisableGate();
    }

    public void ClearBossBattle(BaseEnemy enemy)
    {
        boss.Status.OnChanageEnemyData -= UpdateBossHPBar;
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

        Managers.UIManager.RequestNotice("Clear");
    }

    public Vector3 SpawnPosition { get { return playerSpawnPosition; } }
    public PlayerCharacter Character { get { return character; } }
}
