using UnityEngine;

[CreateAssetMenu(fileName = "DashMovement", menuName = "Movements/Dash")]
public class DashHandler : MovementBehavior
{
    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        rb.AddForce(forceDir, ForceMode.Impulse);
    }
}
