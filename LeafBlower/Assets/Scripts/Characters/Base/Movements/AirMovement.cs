using UnityEngine;

[CreateAssetMenu(fileName = "AirMovement", menuName = "Movements/AirMovement")]
public class AirMovement : NormalMovement
{
    public float airAccelerationExtra;
    public override void ExecuteMovement(Rigidbody rb, Vector3 forceDir)
    {
        base.ExecuteMovement(rb, forceDir);
        forceDir *= airAccelerationExtra;
        rb.AddForce(forceDir, ForceMode.Acceleration);
    }
}
