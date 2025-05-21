using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class VelocityTriggerSound : MonoBehaviour
{
    [SerializeField] private string fmodEventPath = "event:/Interactables/Ball/Ball_Celebration";
    [SerializeField] private float minimumVelocity = 5f;
    [SerializeField] private float cooldownTime = 5f; // Tiempo en segundos entre sonidos

    private bool canPlay = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canPlay) return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null && rb.velocity.magnitude >= minimumVelocity)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEventPath);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            eventInstance.start();
            eventInstance.release();

            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        canPlay = false;
        yield return new WaitForSeconds(cooldownTime);
        canPlay = true;
    }
}
