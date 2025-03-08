using UnityEngine;

public class ShootableObject : Object, IAspirable, IAttacheable
{
    private bool _isAttached;
    public bool IsAttached => _isAttached;

    public float timeToEnableAspirating = 0.5f;
    private bool _hasBeenShoot = false;
    public bool HasBeenShoot => _hasBeenShoot;
    private float distanceBetweenParentAndObject;

    public Quaternion currentRotation;
    private float _currentTime = 0f;
    private float _timerToFreeze = 0f;
    public float timeToFreeze = 0.15f;

    private float _timerSave = 0f;
    public float timeToRestoreSave = 1f;

    private float _timerToReAttach = 0f;
    public float timeToRestoreAttach = 1f;

    public bool canBeAttached = true;

    public bool canBeSaved = true;

    public bool isInTunel = false;

    protected override void Awake()
    {
        base.Awake();
        _isAttached = false;
        _hasBeenShoot = false;
        FreezeConstraints();
    }
    protected override void Update()
    {
        base.Update();

        if(!canBeSaved)
        {
            _timerSave += Time.deltaTime;
            if(_timerSave >= timeToRestoreSave)
            {
                canBeSaved = true;
                _timerSave = 0f;
            }
        }

        if(!canBeAttached) 
        { 
            _timerToReAttach += Time.deltaTime;
            if(_timerToReAttach >= timeToRestoreAttach)
            {
                canBeAttached = true;
                _timerToReAttach = 0f;
            }
        }


        if(_hasBeenShoot)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= timeToEnableAspirating)
            {
                _hasBeenShoot = false;
                _currentTime = 0f;
            }
        }

        if (!_isAttached && !_hasBeenShoot && _rb.velocity.magnitude < 0.05f && _rb.angularVelocity.magnitude < 0.01f && _timerToFreeze >= timeToFreeze)
        {
            if (_isFreezed) return;
            FreezeConstraints();
        }
        else
        {
            _timerToFreeze += Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if (_isAttached)
        {
            Vector3 targetPosition = transform.parent.position + (transform.parent.forward * distanceBetweenParentAndObject);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 25f);

            transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 25f);
        }
    }

 

    public void OnAspiratableInteracts(Vector3 force)
    {
        if(!_isAttached && !_hasBeenShoot)
        {
            _timerToFreeze = 0;
            UnFreeze();
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

    public void OnShoot(Vector3 force)
    {
        if(_isAttached)
        {
            Detach();
            _hasBeenShoot = true;
            UnFreeze();
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }
    public void Attach(Transform pointToAttach, Vector3 closestPoint)
    {
        if (!canBeAttached) return;
        _currentTime = 0;
        transform.rotation = _originalRotation;
        transform.SetParent(pointToAttach);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        distanceBetweenParentAndObject = Vector3.Distance(transform.position, transform.parent.position);
        _rb.useGravity = false;
        _isAttached = true;
    }
    public void Detach()
    {
        canBeAttached = false;
        _timerToFreeze = 0;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        _rb.useGravity = true;
        UnFreeze();
        _rb.AddForce(Vector3.down * 0.5f, ForceMode.Impulse);
        transform.SetParent(null);
        _isAttached = false;
    }

    public override bool CanBeMoved(int level) => (int)weight <= level;

    private void OnEnable()
    {
        _isAttached = false;
        _hasBeenShoot = false;
        _timerToFreeze = 0;
        UnFreeze();
    }

}
