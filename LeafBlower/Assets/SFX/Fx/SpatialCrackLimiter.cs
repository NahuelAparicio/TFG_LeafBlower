using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class SpatialCrackLimiter: MonoBehaviour
{
    [Header("FMOD Events")]
    [SerializeField] private EventReference crackEvent;
    [SerializeField] private EventReference pieceEvent;
    [SerializeField] private EventReference ambientEvent;

    [Header("Settings")]
    [SerializeField] private float impactThreshold = 5f;
    [SerializeField] private float minTimeBetweenSounds = 0.1f;
    [SerializeField] private int maxSimultaneousSounds = 5;
    [SerializeField] private float ambientRadius = 10f;

    private static List<EventInstance> activeSoundInstances = new List<EventInstance>();
    private static float lastSoundPlayTime = -1f;
    private bool crackPlayed = false;

    private EventInstance ambientInstance;
    private bool ambientIsPlaying = false;

    private void Start()
    {
        if (!ambientEvent.IsNull)
        {
            ambientInstance = RuntimeManager.CreateInstance(ambientEvent);
            RuntimeManager.AttachInstanceToGameObject(ambientInstance, transform, GetComponent<Rigidbody>());
            ambientInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            ambientInstance.start();
            ambientIsPlaying = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < impactThreshold) return;

        for (int i = activeSoundInstances.Count - 1; i >= 0; i--)
        {
            activeSoundInstances[i].getPlaybackState(out var state);
            if (state == PLAYBACK_STATE.STOPPED)
            {
                activeSoundInstances[i].release();
                activeSoundInstances.RemoveAt(i);
            }
        }

        if (activeSoundInstances.Count >= maxSimultaneousSounds) return;
        if (Time.time - lastSoundPlayTime < minTimeBetweenSounds) return;

        if (!pieceEvent.IsNull)
        {
            EventInstance piece = RuntimeManager.CreateInstance(pieceEvent);
            piece.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            piece.start();
            piece.release();
            activeSoundInstances.Add(piece);
        }

        if (!crackPlayed && !crackEvent.IsNull)
        {
            crackPlayed = true;

            if (ambientIsPlaying)
            {
                ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                ambientInstance.release();
                ambientIsPlaying = false;
            }

            EventInstance crack = RuntimeManager.CreateInstance(crackEvent);
            crack.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            crack.start();
            crack.release();
            activeSoundInstances.Add(crack);
        }

        lastSoundPlayTime = Time.time;
    }
}
