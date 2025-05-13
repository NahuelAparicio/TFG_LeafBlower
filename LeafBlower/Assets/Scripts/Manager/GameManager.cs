using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Enums.GameState _state = Enums.GameState.Playing;
    private bool _isPaused;
    public bool IsPaused => _isPaused;
    public bool hasStartedNewGame = false;
    public Enums.GameState State => _state;

    // -- Player Sensibility
    [Range(0.1f, 15)] public float sensX = 5f;
    [Range(0.1f, 15)] public float sensY = 5f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        _isPaused = false;
        LoadData();
    }


    public void LoadLevel(string levelName, GameObject _loaderCanvas, Image _progressBar)
    {
        StartCoroutine(LoadSceneAsyc(levelName, _loaderCanvas, _progressBar));
    }

    IEnumerator LoadSceneAsyc(string levelName, GameObject _loaderCanvas, Image _progressBar)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);
        async.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);
        float progress = 0f;
        _progressBar.fillAmount = progress;
        while (!async.isDone)
        {
            _progressBar.fillAmount = progress;
            if (progress >= 0.9f)
            {
                _progressBar.fillAmount = 1;
                async.allowSceneActivation = true;
            }
            progress = async.progress;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        UpdateState(Enums.GameState.Playing);
        MainMenu.Hide();

    }
    public void UpdateState(Enums.GameState state)
    {
        if (_state == state)
        {
            return;
        }

        _state = state;

        switch (_state)
        {
            case Enums.GameState.Menu:
                MainMenu.Show();
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.PauseMenu:
                PauseMenu.Show();
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

    private void OnDisable()
    {
        SaveData();
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("HasStartedGame", hasStartedNewGame ? 1 : 0);
    }

    private void LoadData()
    {
        if(PlayerPrefs.HasKey("HasStartedGame"))
        {
            if (PlayerPrefs.HasKey("HasStartedGame"))
            {
                hasStartedNewGame = PlayerPrefs.GetInt("HasStartedGame") == 1;
            }
        }
    }
}