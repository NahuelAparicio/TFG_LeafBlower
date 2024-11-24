using UnityEngine;

[CreateAssetMenu(fileName = "JumpMovement", menuName = "Movements/Jump")]
public class JumpHandler : MovementBehavior
{
    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(forceDir, ForceMode.Impulse);
    }
}
