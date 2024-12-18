using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header ("Movement Stats:")]
    [SerializeField] private Stat _walkSpeed;
    public float WalkSpeed => _walkSpeed.Value;

    [SerializeField] private Stat _runSpeed;
    public float RunSpeed => _runSpeed.Value;

    [Header("Forces Stats:")]

    [SerializeField] private Stat _dashForce;
    public float DashForce => _dashForce.Value;

    [SerializeField] private Stat _hoverForce;
    public float HoverForce => _hoverForce.Value;

    [SerializeField] private Stat _jumpForce;
    public float JumpForce => _jumpForce.Value;

}
