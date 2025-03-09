using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD;
using FMODUnityResonance;
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
    }

    private Enums.GameState _state = Enums.GameState.Playing;
    private bool _isPaused;
    public bool IsPaused => _isPaused;

    public Enums.GameState State => _state;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if(_state == state)
        {
            return;
        }

        _state = state;

        switch (_state)
        {
            case Enums.GameState.Menu:
                MusicManager.Instance.PlayMenuMusic();
                //MainMenu.Show();
                //LoadMenuScene ?
                break;
            case Enums.GameState.Playing:
                //PlaceHolder
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
}
