using UnityEngine;
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
    }
    public void OnPlayPressed()
    {
        // AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);

        GameManager.Instance.LoadLevel("Main Scene", loadingInspector, loadingFillAmountInspector);
    }

    public void OnSettingsPressed()
    {
        //   AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);

        //    SettingsMenu.Show();
    }

    public override void OnBackPressed()
    {
        //    AudioManager.Instance.PlayFx(Enums.Effects.ButtonClick);

        GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }
}
