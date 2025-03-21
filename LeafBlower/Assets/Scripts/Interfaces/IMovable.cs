using UnityEngine;

public interface IMovable
{
    void StartAspiring(Transform target, Vector3 closestPoint);
    void StopAspiring();
    void OnBlow(Vector3 force, Vector3 point);
}
