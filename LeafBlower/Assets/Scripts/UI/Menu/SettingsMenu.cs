using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;

public class SettingsMenu : BaseMenu<SettingsMenu>
{
    public bool isMusic;
    public bool isSettings;
    public GameObject firstSelectedSettings;
    public GameObject firstSelectedMusic;

    public GameObject music, settings, menu;

    private EventSystem _eventSystem;
    private Slider musicSlider;
    private float lastSliderValue;
    private const float sliderThreshold = 0.01f;

    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        MenuManager.Instance.ChangeMenuState(Enums.MenuState.SettingsMenu);
        menu.SetActive(true);
        settings.SetActive(false);
        music.SetActive(false);

        // 🔍 Buscar el slider dentro del panel de música
        if (music != null)
        {
            musicSlider = music.GetComponentInChildren<Slider>(true);
            if (musicSlider != null)
            {
                lastSliderValue = musicSlider.value;
                musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            }
        }
    }

    private void OnMusicSliderChanged(float value)
    {
        if (Mathf.Abs(value - lastSliderValue) >= sliderThreshold)
        {
            RuntimeManager.PlayOneShot("event:/UI/Selector");
            lastSliderValue = value;
        }
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
        if (!isMusic && !isSettings)
        {
            GetEventSystem().SetSelectedGameObject(_firstSelected);
            return;
        }
        if (isMusic)
        {
            GetEventSystem().SetSelectedGameObject(firstSelectedMusic);
            return;
        }
        if (isSettings)
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

    private void OnDestroy()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }
    }
}
