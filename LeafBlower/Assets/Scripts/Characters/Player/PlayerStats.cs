using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]private float _acceleration;
    public float Acceleration => _acceleration;

    [SerializeField] private float _maxSpeed;
    public float MaxSpeed => _maxSpeed;

    [SerializeField] private float _airAcceleration;
    public float AirAcceleration => _airAcceleration;  

    [SerializeField] private float _maxAirSpeed;
    public float MaxAirSpeed => _maxAirSpeed;

    [SerializeField] private float _dashForce;
    public float DashForce => _dashForce;

    [SerializeField] private float _jumpForce;
    public float JumpForce => _jumpForce;
    //Change this to variables with modifiers
}
