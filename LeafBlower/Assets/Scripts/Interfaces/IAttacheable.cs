using UnityEngine;

public interface IAttacheable  
{
    public void Attach(Transform pointToAttach, Vector3 positionToAttach, bool isAttachedToObject);
    public void Detach();
}
