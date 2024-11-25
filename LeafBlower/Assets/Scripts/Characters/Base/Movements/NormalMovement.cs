using UnityEngine;

public class NormalMovement : MovementBehavior
{
    public float drag;
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        // PlaceHolder
    }

    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        SetRigidbodyDrag(rb);
    }

    protected virtual void SetRigidbodyDrag(Rigidbody rb)
    {
        if (rb.drag == drag) return;
        rb.drag = drag;
    }
}
