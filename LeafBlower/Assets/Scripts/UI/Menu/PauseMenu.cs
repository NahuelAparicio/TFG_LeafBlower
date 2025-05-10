using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PauseMenu : BaseMenu<PauseMenu>
{
    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
        GameManager.Instance.PauseGameHandler();
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.PauseMenu);

        // Activamos el parámetro global de FMOD al abrir el menú
        RuntimeManager.StudioSystem.setParameterByName("Pause", 1f);
    }

    public void OnRetarget()
    {
        GetEventSystem().SetSelectedGameObject(_firstSelected);
    }

    public EventSystem GetEventSystem()
    {
        if (_eventSystem == null)
        {
            _eventSystem = EventSystem.current;
        }
        return _eventSystem;
    }

    public void OnMainMenu()
    {
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.MainMenu);

        GameManager.Instance.PauseGameHandler();
        Hide();

        // Desactivamos el parámetro global de FMOD al salir al menú principal
        RuntimeManager.StudioSystem.setParameterByName("Pause", 0f);

        SceneManager.LoadScene(0);
    }

    public void OnSettingsMenu()
    {
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.SettingsMenu);
        SettingsMenu.Show();
    }

    public void OnQuitPressed()
    {
        GameManager.Instance.LockCursor();
        GameManager.Instance.UpdateState(Enums.GameState.Playing);
        GameManager.Instance.PauseGameHandler();

        // Desactivamos el parámetro global de FMOD al cerrar el menú
        RuntimeManager.StudioSystem.setParameterByName("Pause", 0f);

        Hide();
        Destroy(gameObject); //This menu doesn't destroy itself
    }
}
