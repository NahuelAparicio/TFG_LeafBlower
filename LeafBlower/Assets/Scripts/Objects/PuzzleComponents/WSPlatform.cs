using System.Collections.Generic;
using UnityEngine;

public class WSPlatform : MonoBehaviour
{
    private int _currentWeight;

    public int CurrentWeight => _currentWeight;

    private void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();

        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;
        UpdateWeight((int)obj.weight);
    }

    private void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;
        UpdateWeight(-(int)obj.weight);
    }

    private void UpdateWeight(int weight)
    {
        _currentWeight += weight;
    }

}











