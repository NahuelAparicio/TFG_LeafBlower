using UnityEngine;

public class BlowerForce : MonoBehaviour
{
    private BlowerController _blower;
    private Collider _collider;
    public Collider Collider => _collider;
    private float _maxForceDistance;

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _maxForceDistance = _collider.bounds.size.z;
    }

    private void OnTriggerStay(Collider other)
    {
        IBlowable blowable = other.GetComponent<IBlowable>();

        if(blowable == null)
        {
            return;
        }

        if (_blower.IsBlowing())
        {
            Debug.Log(CalculateForceByDistance(other.gameObject));
            Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * CalculateForceByDistance(other.gameObject);
            blowable.OnBlowableInteracts(forceDir, other.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        }
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(transform.position, go.transform.position);
        return _blower.Stats.blowForce.Value / Mathf.Max(1, distance);
    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;
}
