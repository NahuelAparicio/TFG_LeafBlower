using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    private BlowerController _blower;
    private Collider _collider;
    public Collider Collider => _collider;
    private GameObject _attachedObject;
    private Transform _pointToAttach;
    [SerializeField] private float _distanceToAttach;

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
        _pointToAttach = transform.GetChild(0).transform;
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void Update()
    {
        if(_attachedObject != null)
        {
            if(_blower.Inputs.IsBlowingInputPressed() && _blower.Stats.stamina.Value > 0)
            {
                //Vector3 dirToShoot = objectAttacher.AttachedGo.transform.position - objectAttacher.transform.position;
                //Player Direction
                _attachedObject.GetComponent<IShooteable>()?.OnShoot(_blower.Stats.blowForce.Value, transform.forward);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        IAspirable aspirable = other.GetComponent<IAspirable>();
        if(aspirable == null)
        {
            return;
        }

        if (!_blower.Inputs.IsBlowingInputPressed() && _blower.Inputs.IsAspiringInputPressed() && _blower.Stats.stamina.Value > 0)
        {
            //Aspire from object to attchPoint
            float distance = Vector3.Distance(_pointToAttach.position, other.transform.position);
            if(distance <= _distanceToAttach)
            {
                _attachedObject = other.gameObject;
                
            }
            Vector3 directionToAspire = _pointToAttach.position - other.transform.position;
            aspirable.OnAspiratableInteracts(_blower.Stats.aspireForce.Value, directionToAspire.normalized);
        }
    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;

    public void DetachObject()
    {
        _attachedObject = null;
    }
}
