using UnityEngine;
using System.Collections;

public class TunelComponent : MonoBehaviour
{
    public Transform[] pointsToFollow;
    public Vector3 targetScale;
    public float lerpSpeed;
    private bool _isTunnelBeingUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTunnelBeingUsed) return;

        ShootableObject shootableObject = other.GetComponent<ShootableObject>();

        if(shootableObject != null)
        {
            if(!shootableObject.IsAttached)
            {
                shootableObject.isInTunel = true;
                ResetShootableForces(shootableObject, false);
                StartCoroutine(MoveThroughTunnel(shootableObject));
            }
        }
    }

    private IEnumerator MoveThroughTunnel(ShootableObject shootableObject)
    {
        _isTunnelBeingUsed = true;
        float scaleLerpTime = 0;
        int _currentPointIndex = 0;

        Vector3 initialScale = shootableObject.gameObject.transform.localScale;
        GameObject objToMove = shootableObject.gameObject;

        while (scaleLerpTime < 1f)
        {
            scaleLerpTime += Time.deltaTime * lerpSpeed;
            shootableObject.gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleLerpTime);
            yield return null;
        }
        scaleLerpTime = 0;

        while (_currentPointIndex < pointsToFollow.Length)
        {
            float moveLerpTime = 0;
            Vector3 targetPosition = pointsToFollow[_currentPointIndex].position;
            Vector3 initialPosition = objToMove.transform.position;

            while (moveLerpTime < 1f)
            {
                moveLerpTime += Time.deltaTime * lerpSpeed;
                objToMove.transform.position = Vector3.Lerp(initialPosition, targetPosition, moveLerpTime);
                if(_currentPointIndex == pointsToFollow.Length - 1)
                {
                    scaleLerpTime += Time.deltaTime * lerpSpeed;
                    objToMove.transform.localScale = Vector3.Lerp(objToMove.transform.localScale, initialScale, scaleLerpTime);
                }
                yield return null;
            }
            _currentPointIndex++;
        }

        _isTunnelBeingUsed = false;
        shootableObject.isInTunel = false;
        ResetShootableForces(shootableObject, true);
        shootableObject.UnFreeze();
        shootableObject.Rigidbody.AddForce(10 * Vector3.down, ForceMode.Impulse);
    }

    private void ResetShootableForces(ShootableObject shootableObject, bool _useGravity)
    {
        shootableObject.Rigidbody.velocity = Vector3.zero;
        shootableObject.Rigidbody.angularVelocity = Vector3.zero;
        shootableObject.Rigidbody.useGravity = _useGravity;
    }
}
