using System.Collections.Generic;
using UnityEngine;

public class AspirerForce : BaseLeafBlower
{
    #region Variables
    [SerializeField] private float _distanceToAttach; //Minim distance to attach the object to the point
    private TrajectoryHandler _trajectory;
    private bool _isObjectAttached;
    private (Rigidbody, ShootableObject) _attachedObject;
    private LayerMask _ground, _movable;
    #endregion
    #region Properties
    public bool IsObjectAttached => _isObjectAttached;
    public (Rigidbody, ShootableObject) AttachedObject => _attachedObject;
    #endregion

    private Dictionary<Collider, (IAspirable, IShooteable, Rigidbody)> _componentCache = new Dictionary<Collider, (IAspirable, IShooteable, Rigidbody)>();

    private float _timePressed = 0;
    public Transform targetToaim;
    private Vector3 targetToAimDefaultPos;

    public float addedForceOnMaxPressed;
    public float _maxTimeToShoot;
    public float maxOffsetYTargetToAim;
    protected override void Awake()
    {
        base.Awake();
        _trajectory = GetComponent<TrajectoryHandler>();
        _ground = LayerMask.NameToLayer("Ground");
        _movable = LayerMask.NameToLayer("Movable");
        targetToAimDefaultPos = targetToaim.localPosition;

    }

    protected override void Update()
    {
        if(!_isObjectAttached)
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

            _trajectory.DrawTrajectory(_blower.FirePoint, _attachedObject.Item1, force);

            if (_timePressed >= _maxTimeToShoot)
            {
                _timePressed = _maxTimeToShoot;
                ShootAction(force);
            }
        }
        else
        {
            _trajectory.DrawTrajectory(_blower.FirePoint, _attachedObject.Item1, _blower.Stats.ShootForce);

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
        _attachedObject.Item2.OnShoot(forceDir);
        _timePressed = 0;
        _blower.Hud.ResetShootBarForce();
        DetachObject();
    }

    protected override void OnTriggerStay(Collider other)
    {

        if (!_blower.IsAspirating()) return;

        if(_closestObject != null)
        {
            MovableObject movableObject = _closestObject.GetComponent<MovableObject>();
            if (movableObject != null)
            {
                movableObject.SetKinematic(false);
            }

            var aspirable = _closestObject.GetComponent<IAspirable>();
            var shooteable = _closestObject.GetComponent<ShootableObject>();
            var obj = _closestObject.GetComponent<Object>();

            HandleAspirating(_closestObject.GetComponent<Collider>(), aspirable, shooteable, obj);
        }

        foreach (var obj in _objects)
        {
            if (!obj.IsLeaf()) continue;

            HandleAspirating(obj.GetComponent<Collider>(), obj.GetComponent<IAspirable>(), obj.GetComponent<ShootableObject>(), obj.GetComponent<Object>());
        }

    }

    private void HandleAspirating(Collider other, IAspirable aspirable, ShootableObject shooteable, Object obj)
    {

        if (_blower.IsAspirating())
        {
            if (_isObjectAttached) return;

            if (other.gameObject.layer != _movable)
                other.gameObject.layer = _movable;

            Vector3 pos = other.ClosestPoint(_blower.FirePoint.position);

            if (_blower.DistanceToFirePoint(pos) <= _distanceToAttach && (int)obj.weight <= _blower.Player.Stats.Level)
            {
                TryAttachObject(other, pos, shooteable);
            }
            else
            {
                Vector3 forceDir = _blower.DirectionToFirePointNormalized(other.transform.position) * _blower.Stats.AspireForce;
                aspirable.OnAspiratableInteracts(forceDir);
            }
        }
        else
        {
            if(!_blower.IsShooting())
                HandleNotAspirating(other, shooteable);
        }
    }
    private void TryAttachObject(Collider other, Vector3 closestPoint, ShootableObject shooteable)
    {
        if (shooteable != null)
        {
            if (other.GetComponent<ShootableObject>().IsAttached) return;

            AttachObject(other.attachedRigidbody, closestPoint, shooteable);
        }
        else
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void HandleNotAspirating(Collider other, ShootableObject shooteable)
    {
        if(other.gameObject.layer != _ground)
            other.gameObject.layer = _ground;
        if (shooteable == null) return;
        if (!shooteable.IsAttached) return;
        
        DetachObject();
    }

    public void AttachObject(Rigidbody rb, Vector3 closestPoint, ShootableObject shooteable)
    {
        _trajectory.EnableLineRender();
        _attachedObject = (rb, shooteable);
        rb.GetComponent<IAttacheable>().Attach(_blower.FirePoint, closestPoint);
        _isObjectAttached = true;
    }

    public void DetachObject()
    {
        _trajectory.DisableLineRender();
        _attachedObject.Item1.GetComponent<IAttacheable>().Detach();
        _attachedObject = (null, null);
        _isObjectAttached = false;
    }

    public void SaveObject()
    {
        _trajectory.DisableLineRender();
        _attachedObject = (null, null);
        _isObjectAttached = false;
    }
}
