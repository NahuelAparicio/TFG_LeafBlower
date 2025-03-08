using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsMenu : BaseMenu<SettingsMenu>
{
    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.isInSettingsMenu = true;
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
        MenuManager.Instance.isInSettingsMenu = false;
        MenuManager.Instance.isMainMenu = true;
        Hide();
        //MenuManager.Instance.isMainMenu = false;
        MainMenu.Show();

    }
}
