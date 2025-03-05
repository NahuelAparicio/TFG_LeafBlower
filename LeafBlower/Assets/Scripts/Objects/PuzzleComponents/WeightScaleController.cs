using UnityEngine;

public class WeightScaleController : MonoBehaviour
{
    public float maximumRange = 5f; 
    public float moveSpeed = 2f;
    public float abruptChangeFactor = .5f;
    public float timeToCheck = 0.25f;

    public WSPlatform platformLeft, platformRight;

    private Vector3 _leftInitialPos, _rightInitialPos;
    [SerializeField] private PlayerController _player;

    [SerializeField] private TrackingPlatform _trackingPlatformLeft, _trackingPlatformRight;

    private float _previousYLeft, _previousYRight;
    private float _currentTime = 0f;
    [SerializeField] private bool canThrowObjects = true;
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _leftInitialPos = platformLeft.transform.position;
        _rightInitialPos = platformRight.transform.position;
        _previousYLeft = platformLeft.transform.position.y;
        _previousYRight = platformRight.transform.position.y;
    }

    private void Update()
    {
        UpdatePlatforms();
        _currentTime += Time.deltaTime;
        if (_currentTime >= timeToCheck)
        {
            CheckAbruptTime();
            _currentTime = 0f; 
        }
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

    private void CheckAbruptTime()
    {
        if (!canThrowObjects) return;

        float leftPlatformDeltaY = platformLeft.transform.position.y - _previousYLeft;
        float rightPlatformDeltaY = platformRight.transform.position.y - _previousYRight;


        if (_trackingPlatformRight.IsPlayerInPlatform && leftPlatformDeltaY < -abruptChangeFactor)
        {
            _player.Rigidbody.AddForce(Vector3.up * 40 + Vector3.left * 5, ForceMode.Impulse);
        }
        else if (_trackingPlatformLeft.IsPlayerInPlatform && rightPlatformDeltaY < -abruptChangeFactor)
        {
            _player.Rigidbody.AddForce(Vector3.up * 40 + Vector3.right * 5, ForceMode.Impulse);
        }

        _previousYLeft = platformLeft.transform.position.y;
        _previousYRight = platformRight.transform.position.y;
    }
}
