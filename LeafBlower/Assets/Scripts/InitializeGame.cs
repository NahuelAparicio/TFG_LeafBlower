using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Menu);
        MainMenu.Show();   
    }
}
