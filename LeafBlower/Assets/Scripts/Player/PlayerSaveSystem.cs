using UnityEngine;

public class PlayerSaveSystem : MonoBehaviour
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        LoadPlayerData();
    }

    private void OnDestroy()
    {
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("Gold", _controller.Inventory.Coins);
        //PlayerPrefs.SetInt("PosX", (int)_respawner.PositionToRespawn.x);
        //PlayerPrefs.SetInt("PosY", (int)_respawner.PositionToRespawn.y + 1);
        //PlayerPrefs.SetInt("PosZ", (int)_respawner.PositionToRespawn.z);
        //PlayerPrefs.SetInt("PlayerLevel", _player.Stats.Level);
    }

    public void LoadPlayerData()
    {
        _controller.Inventory.SetCoins(PlayerPrefs.GetInt("Gold"));
        //_text.text = "" + _coins;
        //if (PlayerPrefs.HasKey("PosX"))
        //{
        //    transform.position = new Vector3(PlayerPrefs.GetInt("PosX"), PlayerPrefs.GetInt("PosY"), PlayerPrefs.GetInt("PosZ"));
        //}
        //if (PlayerPrefs.HasKey("PlayerLevel"))
        //{
        //    _player.Stats.SetLevel(PlayerPrefs.GetInt("PlayerLevel"));
        //}
    }
}
