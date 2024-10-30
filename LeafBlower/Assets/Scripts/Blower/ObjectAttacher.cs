using UnityEngine;

public class ObjectAttacher : MonoBehaviour
{
    public bool isObjectAttached;
    private GameObject _attachedObject;

    private void Awake()
    {
        isObjectAttached = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        IShooteable shooteable = other.GetComponent<IShooteable>();
        if(shooteable != null && !isObjectAttached)
        {
            other.gameObject.transform.SetParent(transform);
            shooteable.AttachObject();
            _attachedObject = other.gameObject;
            isObjectAttached = true;
        }
    }
}
