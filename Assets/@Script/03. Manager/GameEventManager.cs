using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GAME_EVENT_TYPE
{
    OnPlayerDie,
    OnEnemyDie,
    OnKillEnemy
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

public class GameEventManager
{
    public event UnityAction<PlayerCharacter> OnPlayerDie;
    public event UnityAction<BaseEnemy> OnEnemyDie;
    public event UnityAction<BaseEnemy> OnKillEnemy;

    private Queue<GameEventMessage> eventQueue;

    public void Initialize()
    {

    }

    public void Update()
    {
        while (eventQueue.Count > 0)
        {
            if (eventQueue.TryDequeue(out GameEventMessage eventMessage))
                Execute(eventMessage);
        }
    }

    public void Execute(GameEventMessage eventMessage)
    {
        switch (eventMessage.eventType)
        {
            case GAME_EVENT_TYPE.OnPlayerDie:
                OnPlayerDie?.Invoke(eventMessage.sender as PlayerCharacter);
                break;
            case GAME_EVENT_TYPE.OnEnemyDie:
                OnEnemyDie?.Invoke(eventMessage.sender as BaseEnemy);
                break;
            case GAME_EVENT_TYPE.OnKillEnemy:
                OnKillEnemy?.Invoke(eventMessage.sender as BaseEnemy);
                break;
        }
    }

    #region Property
    public Queue<GameEventMessage> EventQueue { get { return eventQueue; } }
    #endregion
}
