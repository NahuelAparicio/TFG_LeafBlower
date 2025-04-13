using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

public class ActionerObject : NormalObject
{
    private bool _hasBeenActived = false;
    public UnityEvent OnAction;

    [Header("FMOD")]
    public EventReference fmodEvent;

    protected override void Awake()
    {
        base.Awake();
        _canBeAspired = false;
    }

    public override void OnBlow(Vector3 force, Vector3 point)
    {
        if (!_hasBeenActived)
        {
            PlayFmodEvent();
            OnAction?.Invoke();
            _hasBeenActived = true;
        }
    }

    private void PlayFmodEvent()
    {
        if (fmodEvent.IsNull)
        {
            Debug.LogWarning("FMOD event no asignado en " + gameObject.name);
            return;
        }

        var instance = RuntimeManager.CreateInstance(fmodEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, transform, GetComponent<Rigidbody>());
        instance.start();
        instance.release();
    }
}


