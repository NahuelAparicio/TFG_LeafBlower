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
    }
    public Vector2 GetBlowerMoveDirection() => _actions.Blower.BlowerMove.ReadValue<Vector2>(); // Right Stick -- Mouse Scroll

    private void Blow_performed(InputAction.CallbackContext context)
    {
        //Active blow collider
    }
    private void Blow_canceled(InputAction.CallbackContext context)
    {
        //Deactive collider
    }

    private void Aspire_performed(InputAction.CallbackContext context)
    {
        //Active aspire collider
    }

    private void Aspire_canceled(InputAction.CallbackContext context)
    {
        //Deactive collider
    }

}
