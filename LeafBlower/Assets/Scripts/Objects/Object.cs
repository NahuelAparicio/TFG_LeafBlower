using UnityEngine;

public class Object : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Outline _outline;
    public Sprite uiImage;
    public Enums.ObjectWeight weight;

    protected Quaternion _originalRotation;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
        SetRibidBodyWeight();
        _originalRotation = transform.rotation;
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
}
