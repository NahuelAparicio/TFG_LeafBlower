using UnityEngine;

public class ShootableObject : MonoBehaviour, IAspirable, IShooteable, IAttacheable
{
    private bool _isAttached;
    private Rigidbody _rb;
    public bool IsAttached => _isAttached;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _isAttached = false;
    }

    public void OnAspiratableInteracts(Vector3 force)
    {
        if(!_isAttached)
        {
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

    public void OnShoot(Vector3 force)
    {
        if(_isAttached)
        {
            Detach();
            _rb.AddForce(force * 10, ForceMode.Impulse);
        }
    }


    public void Attach(Transform pointToAttach, Vector3 positionToAttach, bool isAttachedToObject)
    {
        transform.SetParent(pointToAttach);
        _isAttached = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void Detach()
    {
        transform.SetParent(null);
        _isAttached = false;
        _rb.isKinematic = false;
    }
}
