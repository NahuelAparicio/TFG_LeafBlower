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
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_rb.velocity.magnitude < 0.1f && _rb.angularVelocity.magnitude < 0.1f && _type == Enums.BlowType.DirectionalBlow)
        {
            FreezeConstraints();
        }
    }

    public void OnBlowableInteracts(Vector3 force, Vector3 point)
    {
        UnFreeze();
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                if(weight == Enums.ObjectWeight.Leaf)
                {
                    force.y += Mathf.Abs(force.magnitude) * 0.5f; // Scale this to control how much upward force to add
                }
                force /= 2;
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
        UnFreeze();
        if (weight == Enums.ObjectWeight.Leaf) force /= 2;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    public override bool CanBeMoved(int level) => (int)weight <= level + 1;
}
