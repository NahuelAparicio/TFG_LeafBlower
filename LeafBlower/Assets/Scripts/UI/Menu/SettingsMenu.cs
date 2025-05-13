using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;

public class SettingsMenu : BaseMenu<SettingsMenu>
{
    public bool isMusic;
    public bool isSettings;

    public GameObject music, settings, menu;

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

        // Buscar el slider dentro del panel de música
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
    }

    public void BackToMenuSettings()
    {
        isSettings = false;
        isMusic = false;
        settings.SetActive(false);
        music.SetActive(false);
        menu.SetActive(true);
    }

    public void OnOptionsPressed()
    {
        isSettings = true;
        isMusic = false;
        settings.SetActive(true);
        menu.SetActive(false);
    }

    public void OnMusicPressed()
    {
        isSettings = false;
        isMusic = true;
        music.SetActive(true);
        menu.SetActive(false);
    }

    public override void OnBackPressed()
    {
        Hide();

        if (GameManager.Instance.State == Enums.GameState.Playing || GameManager.Instance.State == Enums.GameState.PauseMenu)
        {
            PauseMenu.Show();
        }

    }

    private void OnDestroy()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }
    }
}
