using UnityEngine;

[CreateAssetMenu(fileName = "DashMovement", menuName = "Movements/Dash")]
public class DashHandler : MovementBehavior
{
    [SerializeField] private int _maxDashes;
    private int _currentDashes;
    public override bool CanExecuteMovement() => _maxDashes > _currentDashes;
    public override void ExecuteMovement(Rigidbody rb, float force)
    {
        rb.AddForce(rb.transform.forward * force, ForceMode.Impulse);
        _currentDashes++;
    }
    public override void ResetMovement() => _currentDashes = 0;    
}
