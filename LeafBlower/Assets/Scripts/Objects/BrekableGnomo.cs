using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrekableGnomo : NormalObject
{
    public UnityEvent OnBreak;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private float _force;
    [SerializeField] private Transform _explosionPos;
    [SerializeField] private float _radius;
    private bool isBroken = false;

    [SerializeField] private EventReference _breakableSound;

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

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasBeenShoot) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("WallTuto")) return;
        if (isBroken) return;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        RuntimeManager.PlayOneShot(_breakableSound, transform.position);
        OnBreak?.Invoke();
    }

    public virtual void ActivateBreak()
    {
        isBroken = true;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_force, _explosionPos.position, _radius, 0.5f, ForceMode.Impulse);
            }
        }
        Invoke(nameof(DestroyObject), 2.5f);
    }

    private void DestroyObject() => Destroy(gameObject);
}
