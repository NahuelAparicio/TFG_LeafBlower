using UnityEngine;

public class BlowerForce : BaseLeafBlower
{
    protected override void OnTriggerStay(Collider other)
    {
        var blowable = other.GetComponent<IBlowable>();

        if (!_blower.IsBlowing() || blowable == null || _blower.Aspirer.IsObjectAttached) return;

        if ((int)other.GetComponent<Object>().weight > _blower.Player.Stats.Level + 1) return;

       // Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * CalculateForceByDistance(other.gameObject);
        Vector3 forceDir = _blower.FirePoint.forward * CalculateForceByDistance(other.gameObject);
        blowable.OnBlowableInteracts(forceDir, other.ClosestPointOnBounds(transform.position));
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(_blower.FirePoint.position, go.transform.position);
        return _blower.Stats.BlowForce / Mathf.Max(1, distance);
    }
}
