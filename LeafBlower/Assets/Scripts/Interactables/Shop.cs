using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _shopCanvas;
    [SerializeField]private GameObject _jordanView, _hoverView;
    public GameObject jordanSelected, hoverSelected, returnSelected;
    public int jordanValue, hoverValue;


    private PlayerController _player;
    private bool _isMenuOpen;
    private PlayerInteractable _interactable;
    private Collider _collider;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _collider = GetComponent<Collider>();
        _shopCanvas.SetActive(false);
    }

    private void Update()
    {
        if(_isMenuOpen)
        {
            if(EventSystem.current.firstSelectedGameObject == null)
            {
                //EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }
    }

    public void OnInteract()
    {
        if (_player.isTalking || GameManager.Instance.IsPaused) return;
        GameManager.Instance.PauseGameHandler();
        _isMenuOpen = true;
        _shopCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(returnSelected);
    }

    public void BuyJordans()
    {
        if (_player.Inventory.Coins < jordanValue) return;
        _player.Inventory.RemoveCoins(jordanValue);
        _player.jordanUnlocked = true;
        Destroy(_jordanView);
        if(hoverSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(hoverSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(returnSelected);
        }
    }

    public void BuyHover()
    {
        if (_player.Inventory.Coins < hoverValue) return;
        _player.Inventory.RemoveCoins(hoverValue);
        _player.hoverUnlocked = true;

        Destroy(_hoverView);
        if (jordanSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(jordanSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(returnSelected);
        }
    }

    public void SetInteractableParent(PlayerInteractable parent) => _interactable = parent;

    public void OnEnableCollider()
    {
        _collider.enabled = true;
    }

    public void OnDisableCollider()
    {
        _collider.enabled = false;
        _interactable.RemoveInteractable(gameObject);
    }

    public void HideMenu()
    {
        GameManager.Instance.PauseGameHandler();
        Invoke(nameof(OnEnableCollider), 0.5f);
        _shopCanvas.SetActive(false);
    }

}
