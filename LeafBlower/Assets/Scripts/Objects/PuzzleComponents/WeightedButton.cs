using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    public int neededWeight;

    public IActivable _activable;
    private int _currentWeight;

    private bool _isActive;

    public GameObject pressObject;
    private Vector3 pressedPos;

    private List<Object> objects = new List<Object>();

    private void Awake()
    {
        _isActive = false;
        pressedPos = new Vector3(0, -0.136f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Object>();
        if (obj == null) return;
        if (obj.weight != Enums.ObjectWeight.Leaf)
        {
            var shootableObject = obj.GetComponent<ShootableObject>();
            if (shootableObject != null)
            {
                if (!shootableObject.IsAttached)
                {
                    UpdateWeight((int)obj.weight);
                    if (!objects.Contains(obj))
                        objects.Add(obj);
                }
            }
            else
            {
                UpdateWeight((int)obj.weight);
                if (!objects.Contains(obj))
                    objects.Add(obj);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var obj = other.GetComponent<Object>();
        if (obj == null) return;
        if(!objects.Contains(obj))
        {
            if(obj.GetComponent<ShootableObject>())
            {
                var shootableObject = obj.GetComponent<ShootableObject>();
                if (!shootableObject.IsAttached)
                {
                    UpdateWeight((int)obj.weight);
                    if (!objects.Contains(obj))
                        objects.Add(obj);
                }
            }
            else
            {
                UpdateWeight((int)obj.weight);
                if (!objects.Contains(obj))
                    objects.Add(obj);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<Object>();
        if(obj == null) return;

        if (obj.weight != Enums.ObjectWeight.Leaf)
        {
            var shootableObject = obj.GetComponent<ShootableObject>();
            if (shootableObject != null)
            {
                if (!shootableObject.IsAttached)
                {
                    UpdateWeight(-(int)obj.weight);
                    if (objects.Contains(obj))
                        objects.Remove(obj);
                }
            }
            else
            {
                UpdateWeight(-(int)obj.weight);
                if (objects.Contains(obj))
                    objects.Remove(obj);
            }
        }
    }

    private void UpdateWeight(int weight)
    {
        _currentWeight += weight;

        if(_currentWeight >= neededWeight && !_isActive)
        {
            _activable.DoAction();
            pressObject.transform.localPosition = pressedPos;
            _isActive = true;
        }
        else if(_currentWeight < neededWeight && _isActive)
        {
            _activable.UndoAction();
            pressObject.transform.localPosition = Vector3.zero;
            _isActive = false;
        }
    }
}
