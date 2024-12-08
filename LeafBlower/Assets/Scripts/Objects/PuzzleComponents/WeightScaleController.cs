using UnityEngine;

public class WeightScaleController : MonoBehaviour
{
    public float maximumRange = 5f; 
    public float moveSpeed = 2f;    

    public WSPlatform platformLeft, platformRight;

    private Vector3 _leftInitialPos, _rightInitialPos; 

    private void Start()
    {
        _leftInitialPos = platformLeft.transform.position;
        _rightInitialPos = platformRight.transform.position;
    }

    private void Update()
    {
        UpdatePlatforms();
    }

    private void UpdatePlatforms()
    {
        int totalWeight = platformLeft.CurrentWeight + platformRight.CurrentWeight;

        if (totalWeight == 0)
        {
            platformLeft.transform.position = Vector3.Lerp(platformLeft.transform.position, _leftInitialPos, moveSpeed * Time.deltaTime);
            platformRight.transform.position = Vector3.Lerp(platformRight.transform.position, _rightInitialPos, moveSpeed * Time.deltaTime);
            return;
        }

        // Calculate the target offset based on weight difference
        float deltaPosition = (platformLeft.CurrentWeight - platformRight.CurrentWeight) / (float)totalWeight * maximumRange;

        // Desired target pos within the range to initial pos
        Vector3 leftTargetPos = new Vector3(_leftInitialPos.x, _leftInitialPos.y - deltaPosition, _leftInitialPos.z);
        Vector3 rightTargetPos = new Vector3(_rightInitialPos.x, _rightInitialPos.y + deltaPosition, _rightInitialPos.z);

        platformLeft.transform.position = Vector3.Lerp(platformLeft.transform.position, leftTargetPos, moveSpeed * Time.deltaTime);
        platformRight.transform.position = Vector3.Lerp(platformRight.transform.position, rightTargetPos, moveSpeed * Time.deltaTime);
    }
}
