using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    #region Variables
    private BlowerController _blower;
    private TrajectoryHandler _trajectory;
    private bool _isObjectAttached;
    private (Rigidbody, IShooteable) _attachedObject;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point

    #endregion
    #region Properties
    public bool IsObjectAttached => _isObjectAttached;
    public (Rigidbody, IShooteable) AttachedObject => _attachedObject;
    #endregion

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
        _trajectory = GetComponent<TrajectoryHandler>();
    }

    private void Update()
    {
        if (!_isObjectAttached) return;

        _trajectory.DrawTrajectory(_blower.FirePoint, _attachedObject.Item1, _blower.Stats.ShootForce);

        if(_blower.IsShooting())
        {
            ShootAction();
        }
    }

    private void ShootAction()
    {
        Vector3 forceDir = _blower.Stats.ShootForce * _blower.FirePoint.forward;
        _attachedObject.Item2.OnShoot(forceDir);
        DetachObject();
    }
    private void OnTriggerEnter(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsAspirating() || outlineable == null) return;
        outlineable.EnableOutline();
    }

    private void OnTriggerStay(Collider other)
    {
        var aspirable = other.GetComponent<IAspirable>();

        if(aspirable == null) return;

        var shooteable = other.GetComponent<IShooteable>();

        if (_blower.IsAspirating())
        {
            if(_isObjectAttached)
            {
                return;
            }

            //If true -> and attacheable true attach, and stop doing aspire force
            Vector3 pos = other.GetComponent<Collider>().ClosestPoint(_blower.FirePoint.position);
            if(_blower.DistanceToFirePoint(pos) <= _distanceToAttach)
            {
                if(shooteable != null)
                {
                    if(!other.GetComponent<ShootableObject>().IsAttached)
                    {
                        Debug.Log("PositionClosestPoint = " + pos);
                        AttachObject(other.attachedRigidbody, pos, shooteable);
                    }
                }
                else
                {
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //No force applied just gravity, Should lerp? Deceleration?
                    //other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else
            {
                Vector3 forceDir = _blower.DirectionToFirePointNormalized(other.transform.position) * _blower.Stats.AspireForce;
                aspirable.OnAspiratableInteracts(forceDir);
            }
        }
        else
        {
            if (shooteable != null)
                if (other.GetComponent<ShootableObject>().IsAttached)
                    DetachObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var outlineable = other.GetComponent<IOutlineable>();
        if (_blower.IsAspirating() || outlineable == null) return;
        outlineable.DisableOutline();
    }

    public void AttachObject(Rigidbody rb, Vector3 closestPoint, IShooteable shooteable)
    {
        _trajectory.EnableLineRender();
        rb.gameObject.layer = LayerMask.NameToLayer("Movable");
        _attachedObject.Item1 = rb;
        _attachedObject.Item2 = shooteable;
        rb.GetComponent<IAttacheable>().Attach(_blower.FirePoint, closestPoint);
        _isObjectAttached = true;
    }

    public void DetachObject()
    {
        _trajectory.DisableLineRender();
        _attachedObject.Item1.GetComponent<IAttacheable>().Detach();
        _attachedObject.Item1 = null;
        _attachedObject.Item2 = null;
        _isObjectAttached = false;
    }

    public void SaveObject()
    {
        _trajectory.DisableLineRender();
        _attachedObject.Item1 = null;
        _attachedObject.Item2 = null;
        _isObjectAttached = false;
    }
}
