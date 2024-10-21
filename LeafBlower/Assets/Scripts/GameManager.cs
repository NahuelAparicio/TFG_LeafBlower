using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
        set
        {

        }
    }

    private Enums.GameState _state;
    private bool _isPaused;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateState(Enums.GameState state)
    {
        if(_state == state)
        {
            return;
        }

        _state = state;

        switch (_state)
        {
            case Enums.GameState.Menu:
                //MainMenu.Show();
                //LoadMenuScene ?
                break;
            case Enums.GameState.Playing:
                //PlaceHolder
                break;
            case Enums.GameState.PauseMenu:
                //PauseMenu.Show()
                break;
            case Enums.GameState.Exit:
                Application.Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_state), state, null);
        }
    }
    public void PauseGameHandler()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
    }
}
