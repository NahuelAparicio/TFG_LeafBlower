using UnityEngine;

public class BlowerForce : BaseLeafBlower
{
    protected override void HandleBlow()
    {
        base.HandleBlow();

        if (_closestObject == null) return;

        MovableObject movableObject = _closestObject.GetComponent<MovableObject>();
        ShootableObject shootableObject = _closestObject.GetComponent<ShootableObject>();

        if( movableObject == null && shootableObject == null ) return;

        if(movableObject)
        {
            movableObject.OnBlowableInteracts(GetBlowForceDir(movableObject), movableObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        }


    }
}
