using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameScene : BaseScene
{
    protected GameSceneData gameSceneData;

    [SerializeField] protected PlayerCharacter scenePlayerCharacter;
    [SerializeField] protected Vector3 playerDefaultPosition;

    [Header("Scene Objects")]
    [SerializeField] protected Dictionary<string, ResponseCrystal> responseCrystalsDictionary = new Dictionary<string, ResponseCrystal>();
    [SerializeField] protected Dictionary<string ,ResponseGate> responseGateDictionary = new Dictionary<string, ResponseGate> ();
    [SerializeField] protected Dictionary<string, BossRoom> bossRoomDictionary = new Dictionary<string, BossRoom>();
    [SerializeField] protected Dictionary<string, NPC> npcDictionary = new Dictionary<string, NPC>();

    [SerializeField] protected ResponseTrace[] responseTraces;
    [SerializeField] protected EnemySpawner[] enemySpawners;
    [SerializeField] protected TreasureBox[] treasureBoxes;

    public override void Initialize()
    {
        base.Initialize();
        gameSceneData = Managers.DataManager.GameSceneTable[(SCENE_ID)SceneManager.GetActiveScene().buildIndex];
        scene = gameSceneData.sceneID;
        sceneType = gameSceneData.sceneType;
        sceneName = gameSceneData.sceneName;
        playerDefaultPosition = Functions.FindObjectFromChild(gameObject, "Prefab_Player_Default_Position", true).transform.position;

        // Initialize Response Gates
        ResponseGate[] responseGates = Functions.FindObjectFromChild(gameObject, "Response_Gates", true).GetComponentsInChildren<ResponseGate>(true);
        for (int i = 0; i < responseGates.Length; ++i)
        {
            if (Managers.DataManager.ResponseGateTable.TryGetValue(responseGates[i].name, out ResponseGateData responseGateData))
            {
                responseGates[i].Initialize(responseGateData);
                responseGateDictionary.Add(responseGates[i].ResponseGateData.responseGateID, responseGates[i]);
            }
            else
            {
                Debug.LogWarning($"ID Parse Error: {responseGates[i].name}");
            }
        }

        // Initialize Response Crystals
        ResponseCrystal[] responseCrystals = Functions.FindObjectFromChild(gameObject, "Response_Crystals", true).GetComponentsInChildren<ResponseCrystal>(true);
        for (int i = 0; i < responseCrystals.Length; ++i)
        {
            if (Managers.DataManager.ResponseCrystalTable.TryGetValue(responseCrystals[i].name, out ResponseCrystalData responseCrystalData))
            {
                responseCrystals[i].Initialize(responseCrystalData);
                responseCrystalsDictionary.Add(responseCrystals[i].ResponseCrystalData.responseCrystalID, responseCrystals[i]);
            }
            else
            {
                Debug.LogWarning($"ID Parse Error: {responseCrystals[i].name}");
            }
        }

        // Initialize Response Trace
        responseTraces = Functions.FindObjectFromChild(gameObject, "Response_Traces", true).GetComponentsInChildren<ResponseTrace>(true);
        for (int i = 0; i < responseTraces.Length; ++i)
        {
            if (Managers.DataManager.ResponseTraceTable.TryGetValue(responseTraces[i].name, out ResponseTraceData responseTraceData))
            {
                responseTraces[i].Initialize(responseTraceData);
            }
            else
            {
                Debug.LogWarning($"ID Parse Error: {responseTraces[i].name}");
            }
        }

        // Initialize Enemy Spawner
        enemySpawners = Functions.FindObjectFromChild(gameObject, "Enemy_Spawners", true).GetComponentsInChildren<EnemySpawner>(true);
        for (int i = 0; i < enemySpawners.Length; ++i)
        {
            if (Managers.DataManager.EnemySpawnerTable.TryGetValue(enemySpawners[i].name, out EnemySpawnerData enemySpawnData))
            {
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
                Debug.LogWarning($"ID Parse Error: {enemySpawners[i].name}");
            }
        }

        // Treasure Boxes
        treasureBoxes = Functions.FindObjectFromChild(gameObject, "Treasure_Boxes", true).GetComponentsInChildren<TreasureBox>(true);
        for (int i = 0; i < treasureBoxes.Length; ++i)
        {
            if (Managers.DataManager.TreasureBoxTable.TryGetValue(treasureBoxes[i].name, out TreasureBoxData treasureBoxData))
            {
                treasureBoxes[i].Initialize(treasureBoxData);
            }
            else
            {
                Debug.LogWarning($"ID Parse Error: {treasureBoxes[i].name}");
            }
        }

        // Initialize NPC
        NPC[] npcs = Functions.FindObjectFromChild(gameObject, "NPCs", true).GetComponentsInChildren<NPC>(true);
        if(npcs != null)
        {
            for (int i = 0; i < npcs.Length; ++i)
            {
                if (Managers.DataManager.NPCTable.TryGetValue(npcs[i].name, out NPCData npcData))
                {
                    npcs[i].Initialize(npcData);
                    npcDictionary.Add(npcs[i].NPCData.npcID, npcs[i]);
                }
                else
                {
                    Debug.LogWarning($"ID Parse Error: {npcs[i].name}");
                }
            }
        }

        // Initialize Boss Rooms
        BossRoom[] bossRooms = GetComponentsInChildren<BossRoom>(true);
        for (int i = 0; i < bossRooms.Length; ++i)
        {
            bossRooms[i].Initialize(this);
            bossRoomDictionary.Add(bossRooms[i].name, bossRooms[i]);
        }

        // Player Character
        if (Managers.DataManager.CurrentCharacterData != null)
        {
            // Create Player Character
            if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_PLAYER_CHARACTER).TryGetComponent(out scenePlayerCharacter))
            {
                scenePlayerCharacter.CharacterData.LocationData.LastScene = scene;
                scenePlayerCharacter.transform.position = Managers.DataManager.CurrentCharacterData.LocationData.GetCharacterLastLocation(this);
                scenePlayerCharacter.gameObject.SetActive(true);

                // Create Player Camera
                if (Managers.ResourceManager.InstantiatePrefabSync(Constants.PREFAB_PLAYER_CAMERA).TryGetComponent(out PlayerCamera playerCamera))
                {
                    playerCamera.Initialize(scenePlayerCharacter);
                    scenePlayerCharacter.PlayerCamera = playerCamera;
                }
            }
        }

        RegistObject(Constants.PREFAB_FLOATING_DAMAGE_TEXT, 16);
        Managers.UIManager.UIFixedPanelCanvas.SceneNamePanel.OpenPanel(sceneName);
        Managers.UIManager.UIFixedPanelCanvas.CharacterPanel.OpenPanel();
        Managers.UIManager.SetCursorMode(CURSOR_MODE.LOCK);
        Managers.InputManager.SwitchInputMode(CHARACTER_INPUT_MODE.ALL);
    }

    public void Start()
    {
        Managers.EnvironmentManager.SetWeather(gameSceneData.weatherType);
    }

    public override void ExitScene()
    {
        base.ExitScene();
        scenePlayerCharacter = null;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < enemySpawners.Length; ++i)
        {
            enemySpawners[i].SpawnEnemy();
        }
    }

    public PlayerCharacter ScenePlayerCharacter { get { return scenePlayerCharacter; } }
    public Vector3 PlayerDefaultPosition { get { return playerDefaultPosition; } }
    public Dictionary<string, ResponseCrystal> ResponseCrystalsDictionary { get { return responseCrystalsDictionary; } }
    public Dictionary<string, ResponseGate> ResponseGateDictionary { get { return responseGateDictionary; } }
    public Dictionary<string, BossRoom> BossRoomDictionary { get { return bossRoomDictionary; } }
}
