using UnityEngine;

public class ShootableObject : MonoBehaviour, IAspirable, IShooteable
{
    private bool _isAttached;
    public bool IsAttached => _isAttached;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _isAttached = false;
    }

    public void OnAspiratableInteracts(float force, Vector3 direction)
    {
        if(!_isAttached)
        {
            _rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    public void OnShoot(float force, Vector3 direction)
    {
        if(_isAttached)
        {
            _isAttached = false;
            _rb.isKinematic = false;
            _rb.AddForce(direction * (force * 20), ForceMode.Impulse);
        }
    }

    public void AttachObject()
    {
        _isAttached = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
