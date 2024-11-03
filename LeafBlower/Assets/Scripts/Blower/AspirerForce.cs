using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    #region Variables
    private BlowerController _blower;
    private Collider _collider;

    private bool _isObjectAttached;
    private (GameObject, IShooteable) _attachedObject;

    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point
    #endregion
    #region Properties
    public Collider Collider => _collider;
    public bool ObjectAttached => _isObjectAttached;
    #endregion

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void Update()
    {
        if(_isObjectAttached)
        {
            if(_blower.IsShooting())
            {
                //Vector3 dirToShoot = objectAttacher.AttachedGo.transform.position - objectAttacher.transform.position;
                //Player Direction
                Vector3 forceDir = _blower.Stats.aspireForce.Value * _blower.FirePoint.forward;
                _attachedObject.Item2.OnShoot(forceDir);
                DetachObject();
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

        IShooteable shooteable = other.GetComponent<IShooteable>();


        if (_blower.IsAspirating())
        {
            //If true -> and attacheable true attach, and stop doing aspire force
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(_blower.FirePoint.position);
            if(_blower.DistanceToFirePoint(pos) <= _distanceToAttach)
            {
                if(shooteable != null)
                {
                    AttachObject(other.gameObject, shooteable);
                }
                else
                {
                    //No force applied just gravity, Should lerp? Deceleration?
                    //other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else
            {
                Vector3 forceDir = _blower.DirectionToFirePointNormalized(other.transform.position) * _blower.Stats.aspireForce.Value;
                aspirable.OnAspiratableInteracts(forceDir);
            }
        }
    }

    public void EnableCollider() => _collider.enabled = true;
    public void DisableCollider() => _collider.enabled = false;

    public void AttachObject(GameObject obj, IShooteable shooteable)
    {
        _attachedObject.Item1 = obj;
        _attachedObject.Item2 = shooteable;
        obj.GetComponent<IAttacheable>().Attach(_blower.FirePoint, _blower.FirePoint.position, false);
        _isObjectAttached = true;
    }

    public void DetachObject()
    {
        _attachedObject.Item1.GetComponent<IAttacheable>().Detach();

        _attachedObject.Item1 = null;
        _attachedObject.Item2 = null;
        _isObjectAttached = false;
    }
}
