using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODMultiEventPlayer : MonoBehaviour
{
    [Tooltip("Lista de hasta 10 eventos FMOD reproducibles desde este GameObject")]
    [SerializeField]
    private EventReference[] events = new EventReference[10];

    private EventInstance[] instances = new EventInstance[10];
    private bool[] initialized = new bool[10];

    /// <summary>
    /// Reproduce el evento en el índice indicado (0–9), si está asignado.
    /// </summary>
    public void PlayEvent(int index)
    {
        if (!IsValidIndex(index) || events[index].IsNull)
            return;

        if (!initialized[index])
        {
            instances[index] = RuntimeManager.CreateInstance(events[index]);
            RuntimeManager.AttachInstanceToGameObject(instances[index], transform, GetComponent<Rigidbody>());
            initialized[index] = true;
        }

        instances[index].start();
    }

    /// <summary>
    /// Detiene el evento en el índice indicado, si está en reproducción.
    /// </summary>
    public void StopEvent(int index, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
    {
        if (IsValidIndex(index) && initialized[index])
        {
            instances[index].stop(stopMode);
        }
    }

    /// <summary>
    /// Libera las instancias de FMOD al destruir el objeto.
    /// </summary>
    private void OnDestroy()
    {
        for (int i = 0; i < instances.Length; i++)
        {
            if (initialized[i])
            {
                instances[i].release();
            }
        }
    }

    /// <summary>
    /// Comprueba si el índice está dentro del rango válido (0–9).
    /// </summary>
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < events.Length;
    }
}
