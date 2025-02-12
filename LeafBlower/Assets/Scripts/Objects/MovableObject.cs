using UnityEngine;

public class MovableObject : Object, IBlowable, IAspirable
{
    [SerializeField] private Enums.BlowType _type;
    public Enums.BlowType Type => _type;

    private float _currentTime = 0f;
    public float timeToEnableFreeze = 0.1f;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (_isFreezed) return;
        _currentTime += Time.deltaTime;
        if (_rb.velocity.magnitude < 0.05f && _rb.angularVelocity.magnitude < 0.01f && _type == Enums.BlowType.DirectionalBlow && _currentTime >= timeToEnableFreeze)
        {
            FreezeConstraints();
        }
    }

    public void OnBlowableInteracts(Vector3 force, Vector3 point)
    {
        UnFreeze();
        _currentTime = 0;
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                if(weight == Enums.ObjectWeight.Leaf)
                {
                    force.y += Mathf.Abs(force.magnitude) * 0.35f; 
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
        _currentTime = 0;
        if (weight == Enums.ObjectWeight.Leaf) force /= 2;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    public override bool CanBeMoved(int level) => (int)weight <= level + 1;
}
