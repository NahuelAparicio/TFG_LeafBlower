using UnityEngine;

[CreateAssetMenu(fileName = "HoverMovement", menuName = "Movements/Hover")]
public class HoverHandler : MovementBehavior
{
    private float hoverRaycastDistance = 100f;
    public float desiredHoverHeight;
    public float hoverDamping;
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        RaycastHit hit;
        if (Physics.Raycast(rb.transform.position, Vector3.down, out hit, hoverRaycastDistance, LayerMask.GetMask("Ground")))
        {
            float currentHeight = rb.transform.position.y - hit.point.y;
            float heightDifference = desiredHoverHeight - currentHeight;

            if (heightDifference > 0)
            {
                float adjustedForce = force - Mathf.Abs(Physics.gravity.y);
                adjustedForce += heightDifference * hoverDamping;

                rb.AddForce(Vector3.up * adjustedForce, ForceMode.Acceleration);
            }
        }
        if (rb.velocity.y > 0.5f || rb.velocity.y < -0.5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.95f, rb.velocity.z);
        }
    }
}

#region HoverAllTheTime (Up & Down)
//float currentHeight = transform.position.y - hit.point.y;
//float heightDifference = desiredHoverHeight - currentHeight;

//float adjustedForce = _player.Stats.HoverForce - Mathf.Abs(Physics.gravity.y); 
//adjustedForce += heightDifference * hoverDamping;
//_player.Rigidbody.AddForce(Vector3.up * adjustedForce, ForceMode.Acceleration);
#endregion
