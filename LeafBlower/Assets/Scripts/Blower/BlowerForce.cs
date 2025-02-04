using UnityEngine;

public class BlowerForce : BaseLeafBlower
{
    protected override void HandleBlow()
    {
        base.HandleBlow();

        if (_closestObject == null) return;

        MovableObject movableObject = _closestObject.GetComponent<MovableObject>();

        if (movableObject == null) return;

        movableObject.SetKinematic(false);

        movableObject.OnBlowableInteracts(GetBlowForceDir(movableObject), movableObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    }
}
