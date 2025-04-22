using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenu : BaseMenu<SettingsMenu>
{
    public bool isMusic;
    public bool isSettings;
    public GameObject firstSelectedSettings;
    public GameObject firstSelectedMusic;

    public GameObject music, settings, menu;

    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.SettingsMenu);
        menu.SetActive(true);
        settings.SetActive(false);
        music.SetActive(false);
    }

    public EventSystem GetEventSystem()
    {
        if (_eventSystem == null)
        {
            _eventSystem = EventSystem.current;
        }
        return _eventSystem;
    }
    public void OnRetarget()
    {
        if(!isMusic && !isSettings)
        {
            GetEventSystem().SetSelectedGameObject(_firstSelected);
            return;
        }
        if(isMusic)
        {
            GetEventSystem().SetSelectedGameObject(firstSelectedMusic);
            return;
        }
        if(isSettings)
        {
            GetEventSystem().SetSelectedGameObject(firstSelectedSettings);
            return;
        }
    }
    public void BackToMenuSettings()
    {
        isSettings = false;
        isMusic = false;
        settings.SetActive(false);
        music.SetActive(false);
        menu.SetActive(true);
        OnRetarget();
    }
    public void OnOptionsPressed()
    {
        isSettings = true;
        isMusic = false;
        settings.SetActive(true);
        menu.SetActive(false);
        OnRetarget();

    }

    public void OnMusicPressed()
    {
        isSettings = false;
        isMusic = true;
        music.SetActive(true);
        menu.SetActive(false);
        OnRetarget();

    }

    public override void OnBackPressed()
    {
        Hide();
        if (GameManager.Instance.State == Enums.GameState.Playing || GameManager.Instance.State == Enums.GameState.PauseMenu)
        {
            PauseMenu.Show();
        }
        //else
        //{
        //    MenuManager.Instance.ChangeMenuState(Enums.MenuState.MainMenu);
        //    MainMenu.Show();
        //}
    }
}
