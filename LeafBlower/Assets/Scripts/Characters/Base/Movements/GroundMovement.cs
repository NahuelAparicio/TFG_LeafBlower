using UnityEngine;

[CreateAssetMenu(fileName = "GroundMovement", menuName = "Movements/GroundMovement")]
public class GroundMovement : NormalMovement
{
    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        base.ExecuteMovement(rb, forceDir);
        rb.AddForce(forceDir, ForceMode.VelocityChange);
    }
}
