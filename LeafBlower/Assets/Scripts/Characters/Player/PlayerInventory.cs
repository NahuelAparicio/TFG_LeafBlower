using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject objectSaved;

    private PlayerController _player;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();         
    }

    public void SaveObject(GameObject obj)
    {
        objectSaved = obj;
        obj.SetActive(false);
    }

    public void RemoveObject()
    {
      //  _player.BlowerController.Aspirer.AttachObject(objectSaved.GetComponent<Rigidbody>(), ,objectSaved.GetComponent<IShooteable>);
        objectSaved.SetActive(true);
        objectSaved = null;
    }
}
