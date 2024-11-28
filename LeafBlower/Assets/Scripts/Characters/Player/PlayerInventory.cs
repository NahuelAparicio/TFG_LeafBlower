using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject objectSaved;

    private PlayerController _player;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        objectSaved = null;
    }

    public void SaveObject(GameObject obj)
    {
        objectSaved = obj;
        obj.SetActive(false);
    }

    public void RemoveObject()
    {
        objectSaved.SetActive(true);
        objectSaved = null;
    }

    public bool IsObjectSaved() => objectSaved != null;
}
