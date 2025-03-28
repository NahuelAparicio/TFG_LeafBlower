using Cinemachine;
using UnityEngine;

public class PlayerAimTarget : MonoBehaviour
{
    public Transform targetToAim;
    public Vector2 movementRangeX = new Vector2(-5, 5);
    public Vector2 movementRangeY = new Vector2(-3, 3);
    public float movementSpeed = 5f;

    private PlayerController _player;
    private Vector3 _initialPosition;

    public CinemachineFreeLook cinemachine;
    public bool moveFreeLook;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _initialPosition = targetToAim.localPosition;
    }

    void Update()
    {
        if (moveFreeLook)
            MoveTarget();
        else
            MoveTargetWithDirection();  
    }

    private void MoveTargetWithDirection()
    {
        Vector2 aimDirection = _player.Inputs.GetAimMoveDirection();
        Vector3 newPos = targetToAim.localPosition;

        newPos.x += aimDirection.x * movementSpeed * Time.deltaTime;
        newPos.y += aimDirection.y * movementSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, movementRangeX.x, movementRangeX.y);
        newPos.y = Mathf.Clamp(newPos.y, movementRangeY.x, movementRangeY.y);

        newPos.z = _initialPosition.z;

        targetToAim.localPosition = newPos;
    }

    private void MoveTarget()
    {
        Vector3 newPos = targetToAim.localPosition;

        newPos.y = Mathf.Lerp(movementRangeY.y, movementRangeY.x, cinemachine.m_YAxis.Value);
        newPos.z = _initialPosition.z;

        targetToAim.localPosition = newPos;
    }


}
