using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODTriggerRadius : MonoBehaviour
{
    [SerializeField] private string fmodEventPath = "event:/Ambient/Window";
    [SerializeField] private float triggerRadius = 10f;
    [SerializeField] private Transform player;

    private EventInstance eventInstance;
    private bool isPlaying = false;

    private void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform, GetComponent<Rigidbody>());
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerRadius && !isPlaying)
        {
            eventInstance.start();
            isPlaying = true;
        }
        else if (distance > triggerRadius && isPlaying)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }

    private void OnDestroy()
    {
        eventInstance.release();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
