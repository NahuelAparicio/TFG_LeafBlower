using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
    [SerializeField] private Transform _followTarget; 
    [SerializeField] private float _followSpeed = 5f; 


    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(_followTarget.position.x, transform.position.y, _followTarget.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);

    }
}
