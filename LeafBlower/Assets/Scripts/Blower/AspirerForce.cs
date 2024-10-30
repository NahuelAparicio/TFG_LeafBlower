using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    private BlowerController _blower;
    private Collider _collider;
    public Collider Collider => _collider;

    private GameObject _attachedObject;

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

        if (_blower.Inputs.IsAspiringInputPressed() && _blower.Stats.stamina.Value > 0)
        {
            Debug.Log("Is Applying Force To Aspire");
        }

    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;
}
