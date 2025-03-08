using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : BaseMenu<MainMenu>
{
    public GameObject loadingInspector;
    public Image loadingFillAmountInspector;

    protected override void Awake()
    {
        base.Awake();
        loadingInspector.SetActive(false);
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.isMainMenu = true;
    }
    public void OnPlayPressed()
    {
        // AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);
        MenuManager.Instance.isMainMenu = false;

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
    public void OnRetarget()
    {
        GetEventSystem().SetSelectedGameObject(_firstSelected);
    }

    public void OnSettingsPressed()
    {
        MenuManager.Instance.isMainMenu = false;
        //   AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);
        SettingsMenu.Show();
        MenuManager.Instance.isInSettingsMenu = true;
    }

    public override void OnBackPressed()
    {
        //    AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);
        MenuManager.Instance.isMainMenu = false;

        GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }
}
