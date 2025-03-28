using UnityEngine;

public class MovableObject : MonoBehaviour, IMovable
{
    [SerializeField] private ItemData _data;
    [SerializeField] private Enums.BlowType _type;
    [SerializeField] private float _aspirationSpeed;
    [SerializeField] private float _localScaleSpeed;
    [SerializeField] private GameObject _objectToScale;
    [SerializeField] private float _timeToReEnableAspire = 0.5f;
    [SerializeField][Range(0,1)] private float percentageToScale;
    private bool _canBeAspired = true;
    public GameObject colliderObject;

    private ObjectTransparencyHandler _transparency;
    private Rigidbody _rb;
    private Transform _target;
    private bool _isBeingAspired;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private Vector3 _offsetToTarget;
    private float _currentTime = 0f;

    public Rigidbody RigidBody => _rb;
    public Enums.BlowType Type => _type;
    public ObjectTransparencyHandler Transparency => _transparency;

    private void Awake()
    {
        _transparency = GetComponent<ObjectTransparencyHandler>();
        _rb = GetComponent<Rigidbody>();
        _originalScale = _objectToScale.transform.localScale;
        _targetScale = _originalScale * percentageToScale;
    }

    private void Update()
    {
        if(!_canBeAspired)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= _timeToReEnableAspire)
            {
                _canBeAspired = true;
                _currentTime = 0f;
            }
            return;
        }

        if (!_isBeingAspired) return;

        Vector3 _targetPosition = _target.position + _offsetToTarget;

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _aspirationSpeed * Time.deltaTime);

        if(_data.GetItemType() == Enums.ObjectType.Colectionable)
        {
            _objectToScale.transform.localScale = Vector3.MoveTowards(_objectToScale.transform.localScale, _targetScale, _localScaleSpeed * Time.deltaTime);
        }

        if(Vector3.SqrMagnitude(transform.position - _targetPosition) < 0.0025f)
        {
            _isBeingAspired = false;
            _target = null;

            switch (_data.GetItemType())
            {
                case Enums.ObjectType.Colectionable:
                    GameEventManager.Instance.collectingEvents.InvokeCollectCollectionable(_data.GetCollectionableType(), _data.GetAmount());
                    GameEventManager.Instance.playerEvents.InvokeDestroy(this);
                    Destroy(gameObject);
                    break;
                case Enums.ObjectType.Leaf:
                    StopAspiring();
                    break;
                case Enums.ObjectType.Object:
                    GameEventManager.Instance.playerEvents.InvokeAttach(this);
                    break;
                default:
                    break;
            }
        }
    }

    public bool IsCollectable() => _data.GetItemType() == Enums.ObjectType.Colectionable;
    public bool CanBeAspired() => _canBeAspired;
    public float offsetDistance = 0.5f;
    public void StartAspiring(Transform target, Transform firePoint)
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = false;

        _target = target;

        _offsetToTarget = firePoint.forward * offsetDistance;

        _isBeingAspired = true;
    }

    public void StopAspiring()
    {
        _canBeAspired = false;
        _isBeingAspired = false;
        _target = null;
        _rb.useGravity = true;
    }

    public void OnBlow(Vector3 force, Vector3 point)
    {
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                if(_data.GetItemType() == Enums.ObjectType.Leaf)
                {
                    force.y += Mathf.Abs(force.magnitude) * 0.5f;
                }
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

    public void Shoot(Vector3 force)
    {
        _canBeAspired = false;
        _rb.useGravity = true;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    public void ChangeLayer(LayerMask layer)
    {
        gameObject.layer = layer;
        colliderObject.layer = layer;
    }

    

}
