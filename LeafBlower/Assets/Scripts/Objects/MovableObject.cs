using UnityEngine;

public class MovableObject : MonoBehaviour, IMovable
{
    [SerializeField] private ItemData _data;

    private Rigidbody _rb;

    [SerializeField] private Enums.BlowType _type;
    public Enums.BlowType Type => _type;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void OnAspire(Vector3 force)
    {
        _rb.AddForce(force, ForceMode.Impulse);
    }

    public void OnBlow(Vector3 force, Vector3 point)
    {
        switch (_type)
        {
            case Enums.BlowType.RealisticBlow:
                //if (weight == Enums.ObjectWeight.Leaf)
                //{
                //    force.y += Mathf.Abs(force.magnitude) * 0.35f;

                //    //AQUI BRYAN
                //}
                force /= 2;
                _rb.AddForceAtPosition(force, point); 
                break;
            case Enums.BlowType.DirectionalBlow:
                force.y = 0;
                _rb.AddForce(force, ForceMode.Impulse); 
                break;
            default:
                break;
        }
    }


}
