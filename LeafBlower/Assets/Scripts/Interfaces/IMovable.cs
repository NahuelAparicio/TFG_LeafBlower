using UnityEngine;

public interface IMovable
{
    void OnAspire(Vector3 force);
    void OnBlow(Vector3 force, Vector3 point);
}
