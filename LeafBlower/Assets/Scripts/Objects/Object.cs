using UnityEngine;

public class Object : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Outline _outline;
    public Sprite uiImage;
    public Enums.ObjectWeight weight;

    protected Quaternion _originalRotation;

    protected Vector3 _spawnPosition;

    protected bool _isFreezed = false;

    public Rigidbody Rigidbody => _rb;

    protected virtual void Awake()
    {
        _spawnPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
        SetRibidBodyWeight();
        _originalRotation = transform.rotation;
    }

    protected virtual void Update()
    {
        if(Time.frameCount % 70 == 0)
        {
            if(transform.position.y <=  -35)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;

                if (gameObject.transform.parent == null)
                {
                    transform.position = _spawnPosition;
                    return;
                }

                if(gameObject.transform.parent.name == "Player")
                {
                    gameObject.transform.parent.position = _spawnPosition;
                }
            }
        }
    }

    public virtual void EnableOutline() 
    {
        if(_outline != null)
            _outline.enabled = true;
    }

    public virtual void DisableOutline()
    {
        if (_outline != null)
            _outline.enabled = false;
    }

    protected void ChangeWeight(Enums.ObjectWeight objectWeight)
    {
        if (weight == objectWeight) return;
        weight = objectWeight;
        SetRibidBodyWeight();
    }

    private void SetRibidBodyWeight()
    {
        if (_rb == null) return;
        switch (weight)
        {
            case Enums.ObjectWeight.Leaf:
                _rb.mass = Constants.LEAF_WEIGHT;
                break;
            case Enums.ObjectWeight.Low:
                _rb.mass = Constants.LOW_WEIGHT;
                break;
            case Enums.ObjectWeight.Medium:
                _rb.mass = Constants.MEDIUM_WEIGHT;
                break;
            case Enums.ObjectWeight.Heavy:
                _rb.mass = Constants.HEAVY_WEIGHT;
                break;
            case Enums.ObjectWeight.SuperHeavy:
                _rb.mass = Constants.SUPERHEAVY_WEIGHT;
                break;
            case Enums.ObjectWeight.MegaHeavy:
                _rb.mass = Constants.MEGAHEAVY_WEIGHT;
                break;
            default:
                break;
        }
    }

    public virtual bool CanBeMoved(int level) => true;

    public virtual bool IsLeaf() => weight == Enums.ObjectWeight.Leaf;

    public virtual void FreezeConstraints()
    {
        _isFreezed = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void FreezeRotation()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public virtual void UnFreeze()
    {
        if (!_isFreezed) return;
        _isFreezed = false;
        _rb.constraints = RigidbodyConstraints.None;
    }
}
