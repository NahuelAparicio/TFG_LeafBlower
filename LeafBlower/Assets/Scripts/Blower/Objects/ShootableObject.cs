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
            _rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    public void AttachObject()
    {
        _isAttached = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
