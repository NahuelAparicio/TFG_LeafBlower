using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerController _controller;
    private int _coins = 0;
    public int Coins => _coins;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    void Start()
    {
        GameEventManager.Instance.collectingEvents.onCollectCoin += CollectCoin;
        GameEventManager.Instance.collectingEvents.onCollectColectionable += CollectColectionable;
    }

    private void CollectColectionable(string id)
    {
        Debug.Log(id + " Collected");
    }

    public void CollectCoin(int num)
    {
        _coins += num;
        _controller.Hud.UpdateCoinsText(_coins);
    }
}
