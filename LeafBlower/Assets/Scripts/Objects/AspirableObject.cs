using UnityEngine;

public class AspirableObject : Object, IAspirable
{
    public void OnAspiratableInteracts(Vector3 force)
    {
        _rb.AddForce(force, ForceMode.Impulse);
    }
}
