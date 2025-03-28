using UnityEngine;
public class LeafObject : MovableObject
{
    public override void OnBlow(Vector3 force, Vector3 point)
    {
        force.y += Mathf.Abs(force.magnitude) * 0.5f;
        _rb.AddForceAtPosition(force, point);
    }
    protected override void OnArriveToAttacher()
    {
        StopAspiring();
    }
}
