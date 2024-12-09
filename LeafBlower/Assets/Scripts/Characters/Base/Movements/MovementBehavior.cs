using UnityEngine;

public abstract class MovementBehavior : ScriptableObject
{
    public abstract void ExecuteMovement(Rigidbody rb, float force);
    public abstract bool CanExecuteMovement();
    public abstract void ResetMovement();
    //Ground || Air movement
    public virtual void ExecuteMovement(Rigidbody rb, Vector3 forceDir) { }
}
