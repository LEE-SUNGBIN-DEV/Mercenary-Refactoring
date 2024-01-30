using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GAME_EVENT_TYPE
{
    // Player
    PLAYER_DIE,
    PLAYER_KILL_ENEMY,

    // Enemy
    ENEMY_DIE,
}

public struct GameEventMessage
{
    public GAME_EVENT_TYPE eventType;
    public object sender;

    public GameEventMessage(GAME_EVENT_TYPE eventType, object sender)
    {
        this.eventType = eventType;
        this.sender = sender;
    }
}

[System.Serializable]
public class GameManager: MonoBehaviour
{
    public event UnityAction<PlayerCharacter> OnPlayerDie;
    public event UnityAction<BaseEnemy> OnPlayerKillEnemy;
    public event UnityAction<BaseEnemy> OnEnemyDie;

    [Header("Game Event Queue")]
    private Queue<GameEventMessage> gameEventQueue;

    [Header("Camera")]
    [SerializeField] private BaseCamera activedCamera;
    [SerializeField] private DirectingCamera directingCamera;
    private IEnumerator timeScaleCoroutine;

    #region Private
    private void Update()
    {
        while (gameEventQueue.Count > 0)
        {
            if (gameEventQueue.TryDequeue(out GameEventMessage eventMessage))
                Execute(eventMessage);
        }
    }
    private void Execute(GameEventMessage eventMessage)
    {
        switch (eventMessage.eventType)
        {
            case GAME_EVENT_TYPE.PLAYER_DIE:
                OnPlayerDie?.Invoke(eventMessage.sender as PlayerCharacter);
                break;

            case GAME_EVENT_TYPE.PLAYER_KILL_ENEMY:
                OnPlayerKillEnemy?.Invoke(eventMessage.sender as BaseEnemy);
                break;

            case GAME_EVENT_TYPE.ENEMY_DIE:
                OnEnemyDie?.Invoke(eventMessage.sender as BaseEnemy);
                break;
        }
    }
    #endregion

    public void Initialize()
    {
        gameEventQueue = new Queue<GameEventMessage>();
    }

    public void SaveAndQuit()
    {
        Managers.DataManager.SavePlayerData();
        Application.Quit();
    }

    public IEnumerator CoSetTimeScale(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    public void SendEventMessage(GameEventMessage gameEventMessage)
    {
        gameEventQueue.Enqueue(gameEventMessage);
    }

    #region Property
    public BaseCamera ActivedCamera { get { return activedCamera; } set { activedCamera = value; } }
    #endregion
}
