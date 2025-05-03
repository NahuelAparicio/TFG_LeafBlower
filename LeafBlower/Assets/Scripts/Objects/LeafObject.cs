using System.Collections;
using UnityEngine;
public class LeafObject : MovableObject
{
    public GameObject coin;
    public float timeToDestroy = 0.75f;
    private Coroutine _newCourotine = null;

    public override void OnBlow(Vector3 force, Vector3 point)
    {
        force.y += Mathf.Abs(force.magnitude) * 0.5f;
        _rb.AddForceAtPosition(force, point);
        if(_newCourotine == null)
        {
            _newCourotine = StartCoroutine(DestroyAndInstantiateCoin());
        }
    }
    protected override void OnArriveToAttacher()
    {
        StopAspiring();
    }

    IEnumerator DestroyAndInstantiateCoin()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Instantiate(coin, new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
