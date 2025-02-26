using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    public Dictionary<GameObject, Vector3> objectsRestricted = new Dictionary<GameObject, Vector3>();

    public GameObject[] objects;

    private void Start()
    {
        foreach (GameObject obj in objects)
        {
            objectsRestricted.Add(obj, obj.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(objectsRestricted.ContainsKey(other.gameObject))
        {
            UpdateToNewPosition(other.gameObject);
        }
    }

    public Vector3 GetPosition(GameObject obj)
    {
        if(objectsRestricted.ContainsKey(obj))
        {
            return objectsRestricted[obj];
        }
        return Vector3.zero;
    }

    private void UpdateToNewPosition(GameObject obj)
    {
        if(objectsRestricted.ContainsKey(obj))
        {
            obj.transform.position = GetPosition(obj);
            ShootableObject shootable = obj.GetComponent<ShootableObject>();
            if(shootable)
            {
                if(shootable.IsAttached)
                {
                    GameEventManager.Instance.playerEvents.DetachObject();
                }
            }
            if(obj.GetComponent<Rigidbody>())
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}
