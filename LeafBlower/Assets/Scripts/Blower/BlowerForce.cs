using UnityEngine;

public class BlowerForce : BaseLeafBlower
{

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (!_blower.IsBlowing() || _blower.Aspirer.IsObjectAttached) return;

        if (_closestObject != null)
        {
            MovableObject movableObject = _closestObject.GetComponent<MovableObject>();
            if(movableObject != null)
            {
                movableObject.SetKinematic(false);
                // Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * CalculateForceByDistance(other.gameObject);
                Vector3 forceDir = _blower.FirePoint.forward * CalculateForceByDistance(movableObject.gameObject);
                movableObject.OnBlowableInteracts(forceDir, movableObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
            }
        }

        foreach (var obj in _objects)
        {
            if (!obj.IsLeaf()) continue;

            Vector3 forceDir = _blower.FirePoint.forward * CalculateForceByDistance(obj.gameObject);
            obj.GetComponent<MovableObject>().OnBlowableInteracts(forceDir, obj.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        }
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(_blower.FirePoint.position, go.transform.position);
        return _blower.Stats.BlowForce / Mathf.Max(1, distance);
    }
}
