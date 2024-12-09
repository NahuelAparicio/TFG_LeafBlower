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
    private Vector3 direction = Vector3.zero;
    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
    }

    //Returns Adjusted player position if there is a wall collision on his direction
    public Vector3 IsWall(Vector3 _direction)
    {
        direction = _direction;
        Vector3 low, middle, high, shootPoint;
        if(_player.BlowerController.Aspirer.IsObjectAttached)
        {
            direction = _player.BlowerController.FirePoint.transform.forward;
            low = _player.BlowerController.Aspirer.AttachedObject.Item1.transform.position;
            middle = low;
            high = low;           
        }
        else
        {
            low = transform.position;
            middle = transform.position;
            high = transform.position;
        }
        shootPoint = _player.BlowerController.FirePoint.position;

        high.y += _player.playerCollider.height;
        middle.y += _player.playerCollider.height * -0.25f;
        low.y += _player.playerCollider.height * -0.5f;

        Vector3[] rays = { high, middle, low, shootPoint };
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, _direction, out RaycastHit hit, raycastWallCheckDistance))
            {
                if(hit.collider.tag != "IsWall")
                {
                    continue;
                }
                //float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
                //if (slopeAngle < _maxSlopeAngle)
                //{
                //    // It's a ground hit, ignore for wall collision
                //    continue;
                //}

                if (Vector3.Dot(hit.normal, _direction) < 0)
                {
                    return Vector3.ProjectOnPlane(_direction, hit.normal);
                }
            }
        }
        return _direction;
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

        Vector3 low, middle, high, shootPoint;
        //if (_player.BlowerController.Aspirer.ObjectAttached)
        //{
        //    low = _player.BlowerController.Aspirer.AttachedObject.Item1.transform.position;
        //    middle = low;
        //    high = low;
        //}
        //else
        //{
            low = transform.position;
            middle = transform.position;
            high = transform.position;
        //}

        // Adjust heights based on player's collider
        high.y += _player.playerCollider.height;
        middle.y += _player.playerCollider.height * -0.25f;
        low.y += _player.playerCollider.height * -0.5f;

        // Visualize the rays
        Gizmos.color = Color.red;  // Set the ray color to red for visibility
        Vector3[] rays = { high, middle, low };
        foreach (var ray in rays)
        {
            // Draw the ray using Gizmos.DrawLine
            Gizmos.DrawLine(ray, ray + direction * raycastWallCheckDistance);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Leaf"))
        {
            if(!_player.Movement.isJumping && !_isGrounded)
            {                
                _isGrounded = true;
                _player.Movement.ResetMovements();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Leaf"))
        {
            _isGrounded = false;
        }
    }



}
