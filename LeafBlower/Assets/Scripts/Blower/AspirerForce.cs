using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    #region Variables
    private BlowerController _blower;
    private TrajectoryHandler _trajectory;
    private bool _isObjectAttached;
    private (Rigidbody, IShooteable) _attachedObject;
   // private float _timePressed = 0f;
   // [SerializeField]private float _maxTimeToShoot;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point

    #endregion
    #region Properties
    public bool ObjectAttached => _isObjectAttached;
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
            //_timePressed += Time.deltaTime;
            //_blower.Hud.UpdateShootBarForce(_timePressed, _maxTimeToShoot);
            //if(_timePressed >= _maxTimeToShoot)
            //{
            //    _timePressed = _maxTimeToShoot;
            //    ShootAction();
            //}
        }
        else
        {
            //if(_timePressed != 0)
            //{
            //    ShootAction();
            //}
        }
    }

    private void ShootAction()
    {
        //float force = (_blower.Stats.aspireForce.Value * _timePressed) / _maxTimeToShoot;
        Vector3 forceDir = _blower.Stats.ShootForce * _blower.FirePoint.forward;
        _attachedObject.Item2.OnShoot(forceDir);
        DetachObject();
        //_timePressed = 0;
        //_blower.Hud.ResetShootBarForce();
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
           // if(shooteable != null)
                //if(other.GetComponent<ShootableObject>().IsAttached)
                   // DetachObject();
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
        _attachedObject.Item1.gameObject.layer = LayerMask.NameToLayer("Ground");
        _attachedObject.Item1 = null;
        _attachedObject.Item2 = null;
        _isObjectAttached = false;
    }
}
