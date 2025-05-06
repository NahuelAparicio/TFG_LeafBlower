using UnityEngine;

public class MovableObject : MonoBehaviour, IMovable
{
    [SerializeField] protected ItemData _data;
    [SerializeField] protected Enums.BlowType _type;
    [SerializeField] protected float _aspirationSpeed;

    protected bool _canBeAspired = true;
    protected Rigidbody _rb;
    protected Transform _target;
    protected bool _isBeingAspired;

    private Vector3 _offsetToTarget;
    public float offsetDistance = 0.5f;


    public Rigidbody RigidBody => _rb;
    public Enums.BlowType Type => _type;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        RenableAspiring();

        if (!_isBeingAspired) return;

        Vector3 _targetPosition = _target.position + _offsetToTarget;

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _aspirationSpeed * Time.deltaTime);

        UpdateCustom();

        if(Vector3.SqrMagnitude(transform.position - _targetPosition) < 0.0025f)
        {
            _isBeingAspired = false;
            _target = null;

            OnArriveToAttacher();
        }
    }
    protected virtual void RenableAspiring() { }
    protected virtual void OnArriveToAttacher() {}
    protected virtual void UpdateCustom() {}

    public bool IsCollectable() => _data.GetItemType() == Enums.ObjectType.Colectionable;
    public bool CanBeAspired() => _canBeAspired;

    public virtual void StartAspiring(Transform target, Transform firePoint)
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;
        _target = target;
        _offsetToTarget = firePoint.forward * offsetDistance;
        _isBeingAspired = true;
    }

    public virtual void StopAspiring()
    {
        _canBeAspired = false;
        _isBeingAspired = false;
        _target = null;
        if(_rb == null)
        {
            if(_rb.GetComponent<Rigidbody>())
            {
                _rb.GetComponent<Rigidbody>();
            }
            else
            {
                _rb = gameObject.AddComponent<Rigidbody>();

            }
        }
        _rb.useGravity = true;
    }

    public virtual void OnBlow(Vector3 force, Vector3 point)
    {
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                _rb.AddForceAtPosition(force, point); 
                break;

            case Enums.BlowType.DirectionalBlow:
                force.y = 0;
                _rb.AddForce(force, ForceMode.Force); 
                break;

            default:
                break;
        }
    }    

}
