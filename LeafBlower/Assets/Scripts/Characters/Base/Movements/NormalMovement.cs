using UnityEngine;

public class NormalMovement : MovementBehavior
{
    public float drag;

    public override bool CanExecuteMovement() => true;    

    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        // PlaceHolder
    }

    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        SetRigidbodyDrag(rb);
    }

    public override void ResetMovement() { }

    protected virtual void SetRigidbodyDrag(Rigidbody rb)
    {
        if (rb.drag == drag) return;
        rb.drag = drag;
    }
}
