using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameScene : BaseScene
{
    protected GameSceneData gameSceneData;

    [SerializeField] protected PlayerCharacter character;
    [SerializeField] protected UIGameScene gameSceneUI;
    [SerializeField] protected Vector3 playerDefaultPosition;

    [SerializeField] protected List<NPC> npcList = new List<NPC>();

    [Header("Scene Objects")]
    [SerializeField] protected ResonanceGate[] resonanceGates;
    [SerializeField] protected ResonanceCrystal[] resonanceCrystals;
    [SerializeField] protected EnemySpawner[] enemySpawners;

    [Header("Boss Rooms")]
    [SerializeField] protected BossRoom[] bossRooms;

    public override void Initialize()
    {
        base.Initialize();
        gameSceneData = Managers.DataManager.GameSceneTable[(SCENE_LIST)SceneManager.GetActiveScene().buildIndex];
        scene = gameSceneData.scene;
        sceneType = gameSceneData.sceneType;
        sceneName = gameSceneData.sceneName;
        playerDefaultPosition = Functions.FindObjectFromChild(gameObject, "Prefab_Player_Default_Position", true).transform.position;

        // Initialize NPC
        for (int i = 0; i < npcList.Count; ++i)
        {
            Managers.NPCManager.NPCDictionary.Add(npcList[i].NpcID, npcList[i]);
            npcList[i].Initialize();
        }

        // Initialize Boss Rooms
        bossRooms = GetComponentsInChildren<BossRoom>(true);
        for (int i = 0; i < bossRooms.Length; ++i)
        {
            bossRooms[i].Initialize(this);
        }

        // Initialize Resonance Gates
        resonanceGates = Functions.FindObjectFromChild(gameObject, "Resonance_Gates", true).GetComponentsInChildren<ResonanceGate>(true);
        for (int i = 0; i < resonanceGates.Length; ++i)
        {
            if (int.TryParse(resonanceGates[i].name.Replace("Prefab_Resonance_Gate_", ""), out int resonanceGateID))
            {
                ResonanceGateData resonanceGateData = Managers.DataManager.ResonanceGateTable[resonanceGateID];
                resonanceGates[i].Initialize(resonanceGateData);
            }
            else
            {
                Debug.LogAssertion("ID Parse Error: " + resonanceCrystals[i].name);
            }
        }

        // Initialize Resonance Crystals
        resonanceCrystals = Functions.FindObjectFromChild(gameObject, "Resonance_Crystals", true).GetComponentsInChildren<ResonanceCrystal>(true);
        for (int i = 0; i < resonanceCrystals.Length; ++i)
        {
            if (int.TryParse(resonanceCrystals[i].name.Replace("Prefab_Resonance_Crystal_", ""), out int resonanceCrystalID))
            {
                ResonanceCrystalData resonanceCrystalData = Managers.DataManager.ResonanceCrystalTable[resonanceCrystalID];
                resonanceCrystals[i].Initialize(resonanceCrystalData);
            }
            else
            {
                Debug.LogAssertion("ID Parse Error: " + resonanceCrystals[i].name);
            }
        }
        
        // Initialize Enemy Spawner
        enemySpawners = Functions.FindObjectFromChild(gameObject, "Enemy_Spawners", true).GetComponentsInChildren<EnemySpawner>(true);
        for (int i = 0; i < enemySpawners.Length; ++i)
        {
            if (int.TryParse(enemySpawners[i].name.Replace("Prefab_Enemy_Spawner_", ""), out int spawnerID))
            {
                EnemySpawnerData enemySpawnData = Managers.DataManager.EnemySpawnerTable[spawnerID];
                enemySpawners[i].Initialize(this, enemySpawnData);

                switch (enemySpawners[i].EnemyData.enemyType)
                {
                    case ENEMY_TYPE.Normal:
                        enemySpawners[i].SpawnEnemy();
                        break;
                    case ENEMY_TYPE.Elite:
                        enemySpawners[i].SpawnEnemy();
                        break;
                }
            }
            else
            {
                Debug.LogAssertion("ID Parse Error: " + enemySpawners[i].name);
            }
        }

        // Player Character
        if (Managers.DataManager.CurrentCharacterData != null)
        {
            // Create Resonance Water Render Camera
            Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Camera_For_Render_Resonance_Water);

            // Create Player Character
            if (Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Player_Character).TryGetComponent(out character))
            {
                character.InitializeCharacter(Managers.DataManager.CurrentCharacterData);
                character.CharacterData.LocationData.LastScene = scene;
                character.transform.position = Managers.DataManager.CurrentCharacterData.LocationData.GetCharacterLastLocation(this);
                character.gameObject.SetActive(true);

                // Create Player Camera
                if (Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_Player_Camera).TryGetComponent(out PlayerCamera playerCamera))
                {
                    playerCamera.Initialize(character);
                    character.PlayerCamera = playerCamera;
                }
            }
        }

        // Initialize Game Scene UI
        if (Managers.ResourceManager.InstantiatePrefabSync(Constants.Prefab_UI_Scene_Game).TryGetComponent(out gameSceneUI))
        {
            gameSceneUI.transform.SetParent(Managers.UIManager.RootObject.transform);
            gameSceneUI.transform.SetAsFirstSibling();
            gameSceneUI.Initialize(character.CharacterData);

            if (gameSceneUI.gameObject.activeSelf == false)
                gameSceneUI.gameObject.SetActive(true);
        }

        RegisterObject(Constants.Prefab_Floating_Damage_Text, 16);
        Managers.GameManager.SetCursorMode(false);
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
        for (int i = 0; i < enemySpawners.Length; ++i)
        {
            enemySpawners[i].SpawnEnemy();
        }
    }

    public UIGameScene GameSceneUI { get { return gameSceneUI; } }
    public PlayerCharacter Character { get { return character; } }
    public Vector3 PlayerDefaultPosition { get { return playerDefaultPosition; } }
    public ResonanceCrystal[] ResonanceCrystals { get { return resonanceCrystals; } }
}
