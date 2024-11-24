using UnityEngine;

public class BlowerForce : MonoBehaviour
{
    private BlowerController _blower;

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsBlowing() || outlineable == null) return;
        outlineable.EnableOutline();
    }

    private void OnTriggerStay(Collider other)
    {
        var blowable = other.GetComponent<IBlowable>();

        if (!_blower.IsBlowing() || blowable == null || _blower.Aspirer.ObjectAttached) return;

        //var collider = other.GetComponent<Collider>();

        Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * CalculateForceByDistance(other.gameObject);
        blowable.OnBlowableInteracts(forceDir, other.ClosestPointOnBounds(transform.position));
    }

    private void OnTriggerExit(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsBlowing() || outlineable == null) return;
        outlineable.DisableOutline();
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(transform.position, go.transform.position);
        return _blower.Stats.blowForce.Value / Mathf.Max(1, distance);
    }
}
