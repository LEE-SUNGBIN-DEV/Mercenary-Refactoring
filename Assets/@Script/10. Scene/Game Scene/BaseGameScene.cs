using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BaseGameScene : BaseScene
{
    public event UnityAction<float> OnUpdateBossHPBar;

    protected GameSceneData gameSceneData;

    [SerializeField] protected PlayerCharacter character;
    [SerializeField] protected UIGameScene gameSceneUI;

    [SerializeField] protected List<NPC> npcList = new List<NPC>();

    [SerializeField] protected ResonanceGate resonanceGate;
    [SerializeField] protected ResonanceCrystal[] resonanceCrystals;

    [SerializeField] protected EnemySpawner[] normalEnemySpawners;
    [SerializeField] protected EnemySpawner[] eliteEnemySpawners;
    [SerializeField] protected EnemySpawner[] bossEnemySpawners;

    [SerializeField] protected BaseEnemy currentBoss;
    [SerializeField] protected RoomGate bossRoomGate;


    public override void Initialize()
    {
        base.Initialize();
        gameSceneData = Managers.DataManager.GameSceneTable[(SCENE_LIST)SceneManager.GetActiveScene().buildIndex];
        scene = gameSceneData.scene;
        sceneType = gameSceneData.sceneType;
        sceneName = gameSceneData.sceneName;

        // Creat Player Character
        if (Managers.DataManager.CurrentCharacterData != null)
        {
            character = Functions.CreateCharacterWithCamera(Managers.DataManager.CurrentCharacterData.LocationData.GetLastPosition());
            character.CharacterData.LocationData.LastScene = scene;
        }

        // Initialize Game Scene UI
        gameSceneUI = Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_UI_GAME_SCENE).GetComponent<UIGameScene>();
        gameSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
        gameSceneUI.transform.SetAsFirstSibling();
        gameSceneUI.Initialize(character.CharacterData);

        if (gameSceneUI.gameObject.activeSelf == false)
            gameSceneUI.gameObject.SetActive(true);

        // Initialize NPC
        for (int i=0; i<npcList.Count; ++i)
        {
            Managers.NPCManager.NPCDictionary.Add(npcList[i].NpcID, npcList[i]);
            npcList[i].Initialize();
        }

        // Initialize Resonance Crystals
        resonanceCrystals = new ResonanceCrystal[gameSceneData.resonanceObjectDataList.Count];
        for (int i = 0; i < resonanceCrystals.Length; ++i)
        {
            Managers.ResourceManager.InstantiatePrefabSync("Prefab_" + gameSceneData.resonanceObjectDataList[i].objectName).TryGetComponent(out resonanceCrystals[i]);
            resonanceCrystals[i].Initialize(gameSceneData.resonanceObjectDataList[i], gameSceneUI);
        }

        // Initialize Enemy Spawner
        normalEnemySpawners = new EnemySpawner[gameSceneData.normalSpawnDataList.Count];
        for (int i = 0; i < normalEnemySpawners.Length; ++i)
        {
            if(Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_ENEMY_SPAWNER).TryGetComponent(out normalEnemySpawners[i]))
            {
                normalEnemySpawners[i].Initialize(gameSceneData.normalSpawnDataList[i]);
                normalEnemySpawners[i].SpawnEnemy();
            }
        }

        eliteEnemySpawners = new EnemySpawner[gameSceneData.bossSpawnDataList.Count];
        for (int i = 0; i < eliteEnemySpawners.Length; ++i)
        {
            if(Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_ENEMY_SPAWNER).TryGetComponent(out eliteEnemySpawners[i]))
            {
                eliteEnemySpawners[i].Initialize(gameSceneData.normalSpawnDataList[i]);
                eliteEnemySpawners[i].SpawnEnemy();
            }
        }

        bossEnemySpawners = new EnemySpawner[gameSceneData.bossSpawnDataList.Count];
        for (int i = 0; i < bossEnemySpawners.Length; ++i)
        {
            if(Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_ENEMY_SPAWNER).TryGetComponent(out bossEnemySpawners[i]))
            {
                bossEnemySpawners[i].Initialize(gameSceneData.normalSpawnDataList[i]);
            }
        }

        RegisterObject(Constants.PREFAB_FLOATING_DAMAGE_TEXT, 16);
    }

    public void Start()
    {
        Managers.EnvironmentManager.SetWeather(gameSceneData.weatherType);
    }

    public override void ExitScene()
    {
        base.ExitScene();
        Managers.NPCManager.NPCDictionary.Clear();
        character = null;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < normalEnemySpawners.Length; ++i)
        {
            normalEnemySpawners[i].SpawnEnemy();
        }
    }

    public void UpdateBossHPBar(EnemyData enemyData)
    {
        float ratio = enemyData.CurrentHP / enemyData.MaxHP;
        OnUpdateBossHPBar?.Invoke(ratio);
    }

    public void StartBossBattle()
    {
        currentBoss.Status.OnChanageEnemyData -= UpdateBossHPBar;
        currentBoss.Status.OnChanageEnemyData += UpdateBossHPBar;

        currentBoss.OnEnemyDie -= ClearBossBattle;
        currentBoss.OnEnemyDie += ClearBossBattle;
        //Managers.UIManager.UIGameScene.MonsterPanel.SetBossHPBar(1f);

        bossRoomGate.CloseGate();
        resonanceGate.DisableGate();
    }

    public void ClearBossBattle(BaseEnemy enemy)
    {
        currentBoss.Status.OnChanageEnemyData -= UpdateBossHPBar;
        currentBoss.OnEnemyDie -= ClearBossBattle;

        bossRoomGate.OpenGate();
        resonanceGate.EnableGate();

        StartCoroutine(CoDungeonClear());
    }

    IEnumerator CoDungeonClear()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1f;

        Managers.UIManager.RequestNotice("Clear");
    }

    public UIGameScene GameSceneUI { get { return gameSceneUI; } }
    public PlayerCharacter Character { get { return character; } }
}
