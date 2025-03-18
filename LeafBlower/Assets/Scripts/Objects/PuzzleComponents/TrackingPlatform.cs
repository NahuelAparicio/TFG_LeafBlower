using System.Collections.Generic;
using UnityEngine;

public class TrackingPlatform : MonoBehaviour
{
    private Dictionary<Transform, Vector3> _trackedObjects = new Dictionary<Transform, Vector3>();

    private bool _isPlayerInPlatform = false;

    public bool IsPlayerInPlatform => _isPlayerInPlatform;

    private void FixedUpdate()
    {
        foreach (var obj in _trackedObjects)
        {
            obj.Key.position = transform.position + obj.Value;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;

        if (other.tag != "Player")
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            if(other.GetComponent<ShootableObject>() != null)
            {
                other.GetComponent<ShootableObject>().FreezeConstraints();
            }
            StartTrackingObject(other.transform);
        }
        else
        {
            _isPlayerInPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;

        if (other.tag != "Player")
        {
            if(!other.GetComponent<ShootableObject>().IsAttached)
            {
                other.GetComponent<Rigidbody>().useGravity = true;
            }
            StopTrackingObject(other.transform);
        }
        else
        {
            _isPlayerInPlatform = false;
        }
    }

    private void StartTrackingObject(Transform objTransform)
    {
        if (!_trackedObjects.ContainsKey(objTransform))
        {
            Vector3 initialOffset = objTransform.position - transform.position;
            _trackedObjects.Add(objTransform, initialOffset);
        }
    }

    private void StopTrackingObject(Transform objTransform)
    {
        if (_trackedObjects.ContainsKey(objTransform))
        {
            _trackedObjects.Remove(objTransform);
        }
    }
}
