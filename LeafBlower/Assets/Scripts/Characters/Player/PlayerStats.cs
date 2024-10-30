using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header ("Movement Stats:")]
    [SerializeField] private Stat _maxSpeed;
    public float MaxSpeed => _maxSpeed.Value;

    [SerializeField]private float _acceleration;
    public float Acceleration => _acceleration;

    [SerializeField] private float _airAcceleration;
    public float AirAcceleration => _airAcceleration;  

    [SerializeField] private Stat _dashForce;
    public float DashForce => _dashForce.Value;

    [SerializeField] private Stat _jumpForce;
    public float JumpForce => _jumpForce.Value;
}
