using UnityEngine;

public class AspirerForce : BaseLeafBlower
{
    #region Variables
    public AttachableObject attachableObject;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point
    private LayerMask ground;
    private LayerMask movable;
    #endregion


    private float _timePressed = 0;
    public Transform targetToaim;
    private Vector3 targetToAimDefaultPos;

    public float addedForceOnMaxPressed;
    public float _maxTimeToShoot;
    public float maxOffsetYTargetToAim;
    protected override void Awake()
    {
        base.Awake();
        targetToAimDefaultPos = targetToaim.localPosition;
        ground = LayerMask.NameToLayer("Ground");
        movable = LayerMask.NameToLayer("Movable");
    }

    protected override void Update()
    {
        if(!attachableObject.IsAttached)
        {
            ResetTargetToAimPosition();
            return;
        }

        if (_blower.IsShooting())
        {
            _timePressed += Time.deltaTime;
            _blower.Hud.UpdateShootBarForce(_timePressed, _maxTimeToShoot);

            float normalizedTime = Mathf.Clamp01(_timePressed / _maxTimeToShoot);

            float force = Mathf.Lerp(_blower.Stats.ShootForce, _blower.Stats.ShootForce + addedForceOnMaxPressed, normalizedTime);

            UpdateTargetToAimPosition(normalizedTime);

            attachableObject.trajectory.DrawTrajectory(_blower.FirePoint, attachableObject.Rigidbody, force);

            if (_timePressed >= _maxTimeToShoot)
            {
                _timePressed = _maxTimeToShoot;
                ShootAction(force);
            }
        }
        else
        {
            attachableObject.trajectory.DrawTrajectory(_blower.FirePoint, attachableObject.Rigidbody, _blower.Stats.ShootForce);

            if (_timePressed != 0)
            {
                ShootAction(_blower.Stats.ShootForce);
            }
        }
    }

    private void UpdateTargetToAimPosition(float normalizedTime)
    {
        Vector3 targetPosition = targetToAimDefaultPos + new Vector3(0, maxOffsetYTargetToAim * normalizedTime, 0);
        targetToaim.localPosition = targetPosition;
    }

    private void ResetTargetToAimPosition()
    {
        if (targetToaim.localPosition != targetToAimDefaultPos) 
           targetToaim.localPosition = Vector3.Lerp(targetToaim.localPosition, targetToAimDefaultPos, Time.deltaTime * 5);
    }

    private void ShootAction(float force)
    {
        Vector3 forceDir = force * _blower.FirePoint.forward;

        _blower.StaminaHandler.ConsumeValueStamina(20);
        attachableObject.Shootable.OnShoot(forceDir);
        _timePressed = 0;
        _blower.Hud.ResetShootBarForce();
        attachableObject.Detach();
    }

    protected override void HandleAspire(Object obj)
    {
        base.HandleAspire(obj);

        if (_closestObject != null)
        {
            MovableObject movableObject = _closestObject.GetComponent<MovableObject>();
            if (movableObject != null)
            {
                movableObject.SetKinematic(false);
            }
        }

        Collider collider = obj.GetComponent<Collider>();
        IAspirable aspirable = obj.GetComponent<IAspirable>();
        ShootableObject shooteable = obj.GetComponent<ShootableObject>();

        if(!_blower.IsAspirating())
        {
            if (!_blower.IsShooting())
            {
                HandleNotAspirating(collider, shooteable);
            }
            return;
        }

        if (attachableObject.IsAttached) return;  

        if (collider.gameObject.layer != movable)
            collider.gameObject.layer = movable;

        Vector3 pos = collider.ClosestPoint(_blower.FirePoint.position);

        if (_blower.DistanceToFirePoint(pos) <= _distanceToAttach && (int)obj.weight <= _blower.Player.Stats.Level)
        {
            TryAttachObject(collider, pos, shooteable);
        }
        else
        {
            Vector3 forceDir = _blower.DirectionToFirePointNormalized(collider.transform.position) * _blower.Stats.AspireForce;
            aspirable.OnAspiratableInteracts(forceDir);
        }
    }

    private void TryAttachObject(Collider other, Vector3 closestPoint, ShootableObject shooteable)
    {
        if (shooteable != null)
        {
            if (other.GetComponent<ShootableObject>().IsAttached) return;

            attachableObject.Attach(other.attachedRigidbody, closestPoint, _blower.FirePoint);
        }
        else
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    protected override void HandleNotAspirating(Collider other, ShootableObject shooteable)
    {
        if(other.gameObject.layer != ground)
            other.gameObject.layer = ground;
        if (shooteable == null) return;
        if (!shooteable.IsAttached) return;
        if(attachableObject.IsAttached)
        {
            attachableObject.Detach();
        }
    }
}
