using UnityEngine;

public interface IAttacheable  
{
    public void Attach(Transform pointToAttach, Vector3 positionToAttach);
    public void Detach();
}
