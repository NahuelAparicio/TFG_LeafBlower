using UnityEngine;

public abstract class MovementBehavior : ScriptableObject
{
    public abstract void ExecuteMovement(Rigidbody rb, Vector3 forceDir);
}
