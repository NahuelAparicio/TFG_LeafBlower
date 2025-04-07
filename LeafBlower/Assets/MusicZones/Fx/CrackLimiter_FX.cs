using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class CrackSoundLimiterTrigger : MonoBehaviour
{
    [SerializeField] private EventReference crackEvent;
    [SerializeField] private EventReference pieceEvent;
    [SerializeField] private float impactThreshold = 5f;
    [SerializeField] private float minTimeBetweenSounds = 0.1f;
    [SerializeField] private int maxSimultaneousSounds = 5;

    private static List<EventInstance> activeInstances = new List<EventInstance>();
    private static float lastPlayTime = -1f;
    private bool hasPlayedCrack = false;

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < impactThreshold) return;

        // Limpia instancias que ya terminaron
        for (int i = activeInstances.Count - 1; i >= 0; i--)
        {
            activeInstances[i].getPlaybackState(out var state);
            if (state == PLAYBACK_STATE.STOPPED)
            {
                activeInstances[i].release();
                activeInstances.RemoveAt(i);
            }
        }

        if (activeInstances.Count >= maxSimultaneousSounds) return;
        if (Time.time - lastPlayTime < minTimeBetweenSounds) return;

        // Sonido de pedazos siempre
        if (!pieceEvent.IsNull)
        {
            EventInstance pieceInstance = RuntimeManager.CreateInstance(pieceEvent);
            pieceInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            pieceInstance.start();
            pieceInstance.release();
            activeInstances.Add(pieceInstance);
        }

        // Sonido de grieta solo la primera vez
        if (!hasPlayedCrack && !crackEvent.IsNull)
        {
            hasPlayedCrack = true;
            EventInstance crackInstance = RuntimeManager.CreateInstance(crackEvent);
            crackInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            crackInstance.start();
            crackInstance.release();
            activeInstances.Add(crackInstance);
        }

        lastPlayTime = Time.time;
    }
}
