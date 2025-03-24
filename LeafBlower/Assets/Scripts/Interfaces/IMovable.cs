using UnityEngine;

public interface IMovable
{
    bool IsCollectable();
    bool CanBeAspired();
    void StartAspiring(Transform target, Transform closestPoint);
    void StopAspiring();
    void OnBlow(Vector3 force, Vector3 point);
}
