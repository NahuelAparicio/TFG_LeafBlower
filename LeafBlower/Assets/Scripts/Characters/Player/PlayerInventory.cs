using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private int _coins = 0;
    public int Coins => _coins;

    public GameObject objectSaved;

    public Image objectImage;

    private PlayerController _player;

    //TEMPORAL
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        objectSaved = null;
        objectImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameEventManager.Instance.collectingEvents.onCollectCoin += CollectCoin;
        GameEventManager.Instance.collectingEvents.onCollectColectionable += CollectColectionable;
    }

    private void CollectColectionable(string id)
    {
        Debug.Log(id + " Collected");
    }

    private void CollectCoin(int num)
    {
        _coins += num;
        _text.text = "COINS: " + _coins;
    }

    public void SaveObject(GameObject obj, Sprite sprite )
    {
        objectSaved = obj;
        objectSaved.SetActive(false);
        objectImage.sprite = sprite;
        objectImage.gameObject.SetActive(true);
        _player.BlowerController.Aspirer.CheckForNulls();
    }

    public void RemoveObject()
    {
        if (_player.BlowerController.Aspirer.attachableObject.IsAttached) return;

        if (!_player.CheckCollisions.IsGrounded)
        {
            objectSaved.transform.position = transform.position;
        }

        objectSaved.SetActive(true);
        objectImage.gameObject.SetActive(false);

        objectSaved.GetComponent<ShootableObject>().canBeSaved = false;

        if (_player.BlowerController.IsAspirating())
        {
            _player.BlowerController.Aspirer.attachableObject.Attach(objectSaved.GetComponent<Rigidbody>(), objectSaved.transform.position, _player.BlowerController.FirePoint);
        }
        else
        {
            objectSaved.GetComponent<IAttacheable>().Detach();
        }
        objectSaved = null;

    }

    public bool IsObjectSaved() => objectSaved != null;

    private void OnDisable()
    {
        GameEventManager.Instance.collectingEvents.onCollectCoin -= CollectCoin;
        GameEventManager.Instance.collectingEvents.onCollectColectionable -= CollectColectionable;
    }

}
