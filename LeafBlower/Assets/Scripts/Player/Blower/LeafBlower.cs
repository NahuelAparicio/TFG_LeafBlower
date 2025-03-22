using System;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private Transform _firePoint;

    private HashSet<IMovable> _aspiringObjects = new();

    public MovableObject _attachedObject;

    public bool ObjectAttached => _attachedObject != null;


    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void Start()
    {
        GameEventManager.Instance.playerEvents.OnAttach += OnAttach;
        GameEventManager.Instance.playerEvents.OnDestroy += OnDestroyObject;
    }

    private void OnDestroyObject(IMovable obj)
    {
        _aspiringObjects.Remove(obj);
    }

    private void OnAttach(MovableObject obj)
    {

        _player.Inputs.SetIsAspiring(false);

        obj.RigidBody.isKinematic = true;

        foreach (var aspired in _aspiringObjects)
        {
            aspired.StopAspiring();
        }

        _aspiringObjects.Clear();

        obj.transform.SetParent(transform);
        _attachedObject = obj;
        _attachedObject.ChangeLayer(12);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out IMovable movable)) return;

        if(ObjectAttached)
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


        if (_player.Inputs.IsAspiring())
        {
            if (_aspiringObjects.Add(movable))
            {
                movable.StartAspiring(_firePoint, other.ClosestPoint(_firePoint.position));
            }
        }
        else if(_aspiringObjects.Remove(movable))
        {
            movable.StopAspiring();
        }

        if (_player.Inputs.IsBlowing())
        {
            movable.OnBlow(_firePoint.forward * _player.Stats.BlowerForce.Value, other.ClosestPoint(_firePoint.position));
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
        _attachedObject?.Shoot(_firePoint.forward * _player.Stats.BlowerForce.Value);
        _attachedObject = null;
    }

    private void Detach()
    {
        _attachedObject.ChangeLayer(7);
        _attachedObject.RigidBody.isKinematic = false;
        _attachedObject.transform.SetParent(null);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IMovable movable) && _aspiringObjects.Remove(movable))
        {
            movable.StopAspiring();
        }
    }
}
