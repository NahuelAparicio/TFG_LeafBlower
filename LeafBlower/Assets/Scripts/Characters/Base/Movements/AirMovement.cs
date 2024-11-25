using UnityEngine;

[CreateAssetMenu(fileName = "AirMovement", menuName = "Movements/AirMovement")]
public class AirMovement : NormalMovement
{
    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        base.ExecuteMovement(rb, forceDir);
        rb.AddForce(forceDir, ForceMode.Acceleration);
    }
}
