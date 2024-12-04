using UnityEngine;

public class AspirerForce : BaseLeafBlower
{
    #region Variables
    private TrajectoryHandler _trajectory;
    private bool _isObjectAttached;
    private (Rigidbody, IShooteable) _attachedObject;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point

    #endregion
    #region Properties
    public bool IsObjectAttached => _isObjectAttached;
    public (Rigidbody, IShooteable) AttachedObject => _attachedObject;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _trajectory = GetComponent<TrajectoryHandler>();
    }

    protected override void Update()
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

    protected override void OnTriggerStay(Collider other)
    {
        var aspirable = other.GetComponent<IAspirable>();

        if(aspirable == null) return;

        var shooteable = other.GetComponent<IShooteable>();

        if (_blower.IsAspirating())
        {
            if(_isObjectAttached) return;

            //If true -> and attacheable true attach, and stop doing aspire force
            Vector3 pos = other.GetComponent<Collider>().ClosestPoint(_blower.FirePoint.position);
            if(_blower.DistanceToFirePoint(pos) <= _distanceToAttach)
            {
                if(shooteable != null)
                {
                    if(!other.GetComponent<ShootableObject>().IsAttached)
                    {
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
