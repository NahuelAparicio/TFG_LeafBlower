using UnityEngine;
using FMODUnity;

public class VelocityTriggerSound : MonoBehaviour
{
    [SerializeField] private string fmodEventPath = "event:/Interactables/Ball/Ball_Celebration";
    [SerializeField] private float minimumVelocity = 5f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null && rb.velocity.magnitude >= minimumVelocity)
        {
            RuntimeManager.PlayOneShot(fmodEventPath, transform.position);
        }
    }
}
