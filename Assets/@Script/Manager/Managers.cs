using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Managers : Singleton<Managers>
{
    private bool isInitialized = false;

    [SerializeField] private DataManager dataManager = new DataManager();

    private GameManager gameManager = new GameManager();
    private GameSceneManager gameSceneManager = new GameSceneManager();
    private ResourceManager resourceManager = new ResourceManager();
    private UIManager uiManager = new UIManager();
    private AudioManager audioManager = new AudioManager();
    private NPCManager npcManager = new NPCManager();
    private DialogueManager dialogueManager = new DialogueManager();
    private QuestManager questManager = new QuestManager();
    private SlotManager slotManager = new SlotManager();
    private ObjectPoolManager objectPoolManager = new ObjectPoolManager();

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
    }

    public override void Initialize()
    {
        if(isInitialized == true)
        {
            Debug.Log($"{this} Already Initialized.");
            return;
        }
        else
        {
            // UI Canvas ·Îµå
            GameObject canvas = GameObject.Find("@UI Canvas");
            if (canvas == null)
            {
                canvas = ResourceManager.InstantiatePrefabSync("@UI Canvas", transform);
            }
            canvas.transform.SetParent(transform);

            resourceManager.Initialize();
            dataManager.Initialize();
            uiManager.Initialize(canvas);
            gameManager.Initialize();
            gameSceneManager.Initialize();
            audioManager.Initialize(transform);
            slotManager.Initialize(canvas);

            /*
            npcManager.Initialize();
            dialogueManager.Initialize();
            questManager.Initialize();
            objectPoolManager.Initialize(gameObject);
            */

            isInitialized = true;
            Debug.Log($"{this} Initialization Complete!");
        }
    }    

    #region Property
    public static GameManager GameManager { get { return Instance?.gameManager; } }
    public static GameSceneManager GameSceneManager { get { return Instance?.gameSceneManager; } }
    public static ResourceManager ResourceManager { get { return Instance?.resourceManager; } }
    public static UIManager UIManager { get { return Instance?.uiManager; } }
    public static AudioManager AudioManager { get { return Instance?.audioManager; } }
    public static DataManager DataManager { get { return Instance?.dataManager; } }
    public static NPCManager NPCManager { get { return Instance?.npcManager; } }
    public static DialogueManager DialogueManager { get { return Instance?.dialogueManager; } }
    public static QuestManager QuestManager { get { return Instance?.questManager; } }
    public static SlotManager SlotManager { get { return Instance?.slotManager; } }
    public static ObjectPoolManager ObjectPoolManager { get { return Instance?.objectPoolManager; } }
    #endregion
}
