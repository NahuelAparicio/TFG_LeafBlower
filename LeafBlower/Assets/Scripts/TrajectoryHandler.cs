using UnityEngine;

public class TrajectoryHandler : MonoBehaviour
{
    public int trajectoryPoints = 50;
    public float timeStep = 0.1f;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        DisableLineRender();
    }
    public void DrawTrajectory(Transform shootPoint, Rigidbody rb, float shootForce)
    {
        Vector3 startPosition = shootPoint.position;
        Vector3 startVelocity = shootPoint.transform.forward * shootForce / rb.mass;

        _lineRenderer.positionCount = trajectoryPoints;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * timeStep;

            Vector3 trajectoryPoint = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;

            _lineRenderer.SetPosition(i, trajectoryPoint);
        }
    }

    public void EnableLineRender() => _lineRenderer.enabled = true;
    public void DisableLineRender() => _lineRenderer.enabled = false;
}
