using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    public IActivable _activable;
    private bool _isActive;

    public GameObject pressObject;
    private Vector3 pressedPos;

    private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

    private void Awake()
    {
        _isActive = false;
        pressedPos = new Vector3(0, -0.136f, 0);
    }

    private void Update()
    {
        CheckForNulls();
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Object>();
        if (obj == null) return;
        if (obj.weight != Enums.ObjectWeight.Leaf)
        {
            _activeObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<Object>();
        if(obj == null) return;

        if (obj.weight != Enums.ObjectWeight.Leaf)
        {
            _activeObjects.Remove(other.gameObject);
        }
    }

    private void SetButtonBehavior()
    {
        if(_isActive && _activeObjects.Count <= 0)
        {
            
            _activable.UndoAction();
            pressObject.transform.localPosition = Vector3.zero;
            _isActive = false;
        }
        else if(!_isActive && _activeObjects.Count > 0)
        {
            _activable.DoAction();
            GameEventManager.Instance.triggerEvents.TriggerButton();
            pressObject.transform.localPosition = pressedPos;
            _isActive = true;
        }

    }

    private void CheckForNulls()
    {
        _activeObjects.RemoveWhere(obj => obj == null || !obj.activeInHierarchy);
        SetButtonBehavior();
    }

}
