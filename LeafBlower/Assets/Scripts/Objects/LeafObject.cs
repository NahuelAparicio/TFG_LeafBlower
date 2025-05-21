using System.Collections;
using UnityEngine;
public class LeafObject : MovableObject
{
    public GameObject coin;
    public float timeToDestroy = 0.75f;
    private Coroutine _newCourotine = null;
    public bool isBreak = false;

    public override void OnBlow(Vector3 force, Vector3 point)
    {
        if (isBreak) return;
        force.y += Mathf.Abs(force.magnitude) * 1.5f;
        force.x += 5;
        force.z += 5;
        _rb.AddForceAtPosition(force, point);
        if(_newCourotine == null)
        {
            _newCourotine = StartCoroutine(DestroyAndInstantiateCoin());
        }
    }

    public override void StartAspiring(Transform target, Transform firePoint)
    {
        if (isBreak) return;
        base.StartAspiring(target, firePoint);
    }
    protected override void OnArriveToAttacher()
    {
        StopAspiring();
    }

    public override void StopAspiring()
    {
        _isBeingAspired = false;
        _target = null;
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
        _rb.useGravity = true;
    }

    IEnumerator DestroyAndInstantiateCoin()
    {
        isBreak = true;
        yield return new WaitForSeconds(timeToDestroy);
        GameEventManager.Instance.playerEvents.InvokeDestroy(this);
        Instantiate(coin, new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
