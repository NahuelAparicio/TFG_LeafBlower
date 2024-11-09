using UnityEngine;

public class AspirableObject : MonoBehaviour, IAspirable
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnAspiratableInteracts(Vector3 force)
    {
        _rb.AddForce(force, ForceMode.Impulse);
    }
}
