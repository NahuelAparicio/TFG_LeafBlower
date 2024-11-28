using UnityEngine;
using UnityEngine.InputSystem;

public class BlowerInputs : MonoBehaviour
{
    private BlowerController _blower;
    private BlowerInputsActions _actions;
    [SerializeField] private GameObject _mainCamera, _aimCamera; 
    private void Awake()
    {
        NoAim();
        _blower = GetComponent<BlowerController>();
        _actions = new BlowerInputsActions();
        _actions.Blower.Enable();
        _actions.Blower.Blow.performed += Blow_performed;
        _actions.Blower.Blow.canceled += Blow_canceled;
        _actions.Blower.Aspire.performed += Aspire_performed;
        _actions.Blower.Aspire.canceled += Aspire_canceled;
        _actions.Blower.SaveObject.performed += SaveObject_performed;
    }
    public bool IsBlowingInputPressed() => _actions.Blower.Blow.IsPressed();
    public bool IsAspiringInputPressed() => _actions.Blower.Aspire.IsPressed();

    private void Blow_performed(InputAction.CallbackContext context)
    {
        _blower.Handler.ConsumeStaminaOverTime();
        // _blower.Blower.EnableCollider();
        Aim();
    }
    private void Blow_canceled(InputAction.CallbackContext context)
    {
        //  _blower.Blower.DisableCollider();
        _blower.Handler.ReEnableRecoverStamina();
        NoAim();
    }

    private void Aspire_performed(InputAction.CallbackContext context)
    {
        _blower.Handler.ConsumeStaminaOverTime();

        //  _blower.Aspirer.EnableCollider();
        Aim();
    }

    private void Aspire_canceled(InputAction.CallbackContext context)
    {
        _blower.Handler.ReEnableRecoverStamina();

        //   _blower.Aspirer.DisableCollider();
        NoAim();
    }

    private void Aim()
    {
        //_mainCamera.SetActive(false);
        //_aimCamera.SetActive(true);
    }

    private void NoAim()
    {
        //_mainCamera.SetActive(true);
        //_aimCamera.SetActive(false);
    }
    private void SaveObject_performed(InputAction.CallbackContext context)
    {
        if (!_blower.Aspirer.IsObjectAttached && !_blower.Player.Inventory.IsObjectSaved()) return;
        if(_blower.Player.Inventory.IsObjectSaved())
        {
            _blower.Player.Inventory.RemoveObject();
        }
        else
        {
            _blower.Player.Inventory.SaveObject(_blower.Aspirer.AttachedObject.Item1.gameObject);
            _blower.Aspirer.SaveObject();   
        }
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
