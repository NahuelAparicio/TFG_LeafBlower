using UnityEngine;

public class CheckCollisions : MonoBehaviour
{
    private bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    private PlayerController _player;

    [Header("Ground Check:")]
    public float raycastHitPointDistance = 1f;
    public float maxSlopeAngle = 70f;
    public LayerMask groundLM;

    [Header("Wall Check:")]
    public float raycastWallCheckDistance = 0.5f;

    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
    }

    public RaycastHit GetGroundHit(Vector3 offset)
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + offset, Vector3.down, out _hit, raycastHitPointDistance, groundLM))
        {
            return _hit;
        }
        return _hit;
    }
    //Returns Adjusted player position if there is a wall collision on his direction
    public Vector3 IsWall(Vector3 direction)
    {
        Vector3 middle = transform.position;
        Vector3 high = transform.position;
        Vector3 low = transform.position;
        high.y += _player.playerCollider.height;
        middle.y += _player.playerCollider.height * 0.5f;
        low.y += 0.25f;
        Vector3[] rays = { high, middle, low };
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, direction, out RaycastHit hit, raycastWallCheckDistance, groundLM))
            {
                if (Vector3.Dot(hit.normal, direction) < 0)
                {
                    return Vector3.ProjectOnPlane(direction, hit.normal);
                }
            }
        }
        return direction;
    }

    private void OnDrawGizmos()
    {
        Vector3 middle = transform.position;
        Vector3 high = transform.position;
        Vector3 low = transform.position;
        high.y += _player.playerCollider.height;
        middle.y += _player.playerCollider.height * 0.5f;
        low.y += 0.25f;

        // Dibuja raycasts
        Gizmos.color = Color.red;
        Gizmos.DrawLine(middle, middle + _player.Movement.MoveDirection * raycastWallCheckDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(high, high + _player.Movement.MoveDirection * raycastWallCheckDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(low, low + _player.Movement.MoveDirection * raycastWallCheckDistance);

        Gizmos.DrawRay(transform.position + transform.forward * 0.15f, Vector3.down);

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(!_player.Movement.isJumping)
            {
                
                float slopeAngle = Vector3.Angle(GetGroundHit(Vector3.zero).normal, Vector3.up);
                Debug.Log(slopeAngle);
                if(slopeAngle <= maxSlopeAngle)
                {
                    _isGrounded = true;
                }
                else
                {
                    _isGrounded = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
    }
}
