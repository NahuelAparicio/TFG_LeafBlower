using UnityEngine;

public class ObjectAttacher : MonoBehaviour
{
    public bool isObjectAttached;
    private IShooteable _shooteableAttached;
    public IShooteable ShooteableAttached => _shooteableAttached;

    private void Awake()
    {
        isObjectAttached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IShooteable shooteable = other.GetComponent<IShooteable>();
        if(shooteable != null && !isObjectAttached )
        {
            other.gameObject.transform.SetParent(transform);
            shooteable.AttachObject();
            _shooteableAttached = shooteable;
            isObjectAttached = true;
        }
    }

    public void DeattachObject()
    {
        isObjectAttached = false;
        _shooteableAttached = null;
    }
}
