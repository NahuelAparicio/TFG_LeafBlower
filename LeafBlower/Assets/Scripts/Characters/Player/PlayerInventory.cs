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
    [SerializeField] private PlayerRespawnHandler _respawner;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        objectSaved = null;
        objectImage.gameObject.SetActive(false);
        LoadData();
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

    public void CollectCoin(int num)
    {
        _coins += num;
        _text.text =""+_coins; //Bryan: He cambiado esto para que se adapte al nuevo HUD segun el concept
    }

    public void SaveObject(GameObject obj, Sprite sprite )
    {
        _player.CheckCollisions.SetGrounded(false);
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

        if (_player.BlowerController.IsAspirating() && _player.CheckCollisions.IsGrounded)
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
        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Gold", _coins);
        PlayerPrefs.SetInt("PosX", (int)_respawner.PositionToRespawn.x);
        PlayerPrefs.SetInt("PosY", (int)_respawner.PositionToRespawn.y + 1);
        PlayerPrefs.SetInt("PosZ", (int)_respawner.PositionToRespawn.z);
        PlayerPrefs.SetInt("PlayerLevel", _player.Stats.Level);
    }

    private void LoadData()
    {
        _coins = PlayerPrefs.GetInt("Gold");
        _text.text = "" + _coins;
        if(PlayerPrefs.HasKey("PosX"))
        {
            transform.position = new Vector3(PlayerPrefs.GetInt("PosX"), PlayerPrefs.GetInt("PosY"), PlayerPrefs.GetInt("PosZ"));
        }
        if(PlayerPrefs.HasKey("PlayerLevel"))
        {
            _player.Stats.SetLevel(PlayerPrefs.GetInt("PlayerLevel"));
        }
    }

  

}
