using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    public int neededWeight;

    public IActivable _activable;
    private int _currentWeight;

    private bool _isActive;

    public GameObject pressObject;
    private Vector3 pressedPos;

    private void Awake()
    {
        _isActive = false;
        pressedPos = new Vector3(0, -0.136f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if(obj)
        {
            if(obj.weight != Enums.ObjectWeight.Leaf)
                UpdateWeight((int)obj.weight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (obj)
        {
            if (obj.weight != Enums.ObjectWeight.Leaf)
                UpdateWeight(-(int)obj.weight);
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
