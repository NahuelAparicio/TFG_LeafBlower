using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private static GameEventManager _instance;

    public static GameEventManager Instance
    {
        get
        {
            if(_instance == null )
            {
                GameObject go = new GameObject("Game Events");
                go.AddComponent<GameEventManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public event Action onCollectCoin;
    public event Action onButtonPushed;
    public QuestEvents questEvents;
    private void Awake()
    {
        if( _instance == null )
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        questEvents = new QuestEvents();
    }

    public void CoinCollected() => onCollectCoin?.Invoke();

    public void OnButtonPushed() => onButtonPushed?.Invoke();

}
