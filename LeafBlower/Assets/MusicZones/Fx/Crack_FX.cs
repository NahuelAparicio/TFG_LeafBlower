using UnityEngine;
using FMODUnity;

public class CrackSoundTrigger : MonoBehaviour
{
    [SerializeField] private float impactThreshold = 5f;

    [SerializeField] private EventReference crackEvent;
    [SerializeField] private EventReference hitEvent;

    private bool hasSprayed = false;

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce >= impactThreshold && !hasSprayed)
        {
            if (crackEvent.IsNull == false)
                RuntimeManager.PlayOneShot(crackEvent, transform.position);

            hasSprayed = true;
        }
        else
        {
            if (hitEvent.IsNull == false)
                RuntimeManager.PlayOneShot(hitEvent, transform.position);
        }
    }
}
