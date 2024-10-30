using UnityEngine;

public class CheckCollisions : MonoBehaviour
{
    public bool IsGrounded => _isGrounded;
    private PlayerController _player;

    [Header("Ground Check:")]
    [SerializeField] private LayerMask _groundLM;
    private bool _isGrounded;

    [Header("Slope Check:")]
    [SerializeField] private float _maxSlopeAngle = 60f;
    [SerializeField] private float maxDistanceSlopeRay = 4f;

    private float _slopeAngle;
    public float SlopeAngle => _slopeAngle;

    [Header("Wall Check:")]
    public float raycastWallCheckDistance = 0.5f;

    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
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
            if (Physics.Raycast(ray, direction, out RaycastHit hit, raycastWallCheckDistance))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
                if (slopeAngle < _maxSlopeAngle)
                {
                    // It's a ground hit, ignore for wall collision
                    continue;
                }

                if (Vector3.Dot(hit.normal, direction) < 0)
                {
                    return Vector3.ProjectOnPlane(direction, hit.normal);
                }
            }
        }
        return direction;
    }
    public bool OnSlope() => _slopeAngle < _maxSlopeAngle && _slopeAngle != 0;
    public bool IsOnMaxSlopeAngle() => _slopeAngle >= _maxSlopeAngle;

    public void UpdateTerrainSlopeAngle()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _player.Movement.slopeHit, maxDistanceSlopeRay, _groundLM))
        {
            _slopeAngle = Vector3.Angle(Vector3.up, _player.Movement.slopeHit.normal);
        }
    }

    private void OnDrawGizmos()
    {       
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(!_player.Movement.isJumping)
            {                
                _isGrounded = true;
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
