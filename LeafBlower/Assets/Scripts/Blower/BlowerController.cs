using UnityEngine;

public class BlowerController : MonoBehaviour
{
    // en X de 90 a -40º
    #region Variables
    private BlowerInputs _inputs;
    private BlowerStats _stats;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private PlayerController _player;
    [SerializeField] private BlowerForce _blowerForce;
    [SerializeField] private AspirerForce _aspirerForce;
    #endregion
    #region Properties
    public PlayerController Player => _player;
    public BlowerStats Stats => _stats;
    public BlowerInputs Inputs => _inputs;
    public BlowerForce BlowerForce => _blowerForce;
    public AspirerForce AspirerForce => _aspirerForce;
    public Transform FirePoint => _firePoint;
    #endregion

    private void Awake()
    {
        _inputs = GetComponent<BlowerInputs>();
        _stats = GetComponent<BlowerStats>();
    }

    // Returns if Blow function is being used, while check if can be used
    public bool IsBlowing() => CanUseLeafBlower() && _inputs.IsBlowingInputPressed();

    // Returns if Aspire function is being used, while checks if can be used
    public bool IsAspirating() => CanUseLeafBlower() && _inputs.IsAspiringInputPressed() && !_inputs.IsBlowingInputPressed();

    //Returns if the whole machine (Leaf Blower) can be used
    public bool CanUseLeafBlower() => _stats.HasStamina() && !_aspirerForce.ObjectAttached;

    //Returns if the object attached is being shoot
    public bool IsShooting() => _aspirerForce.ObjectAttached && _inputs.IsBlowingInputPressed() && _stats.HasStamina();

    public Vector3 GetPlayerDirection() => _player.Inputs.GetMoveDirection();

    public float DistanceToFirePoint(Vector3 position) => Vector3.Distance(_firePoint.position, position);

    public Vector3 DirectionToFirePointNormalized(Vector3 position) => (_firePoint.position - position).normalized;
    public Vector3 DirectionFromFirePointNormalized(Vector3 position) => (position - _firePoint.position).normalized;
}
