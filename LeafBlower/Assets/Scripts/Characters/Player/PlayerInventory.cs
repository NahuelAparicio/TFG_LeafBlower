using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public GameObject objectSaved;

    public Image objectImage;

    private PlayerController _player;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        objectSaved = null;
        objectImage.gameObject.SetActive(false);
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
            objectSaved.transform.position = transform.position;

        objectSaved.SetActive(true);
        objectImage.gameObject.SetActive(false);

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
}
