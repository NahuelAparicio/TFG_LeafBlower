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
        switch (weight)
        {
            case Enums.ObjectWeight.Leaf:
                _rb.mass = 0.25f;
                break;
            case Enums.ObjectWeight.Low:
                _rb.mass = (int)Enums.ObjectWeight.Low;
                break;
            case Enums.ObjectWeight.Medium:
                _rb.mass = (int)Enums.ObjectWeight.Medium;
                break;
            case Enums.ObjectWeight.Heavy:
                _rb.mass = (int)Enums.ObjectWeight.Heavy;
                break;
            case Enums.ObjectWeight.SuperHeavy:
                _rb.mass = (int)Enums.ObjectWeight.SuperHeavy;
                break;
            default:
                break;
        }
    }
}
