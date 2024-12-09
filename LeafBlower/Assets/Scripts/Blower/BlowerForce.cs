using UnityEngine;

public class BlowerForce : BaseLeafBlower
{
    protected override void OnTriggerStay(Collider other)
    {
        var blowable = other.GetComponent<IBlowable>();

        if (!_blower.IsBlowing() || blowable == null || _blower.Aspirer.IsObjectAttached) return;

        Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * CalculateForceByDistance(other.gameObject);
        blowable.OnBlowableInteracts(forceDir, other.ClosestPointOnBounds(transform.position));
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(_blower.FirePoint.position, go.transform.position);
        return _blower.Stats.BlowForce / Mathf.Max(1, distance);
    }
}
