using UnityEngine;

[CreateAssetMenu(fileName = "DashMovement", menuName = "Movements/Dash")]
public class DashHandler : MovementBehavior
{
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        rb.AddForce(rb.transform.forward * force, ForceMode.Impulse);
    }
}
