using UnityEngine;

public class BlowableObject : Object, IBlowable
{
    [SerializeField] private Enums.BlowType _type;

    protected override void Awake()
    {
        base.Awake();

        if(_type == Enums.BlowType.PuzzleBlow)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationZ;
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
            case Enums.BlowType.PuzzleBlow:
                break;
            case Enums.BlowType.DirectionalBlow:
                _rb.AddForce(force, ForceMode.Impulse); //Applies force in the center of the object to the direcion between shootPoint and object
                break;
            default:
                break;
        }
    }

}
