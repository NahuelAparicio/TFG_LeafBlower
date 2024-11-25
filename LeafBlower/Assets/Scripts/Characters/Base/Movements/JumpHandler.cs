using UnityEngine;

[CreateAssetMenu(fileName = "JumpMovement", menuName = "Movements/Jump")]
public class JumpHandler : MovementBehavior
{
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}
