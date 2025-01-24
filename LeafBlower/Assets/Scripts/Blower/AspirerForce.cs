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

    protected override void Awake()
    {
        base.Awake();
        _trajectory = GetComponent<TrajectoryHandler>();
        _ground = LayerMask.NameToLayer("Ground");
        _movable = LayerMask.NameToLayer("Movable");

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
        _blower.StaminaHandler.ConsumeValueStamina(20);
        Vector3 forceDir = _blower.Stats.ShootForce * _blower.FirePoint.forward;
        _attachedObject.Item2.OnShoot(forceDir);
        DetachObject();
    }

    protected override void OnTriggerStay(Collider other)
    {
        var aspirable = other.GetComponent<IAspirable>();

        if(aspirable == null) return;

        var shooteable = other.GetComponent<ShootableObject>();
        var obj = other.GetComponent<Object>();

        HandleAspirating(other, aspirable, shooteable, obj);
    }

    private void HandleAspirating(Collider other, IAspirable aspirable, ShootableObject shooteable, Object obj)
    {
        if ((int)obj.weight > _blower.Player.Stats.Level + 1) return; // CHECK

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
