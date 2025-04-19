using System;
using Unity.VisualScripting;
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
        GameEventManager.Instance.collectingEvents.OnCollectColectionable += CollectColectionable;
    }

    public void RemoveCoins(int v)
    {
        _coins -= v;

        if(_coins <= 0)
        {
            _coins = 0;
        }
        _controller.Hud.UpdateCoinsText(_coins);
    }

    private void CollectColectionable(Enums.CollectionableType type, int amount)
    {
        switch (type)
        {
            case Enums.CollectionableType.Coin:
                _coins += amount;
                _controller.Hud.UpdateCoinsText(_coins);
                break;
            case Enums.CollectionableType.Jordan:
                Debug.Log("Collected Jordans " + amount);
                break;
            default:
                break;
        }
    }

}
