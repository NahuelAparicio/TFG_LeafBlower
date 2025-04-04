using System;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject vfxAspiration;


    private HashSet<IMovable> _aspiringObjects = new();
    private NormalObject _attachedObject;

    public bool IsObjectAttached => _attachedObject != null;
    public NormalObject ObjectAttached => _attachedObject;


    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerController>();
        vfxAspiration.SetActive(false);
    }

    private void Start()
    {
        GameEventManager.Instance.playerEvents.OnAttach += OnAttach;
        GameEventManager.Instance.playerEvents.OnDestroy += OnDestroyObject;
    }

    private void Update()
    {
        // -- TEMPORAL
        if(_player.Inputs.IsAspiring())
        {
            vfxAspiration.SetActive(true);
        }
        else
        {
            vfxAspiration.SetActive(false);
        }
    }

    private void OnDestroyObject(IMovable obj)
    {
        _aspiringObjects.Remove(obj);
    }

    private void OnAttach(NormalObject obj)
    {
        _player.Inputs.SetIsAspiring(false);

        obj.RigidBody.isKinematic = true;

        foreach (var aspired in _aspiringObjects)
        {
            if(!aspired.IsCollectable())
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

        if(IsObjectAttached)
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

        //Si object not attached
        if (!movable.CanBeAspired()) return;

        if (_player.Inputs.IsAspiring())
        {

            if (_aspiringObjects.Add(movable))
            {
                //movable.StartAspiring(_firePoint, other.ClosestPoint(_firePoint.position));
                movable.StartAspiring(_firePoint, _firePoint);
            }
        }
        else if(_aspiringObjects.Remove(movable))
        {

            if (!movable.IsCollectable())
            {
                movable.StopAspiring();
            }
        }

        if (_player.Inputs.IsBlowing())
        {
            movable.OnBlow(_firePoint.forward * _player.Stats.BlowerForce, other.ClosestPoint(_firePoint.position));
            //movable.OnBlow(_firePoint.forward * _player.Stats.BlowerForce.Value, other.transform.position);
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
