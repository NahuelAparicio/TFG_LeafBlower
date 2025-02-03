using UnityEngine;

public class MovableObject : Object, IBlowable, IAspirable
{
    [SerializeField] private Enums.BlowType _type;

    public Enums.BlowType Type => _type;

    protected override void Awake()
    {
        base.Awake();

        if(_type == Enums.BlowType.DirectionalBlow)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.isKinematic = true;
        }
    }

    public void OnBlowableInteracts(Vector3 force, Vector3 point)
    {
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                if(weight == Enums.ObjectWeight.Leaf)
                {
                    force.y += Mathf.Abs(force.magnitude) * 0.5f; // Scale this to control how much upward force to add
                }
                _rb.AddForceAtPosition(force, point); // Applies force in the nearest point between the object and the blower (More Realistic)
                break;
            case Enums.BlowType.DirectionalBlow:
                force.y = 0;
                _rb.AddForce(force, ForceMode.Impulse); //Applies force in the center of the object to the direcion between shootPoint and object
                break;
            default:
                break;
        }
    }

    public void OnAspiratableInteracts(Vector3 force)
    {
        if (weight == Enums.ObjectWeight.Leaf) force /= 2;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    public void SetKinematic(bool active)
    {
        if (_type != Enums.BlowType.DirectionalBlow) return;

        _rb.isKinematic = active; 
    }
    public override bool CanBeMoved(int level) => (int)weight <= level + 1;
}
