using UnityEngine;

public class BlowableObject : MonoBehaviour, IBlowable
{
    private Rigidbody _rb;
    public void OnBlowableInteracts(Vector3 force, Vector3 point)
    {
        _rb.AddForceAtPosition(force, point); // More realistis
        //_rb.AddForce(force, ForceMode.Impulse); Just Push
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
