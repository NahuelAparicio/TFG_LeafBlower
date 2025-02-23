using UnityEngine;

public class AspirerForce : BaseLeafBlower
{
    #region Variables
    public AttachableObject attachableObject;
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point
    private LayerMask ground;
    private LayerMask movable;
    #endregion

    public float _shootDelayThreshold = 0.25f;
    private float _timePressed = 0;
    public float TimePressed => _timePressed;
    public Transform targetToaim;
    private Vector3 targetToAimDefaultPos;

    public float addedForceOnMaxPressed;
    public float _maxTimeToShoot;
    public float maxOffsetYTargetToAim;


    public bool wasShootPressed = false;
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
            wasShootPressed = true;
            _timePressed += Time.deltaTime;

            if(_timePressed > _shootDelayThreshold)
            {
                float chargeTime = Mathf.PingPong(Time.time * 2, 1);
                float effectiveTime = chargeTime * _maxTimeToShoot;

                _blower.Hud.UpdateShootBarForce(effectiveTime, _maxTimeToShoot);
                UpdateTargetToAimPosition(chargeTime);
            }

            attachableObject.trajectory.DrawTrajectory(_blower.FirePoint, attachableObject.Rigidbody, GetShootForce());
        }
        else
        {
            if (wasShootPressed)
            {
                wasShootPressed = false;
                ShootAction(GetShootForce()); 
            }
            if(attachableObject.Rigidbody != null)
            {
                attachableObject.trajectory.DrawTrajectory(_blower.FirePoint, attachableObject.Rigidbody, GetShootForce());
            }
            _timePressed = 0f; 
        }
     
    }
    private float GetEffectiveTime() => Mathf.Max(0, _timePressed - _shootDelayThreshold);
    private float GetNormalizedTime() => Mathf.Clamp01(GetEffectiveTime() / _maxTimeToShoot);
    private float GetShootForce() => Mathf.Lerp(_blower.Stats.ShootForce, _blower.Stats.ShootForce + addedForceOnMaxPressed, GetNormalizedTime());
    public bool IsNormalShoot() => _timePressed < _shootDelayThreshold;

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

    public void ShootAction(float force)
    {
        _blower.StaminaHandler.ConsumeValueStamina(20);
        Vector3 forceDir = force * _blower.FirePoint.forward;
        attachableObject.Shootable.OnShoot(forceDir);
        _timePressed = 0;
        _blower.Hud.ResetShootBarForce();
        attachableObject.Detach();
    }

    protected override void HandleAspire(Object obj)
    {
        base.HandleAspire(obj);

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

    public void AttachObjectOnSave()
    {
        if (attachableObject.IsAttached) return;


        Collider collider = _closestObject.GetComponent<Collider>();
        IAspirable aspirable = _closestObject.GetComponent<IAspirable>();
        ShootableObject shooteable = _closestObject.GetComponent<ShootableObject>();
        _closestObject.transform.position = _blower.FirePoint.position;
        TryAttachObject(collider,_blower.FirePoint.position, shooteable);
    }

    private void TryAttachObject(Collider other, Vector3 closestPoint, ShootableObject shooteable)
    {
        if (shooteable != null)
        {
            if (other.GetComponent<ShootableObject>().IsAttached || other.GetComponent<ShootableObject>().HasBeenShoot) return;

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
