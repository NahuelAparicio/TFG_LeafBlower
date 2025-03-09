using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenu : BaseMenu<SettingsMenu>
{

    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.SettingsMenu);
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
        GetEventSystem().SetSelectedGameObject(_firstSelected);
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
