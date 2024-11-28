using UnityEngine;

public class ShootableObject : Object, IAspirable, IShooteable, IAttacheable
{
    private bool _isAttached;
    public bool IsAttached => _isAttached;

    public float timeToEnableAspirating = 0.5f;
    private bool _hasBeenShoot = false;
    float distanceBetweenParentAndObject;

    internal override void Awake()
    {
        base.Awake();
        _isAttached = false;
        _hasBeenShoot = false;

    }
    private void Update()
    {
        if (_isAttached)
        {
            Vector3 targetPosition = transform.parent.position + (transform.parent.forward * distanceBetweenParentAndObject);
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 25f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collisioned");
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
            _rb.AddForce(force, ForceMode.Impulse);
            Invoke(nameof(ResetAspiratable), timeToEnableAspirating);
        }
    }


    public void Attach(Transform pointToAttach, Vector3 closestPoint)
    {
        tag = "Untagged";
        transform.SetParent(pointToAttach);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        distanceBetweenParentAndObject = Vector3.Distance(transform.position, transform.parent.position);
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        _isAttached = true;
    }
    public void Detach()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");

        tag = "IsWall";
        _rb.constraints = RigidbodyConstraints.None;
        _rb.useGravity = true;

        transform.SetParent(null);
        _isAttached = false;
    }

    private void ResetAspiratable() => _hasBeenShoot = false;
}
