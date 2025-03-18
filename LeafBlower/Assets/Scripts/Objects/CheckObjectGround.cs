using UnityEngine;

public class CheckObjectGround : MonoBehaviour
{
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        _isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * 1000);
    }
}
