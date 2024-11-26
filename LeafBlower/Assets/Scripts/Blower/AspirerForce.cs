using UnityEngine;

public class AspirerForce : MonoBehaviour
{
    #region Variables
    private BlowerController _blower;

    private bool _isObjectAttached;
    private (GameObject, IShooteable) _attachedObject;
   // private float _timePressed = 0f;
   // [SerializeField]private float _maxTimeToShoot;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point

    #endregion
    #region Properties
    public bool ObjectAttached => _isObjectAttached;
    #endregion

    private void Awake()
    {
        _blower = transform.parent.GetComponent<BlowerController>();
    }

    private void Update()
    {
        if (!_isObjectAttached) return;

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
        Vector3 forceDir = _blower.Stats.aspireForce.Value * _blower.FirePoint.forward;
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
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(_blower.FirePoint.position);
            if(_blower.DistanceToFirePoint(pos) <= _distanceToAttach)
            {
                if(shooteable != null)
                {
                    if(!other.GetComponent<ShootableObject>().IsAttached)
                        AttachObject(other.gameObject, pos, shooteable);
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
                Vector3 forceDir = _blower.DirectionToFirePointNormalized(other.transform.position) * _blower.Stats.aspireForce.Value;
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

    public void AttachObject(GameObject obj, Vector3 closestPoint, IShooteable shooteable)
    {
        obj.layer = LayerMask.NameToLayer("Movable");
        _attachedObject.Item1 = obj;
        _attachedObject.Item2 = shooteable;
        obj.GetComponent<IAttacheable>().Attach(_blower.FirePoint, closestPoint);
        _isObjectAttached = true;
    }

    public void DetachObject()
    {
        _attachedObject.Item1.layer = LayerMask.NameToLayer("Ground");
        _attachedObject.Item1.GetComponent<IAttacheable>().Detach();
        _attachedObject.Item1 = null;
        _attachedObject.Item2 = null;
        _isObjectAttached = false;
    }
}
