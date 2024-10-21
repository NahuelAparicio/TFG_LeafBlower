using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public float radius = 0.1f;
    public LayerMask groundLM;

    private bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } }
    public float raycastDistance = 1f;

    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, radius, groundLM);
    }

    public Vector3 GetGroundHitPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLM))
        {
            return hit.point; 
        }
        return transform.position; // Return current position if no ground is hit
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
