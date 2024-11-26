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
    private void Update()
    {
        if(_isAttached)
        {
            // Check the distance between the object's position and the attached point's position
            float distance = Vector3.Distance(transform.position, transform.parent.position);

            // If the distance is greater than 0.1f, update the object's position
            if (distance > 0.1f)
            {
                // Move the object towards the parent (attachment point) smoothly
                transform.position = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * 10f); // Adjust speed as needed
            }
        }
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


    public void Attach(Transform pointToAttach, Vector3 closestPoint)
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity = false;

        transform.position = closestPoint;

        transform.SetParent(pointToAttach);
        _isAttached = true;
    }
    public void Detach()
    {
        _isAttached = false;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
        transform.SetParent(null);
    }

    private void ResetAspiratable() => _hasBeenShoot = false;
}
