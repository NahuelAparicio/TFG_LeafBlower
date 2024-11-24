using UnityEngine;

public class Object : MonoBehaviour
{
    internal Rigidbody _rb;
    internal Outline _outline;
    public Enums.ObjectWeight weight;

    internal virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
        SetRibidBodyWeight();
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

    internal void ChangeWeight(Enums.ObjectWeight objectWeight)
    {
        if (weight == objectWeight) return;
        weight = objectWeight;
        SetRibidBodyWeight();
    }

    private void SetRibidBodyWeight()
    {
        switch (weight)
        {
            case Enums.ObjectWeight.None:
                _rb.mass = 0.1f;
                break;
            case Enums.ObjectWeight.Leaf:
                _rb.mass = 0.5f;
                break;
            case Enums.ObjectWeight.Low:
                _rb.mass = 2;
                break;
            case Enums.ObjectWeight.Medium:
                _rb.mass = 3;
                break;
            case Enums.ObjectWeight.Heavy:
                _rb.mass = 4;
                break;
            case Enums.ObjectWeight.SuperHeavy:
                _rb.mass = 5;
                break;
            default:
                break;
        }
    }
}
