using System.Collections.Generic;
using UnityEngine;

// --   -- //
[RequireComponent(typeof(Collider))]
public class ObjectAttacher : MonoBehaviour
{
    public bool isObjectAttached;
    public int maxAttachedObjects = 1;
    private List<IAttacheable> _attachedObjects = new List<IAttacheable>();

    private void Awake()
    {
        isObjectAttached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_attachedObjects.Count > maxAttachedObjects)
        {
            return;
        }

        IAttacheable attacheable = other.GetComponent<IAttacheable>();

        if(attacheable != null && isObjectAttached)
        {
            other.gameObject.transform.SetParent(transform);

            Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            attacheable.Attach(transform, contactPoint);

            _attachedObjects.Add(attacheable);
            maxAttachedObjects++;
        }
    }

    public void DeattachObject()
    {
        isObjectAttached = false;
    }
}
