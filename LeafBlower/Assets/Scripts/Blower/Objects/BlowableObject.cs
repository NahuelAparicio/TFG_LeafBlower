using UnityEngine;

public class BlowableObject : MonoBehaviour, IBlowable
{
    private Rigidbody _rb;
    public void OnBlowableInteracts(float force, Vector3 direction)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
