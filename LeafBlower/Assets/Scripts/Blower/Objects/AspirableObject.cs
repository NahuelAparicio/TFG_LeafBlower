using UnityEngine;

public class AspirableObject : MonoBehaviour, IAspirable
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnAspiratableInteracts(float force, Vector3 direction)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
