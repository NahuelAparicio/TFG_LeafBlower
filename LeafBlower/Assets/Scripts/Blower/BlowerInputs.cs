using UnityEngine;
using UnityEngine.InputSystem;

public class BlowerInputs : MonoBehaviour
{
    private BlowerController _blower;
    private BlowerInputsActions _actions;


    private void Awake()
    {
        _blower = GetComponent<BlowerController>();
        _actions = new BlowerInputsActions();
        _actions.Blower.Enable();
        _actions.Blower.Blow.performed += Blow_performed;
        _actions.Blower.Blow.canceled += Blow_canceled;
        _actions.Blower.Aspire.performed += Aspire_performed;
        _actions.Blower.Aspire.canceled += Aspire_canceled;
    }
    public bool IsBlowingInputPressed() => _actions.Blower.Blow.IsPressed();
    public bool IsAspiringInputPressed() => _actions.Blower.Aspire.IsPressed();
    public Vector2 GetBlowerMoveDirection() => _actions.Blower.BlowerMove.ReadValue<Vector2>(); // Right Stick -- Mouse Scroll

    private void Blow_performed(InputAction.CallbackContext context)
    {
        _blower.BlowerForce.EnableCollider();
    }
    private void Blow_canceled(InputAction.CallbackContext context)
    {
        _blower.BlowerForce.DisableCollider();
    }

    private void Aspire_performed(InputAction.CallbackContext context)
    {
        _blower.AspirerForce.EnableCollider();
    }

    private void Aspire_canceled(InputAction.CallbackContext context)
    {
        _blower.AspirerForce.DisableCollider();
    }

    private void OnDestroy()
    {
        _actions.Blower.Disable();
        _actions.Blower.Blow.performed -= Blow_performed;
        _actions.Blower.Blow.canceled -= Blow_canceled;
        _actions.Blower.Aspire.performed -= Aspire_performed;
        _actions.Blower.Aspire.canceled -= Aspire_canceled;
    }
}
