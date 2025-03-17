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
        _actions.Blower.SaveObject.performed += SaveObject_performed;
        _actions.Blower.RotateObject.performed += RotateObject_performed;
    }

    private void Update()
    {
        if(!IsAspiringInputPressed() && !IsBlowingInputPressed())
        {
            _blower.blowVFX.SetActive(false);
            _blower.aspirarVFX.SetActive(false);

        }
    }

    private void RotateObject_performed(InputAction.CallbackContext obj)
    {
        //if(_blower.Aspirer.attachableObject.IsAttached)
        //    _blower.Aspirer.attachableObject.Shootable.OnRotate(_blower.FirePoint.forward);
    }

    public bool IsBlowingInputPressed() => _actions.Blower.Blow.IsPressed();
    public bool IsAspiringInputPressed() => _actions.Blower.Aspire.IsPressed();

    private void Blow_performed(InputAction.CallbackContext context)
    {
        if (_blower.Player.IsTalking) return;

        //_blower.Handler.StartConsumingStamina();
        if (_blower.Aspirer.attachableObject.IsAttached)
        {
            _blower.Aspirer.wasShootPressed = true;
        }
        if (IsAspiringInputPressed()) return;
        _blower.blowVFX.SetActive(true);
        _blower.Player.Sounds.PlayEngineSound();
    }


    private void Blow_canceled(InputAction.CallbackContext context)
    {
        if (_blower.isHovering || IsAspiringInputPressed()) return;
        _blower.StaminaHandler.StopConsumingStamina();
        _blower.blowVFX.SetActive(false);
        _blower.Player.Sounds.StopEngineSound();
    }

    private void Aspire_performed(InputAction.CallbackContext context)
    {
        if (_blower.Player.IsTalking) return;

        //_blower.Handler.StartConsumingStamina();
        if (IsBlowingInputPressed()) return;
        _blower.aspirarVFX.SetActive(true);
        _blower.Player.Sounds.PlayEngineSound();

    }

    private void Aspire_canceled(InputAction.CallbackContext context)
    {
        if (IsBlowingInputPressed()) return;
        _blower.StaminaHandler.StopConsumingStamina();
        _blower.aspirarVFX.SetActive(false);
        _blower.Player.Sounds.StopEngineSound();

    }
    private void SaveObject_performed(InputAction.CallbackContext context)
    {
        if (_blower.Player.IsTalking) return;
        if (_blower.Aspirer.ClosestObject == null && !_blower.Player.Inventory.IsObjectSaved()) return;        

        if (_blower.Player.Inventory.IsObjectSaved())
        {
            _blower.Player.Inventory.RemoveObject();
        }
        else
        {
            if (_blower.Aspirer.ClosestObject.GetComponent<MovableObject>() != null) return;
            if (!_blower.Aspirer.ClosestObject.GetComponent<ShootableObject>().canBeSaved) return;
            _blower.Aspirer.AttachObjectOnSave();
            _blower.Player.Inventory.SaveObject(_blower.Aspirer.ClosestObject.gameObject, _blower.Aspirer.ClosestObject.uiImage);
            _blower.Aspirer.attachableObject.DetachOnSave();   
        }

    }

    private void OnDestroy()
    {
        _actions.Blower.Disable();
        _actions.Blower.Blow.performed -= Blow_performed;
        _actions.Blower.Blow.canceled -= Blow_canceled;
        _actions.Blower.Aspire.performed -= Aspire_performed;
        _actions.Blower.Aspire.canceled -= Aspire_canceled;
        _actions.Blower.SaveObject.performed -= SaveObject_performed;
    }
}
