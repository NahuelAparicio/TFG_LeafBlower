using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class LeafBlower : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject vfxAspiration;

    private HashSet<IMovable> _aspiringObjects = new();
    private NormalObject _attachedObject;

    private bool _wasAspiring = false;
    private bool _wasBlowing = false;

    private EventInstance _aspirationSound;
    private EventInstance _blowSound;

    public bool IsObjectAttached => _attachedObject != null;
    public NormalObject ObjectAttached => _attachedObject;

    private void Awake()
    {
        _player = transform.parent.parent.parent.GetComponent<PlayerController>();
        vfxAspiration.SetActive(false);
    }

    private void Start()
    {
        GameEventManager.Instance.playerEvents.OnAttach += OnAttach;
        GameEventManager.Instance.playerEvents.OnDestroy += OnDestroyObject;

        // Crear la instancia de aspirar
        _aspirationSound = RuntimeManager.CreateInstance("event:/Tools/Aspiring");
        RuntimeManager.AttachInstanceToGameObject(_aspirationSound, transform, GetComponent<Rigidbody>());

        // Crear la instancia de soplar
        _blowSound = RuntimeManager.CreateInstance("event:/Tools/Blowing");
        RuntimeManager.AttachInstanceToGameObject(_blowSound, transform, GetComponent<Rigidbody>());
    }

    private void Update()
    {
        // ---- ASPIRAR ----
        bool isAspiring = _player.Inputs.IsAspiring();

        if (isAspiring && !_wasAspiring)
        {
            vfxAspiration.SetActive(true);
            _aspirationSound.start();
            StartCoroutine(UpdateRPM(_aspirationSound, 0f, 2000f, 0.5f));
        }
        else if (!isAspiring && _wasAspiring)
        {
            vfxAspiration.SetActive(false);
            _aspirationSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        _wasAspiring = isAspiring;

        // ---- SOPLAR ----
        bool isBlowing = _player.Inputs.IsBlowing();

        if (isBlowing && !_wasBlowing)
        {
            _blowSound.start();
            StartCoroutine(UpdateRPM(_blowSound, 0f, 2000f, 0.5f));
        }
        else if (!isBlowing && _wasBlowing)
        {
            _blowSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        _wasBlowing = isBlowing;
    }

    private IEnumerator UpdateRPM(EventInstance instance, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float rpm = Mathf.Lerp(start, end, elapsed / duration);
            instance.setParameterByName("RPM", rpm);
            yield return null;
        }
        instance.setParameterByName("RPM", end);
    }

    private void OnDestroy()
    {
        // Liberar ambas instancias FMOD correctamente
        _aspirationSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _aspirationSound.release();

        _blowSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _blowSound.release();
    }

    private void OnDestroyObject(IMovable obj)
    {
        _aspiringObjects.Remove(obj);
    }

    private void OnAttach(NormalObject obj)
    {
        RuntimeManager.PlayOneShot("event:/Tools/Attach", transform.position);

        _player.Inputs.SetIsAspiring(false);

        obj.RigidBody.isKinematic = true;

        foreach (var aspired in _aspiringObjects)
        {
            if (!aspired.IsCollectable())
            {
                aspired.StopAspiring();
            }
        }

        _aspiringObjects.Clear();

        obj.transform.SetParent(transform);
        _attachedObject = obj;
        _attachedObject.ChangeLayer(12);
        _attachedObject.Transparency.EnableTransparency();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out IMovable movable)) return;

        if (IsObjectAttached)
        {
            if (_player.Inputs.IsBlowing())
            {
                BlowAttachedObject();
            }
            else if (_player.Inputs.IsAspirePressed())
            {
                DetachObject();
            }
            return;
        }

        if (!movable.CanBeAspired()) return;

        if (_player.Inputs.IsAspiring())
        {
            if (_aspiringObjects.Add(movable))
            {
                movable.StartAspiring(_firePoint, _firePoint);
            }
        }
        else if (_aspiringObjects.Remove(movable))
        {
            if (!movable.IsCollectable())
            {
                movable.StopAspiring();
            }
        }

        if (_player.Inputs.IsBlowing())
        {
            movable.OnBlow(_firePoint.forward * _player.Stats.BlowerForce, other.ClosestPoint(_firePoint.position));
        }
    }

    private void DetachObject()
    {
        Detach();
        _attachedObject?.StopAspiring();
        _attachedObject = null;
    }

    private void BlowAttachedObject()
    {
        Detach();
        _attachedObject?.Shoot(_player.MainCamera.transform.forward * _player.Stats.ShootForce);
        _attachedObject = null;
    }

    private void Detach()
    {
        RuntimeManager.PlayOneShot("event:/Tools/UnAttach", transform.position);
        _attachedObject.ChangeLayer(7);
        _attachedObject.RigidBody.isKinematic = false;
        _attachedObject.transform.SetParent(null);
        _attachedObject.Transparency.DisableTransparency();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IMovable movable) && _aspiringObjects.Remove(movable))
        {
            movable.StopAspiring();
        }
    }
}
