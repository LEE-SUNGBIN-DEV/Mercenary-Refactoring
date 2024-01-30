using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Managers : Singleton<Managers>
{
    private ResourceManager resourceManager = new ResourceManager();
    [SerializeField] private DataManager dataManager = new DataManager();
    private GameManager gameManager;
    [SerializeField] private InputManager inputManager = new InputManager();
    private UIManager uiManager;
    private AudioManager audioManager = new AudioManager();
    private SceneManagerEX sceneManagerEX = new SceneManagerEX();

    private PostProcessingManager postProcessingManager;
    private SpecialCombatManager specialCombatManager;
    private EnvironmentManager environmentManager;

    public override void Initialize()
    {
        if(isInitialized == true)
        {
            Debug.Log($"{this} Already Initialized.");
            return;
        }
        else
        {
            resourceManager.Initialize();
            dataManager.Initialize();
            gameManager = Functions.GetOrAddComponent<GameManager>(gameObject);
            gameManager.Initialize();
            inputManager.Initialize();
            uiManager = Functions.GetOrAddComponent<UIManager>(gameObject);
            uiManager.Initialize();
            audioManager.Initialize(gameObject);
            sceneManagerEX.Initialize();
            /*
            dialogueManager.Initialize();
            */

            postProcessingManager = Functions.GetOrAddComponent<PostProcessingManager>(gameObject);
            postProcessingManager.Initialize(gameObject);
            specialCombatManager = Functions.GetOrAddComponent<SpecialCombatManager>(gameObject);
            specialCombatManager.Initialize();
            environmentManager = Functions.GetOrAddComponent<EnvironmentManager>(gameObject);
            environmentManager.Initialize();

            isInitialized = true;
            Debug.Log($"{this} Initialization Complete!");
        }
    }

    private void OnApplicationQuit()
    {
        dataManager?.SavePlayerData();
    }

    #region Property
    public static GameManager GameManager { get { return Instance != null ? Instance.gameManager : null; } }
    public static SceneManagerEX SceneManagerEX { get { return Instance != null ? Instance.sceneManagerEX : null; } }
    public static ResourceManager ResourceManager { get { return Instance != null ? Instance.resourceManager : null; } }
    public static InputManager InputManager { get { return Instance != null ? Instance.inputManager : null; } }
    public static UIManager UIManager { get { return Instance != null ? Instance.uiManager : null; } }
    public static AudioManager AudioManager { get { return Instance != null ? Instance.audioManager : null; } }
    public static DataManager DataManager { get { return Instance != null ? Instance.dataManager : null; } }

    public static PostProcessingManager PostProcessingManager { get { return Instance != null ? Instance.postProcessingManager : null; } }
    public static SpecialCombatManager SpecialCombatManager { get { return Instance != null ? Instance.specialCombatManager : null; } }
    public static EnvironmentManager EnvironmentManager { get { return Instance != null ? Instance.environmentManager : null; } }
    #endregion
}
