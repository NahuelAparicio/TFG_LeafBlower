using UnityEngine;
public class NormalObject : MovableObject
{
    public GameObject colliderObject;
    private Quaternion _originalRotation;
    private ObjectTransparencyHandler _transparency;
    public ObjectTransparencyHandler Transparency => _transparency;
    private float _currentTime = 0f;
    [SerializeField] protected float _timeToReEnableAspire = 0.5f;
    protected bool _hasBeenShoot;
    public bool HasBeenShoot => _hasBeenShoot;

    protected override void Awake()
    {
        base.Awake();
        _transparency = GetComponent<ObjectTransparencyHandler>();

        _originalRotation = transform.rotation;
    }

    protected override void OnArriveToAttacher()
    {
        GameEventManager.Instance.playerEvents.InvokeAttach(this);
    }
    public virtual void Shoot(Vector3 force)
    {
        _canBeAspired = false;
        _rb.useGravity = true;
        _rb.AddForce(force, ForceMode.Impulse);
        _hasBeenShoot = true;
    }
    public override void StartAspiring(Transform target, Transform firePoint)
    {
        base.StartAspiring(target, firePoint);
        ChangeLayer(12);
    }

    public override void StopAspiring()
    {
        base.StopAspiring();
        ChangeLayer(7);
    }

    public void ChangeLayer(LayerMask layer)
    {
        gameObject.layer = layer;
        colliderObject.layer = layer;
    }
    protected override void RenableAspiring()
    {
        if (!_canBeAspired)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeToReEnableAspire)
            {
                _canBeAspired = true;
                _currentTime = 0f;
            }
            return;
        }
    }
    public void ResetObjectRotation()
    {
        if (transform.localRotation == _originalRotation) return;
        transform.localRotation = Quaternion.identity;
    }
}
