using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Managers : Singleton<Managers>
{
    private bool isInitialized = false;

    [SerializeField] private DataManager dataManager = new DataManager();
    [SerializeField] private GameManager gameManager = new GameManager();
    private SceneManagerCS sceneManagerCS = new SceneManagerCS();
    private ResourceManager resourceManager = new ResourceManager();
    private InputManager inputManager = new InputManager();
    private UIManager uiManager = new UIManager();
    private SlotManager slotManager = new SlotManager();
    private NPCManager npcManager = new NPCManager();
    private AudioManager audioManager = new AudioManager();
    private GameEventManager gameEventManager = new GameEventManager();
    private QuestManager questManager = new QuestManager();
    private CompeteManager competeManager;
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
            inputManager.Initialize();
            uiManager.Initialize(transform);
            slotManager.Initialize(gameObject);
            gameManager.Initialize();
            sceneManagerCS.Initialize();
            audioManager.Initialize(transform);
            gameEventManager.Initialize();
            npcManager.Initialize();
            /*
            dialogueManager.Initialize();
            questManager.Initialize();
            */

            // Mono
            competeManager = Functions.GetOrAddComponent<CompeteManager>(gameObject);
            environmentManager = Functions.GetOrAddComponent<EnvironmentManager>(gameObject);

            isInitialized = true;
            Debug.Log($"{this} Initialization Complete!");
        }
    }    

    #region Property
    public static GameManager GameManager { get { return Instance?.gameManager; } }
    public static SceneManagerCS SceneManagerCS { get { return Instance?.sceneManagerCS; } }
    public static ResourceManager ResourceManager { get { return Instance?.resourceManager; } }
    public static InputManager InputManager { get { return Instance?.inputManager; } }
    public static UIManager UIManager { get { return Instance?.uiManager; } }
    public static SlotManager SlotManager { get { return Instance?.slotManager; } }
    public static AudioManager AudioManager { get { return Instance?.audioManager; } }
    public static GameEventManager GameEventManager { get { return Instance?.gameEventManager; } }
    public static DataManager DataManager { get { return Instance?.dataManager; } }
    public static NPCManager NPCManager { get { return Instance?.npcManager; } }
    public static QuestManager QuestManager { get { return Instance?.questManager; } }
    public static CompeteManager CompeteManager { get { return Instance?.competeManager; } }
    public static EnvironmentManager EnvironmentManager { get { return Instance?.environmentManager; } }
    #endregion
}
