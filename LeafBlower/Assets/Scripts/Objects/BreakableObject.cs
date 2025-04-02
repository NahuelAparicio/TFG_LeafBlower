using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;

public class BreakableObject : NormalObject
{
    public UnityEvent OnBreak;
    private bool _hasBeenShoot;
    [SerializeField] private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private float _force;
    [SerializeField] private Transform _explosionPos;
    [SerializeField] private float _radius;
    private bool isBroken = false;

    public override void StartAspiring(Transform target, Transform firePoint)
    {
        if (isBroken) return;
        base.StartAspiring(target, firePoint);
    }

    public override void StopAspiring()
    {
        if (isBroken) return;

        base.StopAspiring();
    }

    public override void OnBlow(Vector3 force, Vector3 point)
    {
        if (isBroken) return;

        base.OnBlow(force, point);
    }

    public override void Shoot(Vector3 force)
    {
        _hasBeenShoot = true;
        base.Shoot(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasBeenShoot) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        OnBreak?.Invoke();
    }

    public void ActivateBreak()
    {
        isBroken = true;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(_force, _explosionPos.position, _radius, 0.5f, ForceMode.Impulse);
            }
        }
        //Invoke(nameof(DestroyObject), 2.5f);
    }

    private void DestroyObject() => Destroy(gameObject);
}
