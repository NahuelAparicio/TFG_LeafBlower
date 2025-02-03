using System.Collections.Generic;
using UnityEngine;

public class BaseLeafBlower : MonoBehaviour
{
    protected BlowerController _blower;

  //  protected List<MovableObject> _objects = new List<MovableObject>();

    protected HashSet<Object> _objects = new HashSet<Object>();

    protected Object _closestObject;

    protected virtual void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Object movableObject = other.GetComponent<Object>();

        if (movableObject == null) return;

        _objects.Add(movableObject);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        UpdateClosestMovable();        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Object movableObject = other.GetComponent<Object>();

        if (movableObject == null) return;      

        if (_objects.Remove(movableObject))
        {
            movableObject.GetComponent<MovableObject>()?.SetKinematic(true);
        }
    }

    protected void UpdateClosestMovable()
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
            if (distance < minDistance && obj.CanBeMoved(_blower.Player.Stats.Level) && !obj.IsLeaf())
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

}
