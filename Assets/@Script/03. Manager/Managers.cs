using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Managers : Singleton<Managers>
{
    private bool isInitialized = false;

    [SerializeField] private DataManager dataManager = new DataManager();

    private GameManager gameManager = new GameManager();
    private SceneManagerCS sceneManagerCS = new SceneManagerCS();
    private ResourceManager resourceManager = new ResourceManager();
    private InputManager inputManager = new InputManager();
    private UIManager uiManager = new UIManager();
    private SlotManager slotManager = new SlotManager();
    private NPCManager npcManager = new NPCManager();
    private AudioManager audioManager = new AudioManager();
    private EventManager eventManager = new EventManager();
    private QuestManager questManager = new QuestManager();

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
            eventManager.Initialize();
            npcManager.Initialize();

            /*
            dialogueManager.Initialize();
            questManager.Initialize();
            */

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
    public static EventManager EventManager { get { return Instance?.eventManager; } }
    public static DataManager DataManager { get { return Instance?.dataManager; } }
    public static NPCManager NPCManager { get { return Instance?.npcManager; } }
    public static QuestManager QuestManager { get { return Instance?.questManager; } }
    #endregion
}
