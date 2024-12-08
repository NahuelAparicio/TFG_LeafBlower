using UnityEngine;

public class WSPlatform : MonoBehaviour
{
    private int _currentWeight;

    public int CurrentWeight => _currentWeight;

    private void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (obj)
        {
            if (obj.weight != Enums.ObjectWeight.Leaf)
            {
                UpdateWeight((int)obj.weight);
            //    other.transform.SetParent(gameObject.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (obj)
        {
            if (obj.weight != Enums.ObjectWeight.Leaf)
            {
                UpdateWeight(-(int)obj.weight);
            //    other.transform.SetParent(null);
            }
        }
    }

    private void UpdateWeight(int weight)
    {
        _currentWeight += weight;
    }
}
