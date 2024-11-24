using UnityEngine;

public class ShootableObject : Object, IAspirable, IShooteable, IAttacheable
{
    private bool _isAttached;
    public bool IsAttached => _isAttached;

    public float timeToEnableAspirating = 0.5f;
    private bool _hasBeenShoot = false;

    internal override void Awake()
    {
        base.Awake();
        _isAttached = false;
        _hasBeenShoot = false;
    }

    public void OnAspiratableInteracts(Vector3 force)
    {
        if(!_isAttached && !_hasBeenShoot)
        {
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

    public void OnShoot(Vector3 force)
    {
        if(_isAttached)
        {
            _hasBeenShoot = true;
            Detach();
            _rb.AddForce(force * 10, ForceMode.Impulse);
            Invoke(nameof(ResetAspiratable), timeToEnableAspirating);
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

    private void ResetAspiratable() => _hasBeenShoot = false;
}
