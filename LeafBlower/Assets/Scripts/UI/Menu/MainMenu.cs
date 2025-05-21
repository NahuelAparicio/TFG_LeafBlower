using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : BaseMenu<MainMenu>
{
    public GameObject loadingInspector;
    public Image loadingFillAmountInspector;
    public GameObject continueGo;
    protected override void Awake()
    {
        base.Awake();
        loadingInspector.SetActive(false);
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.MainMenu);
        if(GameManager.Instance.hasStartedNewGame)
        {
            continueGo.SetActive(true);
        }
        else
        {
            continueGo.SetActive(false);
        }
    }
    public void OnNewGamePressed()
    {
        GameManager.Instance.hasStartedNewGame = true;
        ResetData();
        MusicManager.Instance.StopMenuMusic();
        GameManager.Instance.LockCursor();
        GameManager.Instance.LoadLevel("Main Scene", loadingInspector, loadingFillAmountInspector);
    }

    public void OnContinuePressed()
    {

        GameManager.Instance.LockCursor();
        MusicManager.Instance.StopMenuMusic();
        GameManager.Instance.LoadLevel("Main Scene", loadingInspector, loadingFillAmountInspector);
    }

    public EventSystem GetEventSystem()
    {
        if (_eventSystem == null)
        {
            _eventSystem = EventSystem.current;
        }
        return _eventSystem;
    }

    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }


    public void OnSettingsPressed()
    {

        SettingsMenu.Show();
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.SettingsMenu);
    }

    public override void OnBackPressed()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }
}
