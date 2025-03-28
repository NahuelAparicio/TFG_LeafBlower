using System.Collections.Generic;
using UnityEngine;

public class BaseLeafBlower : MonoBehaviour
{
    protected BlowerController _blower;

    protected HashSet<Object> _objects = new HashSet<Object>();

    protected Object _closestObject;

    public Object ClosestObject => _closestObject;

    protected virtual void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
    }


    private void PlayerEvents_onDetachObject()
    {
        CheckForNulls();
    }

    protected virtual void Start()
    {
        GameEventManager.Instance.playerEvents.onDetachObject += PlayerEvents_onDetachObject;

    }
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();

        if (obj == null) return;

        _objects.Add(obj);
    }

    protected virtual void OnTriggerStay(Collider other)
    {        
        UpdateClosestMovable();
        //if (!_blower.Inputs.IsBlowingInputPressed() && !_blower.Inputs.IsAspiringInputPressed()) return;

        if(_blower.IsBlowing() && !_blower.Aspirer.attachableObject.IsAttached)
        {
            HandleBlow();
        }

        if (_blower.IsAspirating())
        {
            if(_closestObject != null)
                HandleAspire(_closestObject);
        }
        else
        {
            if(!_blower.IsShooting())
            {
                if (_closestObject != null)
                    HandleAspire(_closestObject);
            }
        }

        foreach (var obj in _objects)
        {
            if (!obj.IsLeaf()) continue;

            if(_blower.IsAspirating())
            {
                HandleAspire(obj);
            }
            if (_blower.IsBlowing() && !_blower.Aspirer.attachableObject.IsAttached)
            {
                BlowLeaf(obj.GetComponent<MovableObject>());
            }
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();

        if (obj == null) return;

        _objects.Remove(obj);
    }

    public void UpdateClosestMovable()
    {
        if (_objects.Count == 0)
        {
            if (_closestObject != null)
            {
                _closestObject.DisableOutline(); 
                _closestObject = null;
            }
            return;
        }

        float minDistance = float.MaxValue;
        Object newClosest = null;
        float threshold = 0.2f;

        foreach (var obj in _objects)
        {
            float distance = Vector3.Distance(_blower.FirePoint.position, obj.transform.position);
            if (distance < minDistance && obj.CanBeMoved(_blower.Player.Stats.Level) && !obj.IsLeaf() && obj.gameObject != _blower.Player.Inventory.objectSaved)
            {
                minDistance = distance;
                newClosest = obj;
            }
        }

        if (_closestObject != null && newClosest != null)
        {
            float prevDistance = Vector3.Distance(_blower.FirePoint.position, _closestObject.transform.position);
            if (Mathf.Abs(prevDistance - minDistance) < threshold)
            {
                return; 
            }

            _closestObject.DisableOutline();
        }

        _closestObject = newClosest;

        if(newClosest != null)
            _closestObject.EnableOutline();

    }

    protected virtual void BlowLeaf(MovableObject obj)
    {
        obj.OnBlowableInteracts(GetBlowForceDir(obj), obj.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    }

    protected virtual void HandleBlow() { }

    protected virtual void HandleAspire(Object obj) 
    {
        if (!obj.CanBeMoved(_blower.Player.Stats.Level)) return;
    }
    protected virtual void HandleNotAspirating(Collider other, ShootableObject shooteable) { }

    protected Vector3 GetBlowForceDir(Object obj) => _blower.FirePoint.forward * CalculateForceByDistance(obj.gameObject);
    protected float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(_blower.FirePoint.position, go.transform.position);
        return _blower.Stats.BlowForce / Mathf.Max(1, distance);
    }

    public void CheckForNulls()
    {
        _objects.RemoveWhere(obj => obj == null || !obj.isActiveAndEnabled);
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.playerEvents.onDetachObject -= PlayerEvents_onDetachObject;

    }

}
