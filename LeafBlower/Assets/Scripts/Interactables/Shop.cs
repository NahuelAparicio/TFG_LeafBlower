using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject instantiationPoint;
    public GameObject gnomo;
    [SerializeField] private GameObject _shopCanvas;
    [SerializeField]private GameObject _jordanView, _hoverView, _gnomoView;
    public int jordanValue, hoverValue, gnomoValue;

    [SerializeField] private TextMeshProUGUI textJordan, textHover, textGnomo;


    private PlayerController _player;
    private bool _isMenuOpen;
    private PlayerInteractable _interactable;
    private Collider _collider;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _collider = GetComponent<Collider>();
        _shopCanvas.SetActive(false);

        textJordan.text = ""+jordanValue;
        textHover.text = "" + hoverValue;
        textGnomo.text = "" + gnomoValue;
    }


    public void OnInteract()
    {
        if (_player.isTalking || GameManager.Instance.IsPaused) return;
        GameManager.Instance.PauseGameHandler();
        _isMenuOpen = true;
        _shopCanvas.SetActive(true);
        GameManager.Instance.UnlockCursor();
    }

    public void BuyJordans()
    {
        if (_player.Inventory.Coins < jordanValue) return;
        _player.Inventory.RemoveCoins(jordanValue);
        _player.jordanUnlocked = true;
        Destroy(_jordanView);
    }

    public void BuyHover()
    {
        if (_player.Inventory.Coins < hoverValue) return;
        _player.Inventory.RemoveCoins(hoverValue);
        _player.hoverUnlocked = true;

        Destroy(_hoverView);
    }

    public void BuyGnomo()
    {
        if (_player.Inventory.Coins < gnomoValue) return;
        _player.Inventory.RemoveCoins(gnomoValue);

        gnomoValue = 0;
        textGnomo.text = ""+gnomoValue;
        Instantiate(gnomo,instantiationPoint.transform.position,Quaternion.identity);
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
        GameManager.Instance.LockCursor();
        Invoke(nameof(OnEnableCollider), 0.5f);
        _shopCanvas.SetActive(false);
    }

}
