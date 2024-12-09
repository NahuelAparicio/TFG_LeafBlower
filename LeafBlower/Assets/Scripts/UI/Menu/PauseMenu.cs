using UnityEngine;
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
    }
    public void OnMainMenu()
    {
        GameManager.Instance.PauseGameHandler();
        Hide();
        SceneManager.LoadScene(0);
    }
    public void OnQuitPressed()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Playing);
        GameManager.Instance.PauseGameHandler();
        Hide();
        Destroy(gameObject); //This menu doesn't destroy itself
    }
}
