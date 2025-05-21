using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableWall : MonoBehaviour
{
    public UnityEvent OnBreak;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private float _force;
    [SerializeField] private Transform _explosionPos;
    [SerializeField] private float _radius;

    [SerializeField] private EventReference _breakableSound;

    [SerializeField] private string _tagToCompare;

    public float timeToDestroy;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_tagToCompare))
        {
            RuntimeManager.PlayOneShot(_breakableSound, transform.position);
            OnBreak?.Invoke();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }

    public virtual void ActivateBreak()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_force, _explosionPos.position, _radius, 0.5f, ForceMode.Impulse);
            }
        }
        Invoke(nameof(DestroyObject), timeToDestroy);
    }

    private void DestroyObject() => Destroy(gameObject);
}

