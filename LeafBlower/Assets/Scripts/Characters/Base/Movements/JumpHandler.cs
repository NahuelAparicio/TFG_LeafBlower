using UnityEngine;

[CreateAssetMenu(fileName = "JumpMovement", menuName = "Movements/Jump")]
public class JumpHandler : MovementBehavior
{
    [SerializeField] private int _maxJumps;
    private int _currentJumps;
    public override bool CanExecuteMovement() => _maxJumps > _currentJumps;
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        _currentJumps++;
    }
    public override void ResetMovement() => _currentJumps = 0;

}
