using System.Collections.Generic;
using UnityEngine;

public class ObjectInstancier : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _quantity;
    public readonly List<Rigidbody> rigidBodies = new List<Rigidbody>();

    public void InstanceObjects()
    {
        for (int i = 0; i < _quantity; i++)
        {
            GameObject go = Instantiate(_prefab, transform.position, Quaternion.identity);
            rigidBodies.Add(go.GetComponent<Rigidbody>());
        }
    }
    
}
