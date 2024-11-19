using UnityEngine;

public class BlowableObject : MonoBehaviour, IBlowable
{
    private Rigidbody _rb;
    [SerializeField] private Enums.BlowType _type;
    public void OnBlowableInteracts(Vector3 force, Vector3 point)
    {
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
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
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
