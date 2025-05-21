using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class LimiterSoundTrigger : MonoBehaviour
{
    [SerializeField] private EventReference soundEvent;
    [SerializeField] private float minTimeBetweenSounds = 0.1f;
    [SerializeField] private int maxSimultaneousSounds = 5;

    private static List<EventInstance> activeInstances = new List<EventInstance>();
    private static float lastPlayTime = -1f;

    private void OnCollisionEnter(Collision collision)
    {
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

        EventInstance instance = RuntimeManager.CreateInstance(soundEvent);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        instance.start();
        instance.release(); // se libera después del start
        activeInstances.Add(instance);
        lastPlayTime = Time.time;
    }
}
