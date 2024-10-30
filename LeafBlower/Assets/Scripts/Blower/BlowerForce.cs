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
        if (other.gameObject.tag != "Movable")
            return;
        if (_blower.Inputs.IsBlowingInputPressed() && _blower.Stats.stamina.Value > 0)
        {
            IBlowable isBlowable = other.GetComponent<IBlowable>();
            if (isBlowable != null)
            {
                Vector3 directionToBlow = other.gameObject.transform.position - transform.position;
                other.GetComponent<IBlowable>().OnBlowableInteracts(CalculateForceByDistance(other.gameObject), directionToBlow.normalized);
            }

        }
    }

    private float CalculateForceByDistance(GameObject go)
    {
        float distance = Vector3.Distance(go.transform.position, transform.position);
        return distance * _blower.Stats.blowForce.Value;
    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;
}
