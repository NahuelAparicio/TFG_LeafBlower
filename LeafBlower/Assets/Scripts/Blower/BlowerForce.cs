using UnityEngine;

public class BlowerForce : MonoBehaviour
{
    private BlowerController _blower;
    private Collider _collider;
    public Collider Collider => _collider;

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
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
            Vector3 forceDir = _blower.DirectionFromFirePointNormalized(other.gameObject.transform.position) * _blower.Stats.blowForce.Value;
            blowable.OnBlowableInteracts(forceDir, other.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
        }
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(go.transform.position, transform.position);
        return _blower.Stats.blowForce.Value;
    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;
}
