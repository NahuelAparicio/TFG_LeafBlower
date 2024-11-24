using Unity.VisualScripting;
using UnityEngine;

public class BlowerController : MonoBehaviour
{
    #region Variables
    private BlowerInputs _inputs;
    private BlowerStats _stats;
    private StaminaHandler _handler;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private PlayerController _player;
    [SerializeField] private BlowerForce _blower;
    [SerializeField] private AspirerForce _aspirer;
    [SerializeField] private BlowerHUD _hud;

    private bool _wasInteracting;
    private float _currentTime;
    public bool canUseLeafBlower;
    public bool isHovering;

    #endregion
    #region Properties
    public PlayerController Player => _player;
    public BlowerStats Stats => _stats;
    public BlowerInputs Inputs => _inputs;
    public BlowerForce Blower => _blower;
    public AspirerForce Aspirer => _aspirer;
    public Transform FirePoint => _firePoint;
    public BlowerHUD Hud => _hud;
    public StaminaHandler Handler => _handler;
    #endregion

    private void Awake()
    {
        _handler = GetComponent<StaminaHandler>();
        _inputs = GetComponent<BlowerInputs>();
        _stats = GetComponent<BlowerStats>();
        _hud = GetComponent<BlowerHUD>();
        isHovering = false;
    }

    private void Update()
    {
        bool isBlowingAspiringHovering = IsBlowing() || IsAspirating() || isHovering;
        //&& _aspirer.ObjectAttached
        if (isBlowingAspiringHovering )
        {
            _handler.ConsumeStaminaOverTime();
        }
        else
        {
            _currentTime += Time.deltaTime;
            if(_wasInteracting)
            {
                _handler.StopConsumingStamina();
                _currentTime = 0;
            }
            if (_currentTime >= _handler.timeToStartRecovering)
            {
                _handler.RecoverStaminaOverTime();
                
            }
        }

        _wasInteracting = isBlowingAspiringHovering;
    }

    // Returns if Blow function is being used, while check if can be used
    public bool IsBlowing() => CanUseLeafBlower() && _inputs.IsBlowingInputPressed() && !_inputs.IsAspiringInputPressed();

    // Returns if Aspire function is being used, while checks if can be used
    public bool IsAspirating() => CanUseLeafBlower() && _inputs.IsAspiringInputPressed() && !_inputs.IsBlowingInputPressed() && !isHovering;

    //Returns if the whole machine (Leaf Blower) can be used
    public bool CanUseLeafBlower() => _handler.HasStamina() && canUseLeafBlower;

    //Returns if the object attached is being shoot
    public bool IsShooting() => _aspirer.ObjectAttached && _inputs.IsBlowingInputPressed();

    public Vector3 GetPlayerDirection() => _player.Inputs.GetMoveDirection();

    public float DistanceToFirePoint(Vector3 position) => Vector3.Distance(_firePoint.position, position);

    public Vector3 DirectionToFirePointNormalized(Vector3 position) => (_firePoint.position - position).normalized;
    public Vector3 DirectionFromFirePointNormalized(Vector3 position) => (position - _firePoint.position).normalized;

    public void ResetStaminaCurrentTime() => _currentTime = 0;
}
