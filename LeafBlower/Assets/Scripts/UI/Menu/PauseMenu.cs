using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMenu<PauseMenu>
{
    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
        GameManager.Instance.PauseGameHandler();
        MenuManager.Instance.isPauseMenu = true;
    }

    public void OnRetarget()
    {
        GetEventSystem().SetSelectedGameObject(_firstSelected);
    }
    public EventSystem GetEventSystem()
    {
        if( _eventSystem == null )
        {
            _eventSystem = EventSystem.current;
        }
        return _eventSystem;
    } 
    public void OnMainMenu()
    {
        MenuManager.Instance.isPauseMenu = false;

        GameManager.Instance.PauseGameHandler();
        Hide();
        SceneManager.LoadScene(0);
    }

    public void OnSoundMenu()
    {
        Hide();
        MenuManager.Instance.isPauseMenu = false;

        GameManager.Instance.PauseGameHandler();
        Hide();
        SceneManager.LoadScene(1);
    }

    public void OnQuitPressed()
    {
        MenuManager.Instance.isPauseMenu = false;

        GameManager.Instance.UpdateState(Enums.GameState.Playing);
        GameManager.Instance.PauseGameHandler();
        Hide();
        Destroy(gameObject); //This menu doesn't destroy itself
    }
}
