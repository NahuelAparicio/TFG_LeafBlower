using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    [Header("Stamina Stadistics:")]
    public float staminaConsumedOverTime = 0.1f;
    public float consumeRate = 0.1f;

    public float staminaRecoverOverTime = 0.1f;
    public float recoveryRate = 0.1f;

    public float timeToStartRecovering = 1f;

    private BlowerController _blowerController;
    private float _currentStamina;
    private float _recoveryTimer = 0f;

    public bool isConsumingStamina = false;

    private void Start()
    {
        _blowerController = GetComponent<BlowerController>();
        _currentStamina = _blowerController.Stats.maxStamina.Value;
        _blowerController.Hud.ResetStaminaBar();
    }
    private void Update()
    {
        if (_blowerController.canUseLeafBlower && isConsumingStamina)
        {
            ConsumeStamina();
        }
        else
        {
            HandleStaminaRecover();
        }

        if (!_blowerController.canUseLeafBlower && (_currentStamina / _blowerController.Stats.maxStamina.Value) >= 0.15f)
        {
            EnableLeafBlower();

        }
    }

    private void HandleStaminaRecover()
    {
        if (!isConsumingStamina && _currentStamina < _blowerController.Stats.maxStamina.Value)
        {
            _recoveryTimer += Time.deltaTime;

            if (_recoveryTimer >= timeToStartRecovering)
            {
                ModifyStamina(staminaRecoverOverTime, recoveryRate);
            }
        }
    }
    public void ConsumeValueStamina(float value)
    {
        _currentStamina -= value;
        _blowerController.Hud.UpdateStaminaBar(_currentStamina, _blowerController.Stats.maxStamina.Value);
        _recoveryTimer = 0f;

    }
    private void ConsumeStamina()
    {
        if (_currentStamina > 0)
        {
            ModifyStamina(-staminaConsumedOverTime, consumeRate);
        }

        if (_currentStamina <= 0)
        {
            DisableLeafBlower();
            _blowerController.isHovering = false;
            isConsumingStamina = false;
            _recoveryTimer = 0f;
        }
    }
    public void StartConsumingStamina()
    {
        if (isConsumingStamina || !_blowerController.canUseLeafBlower) return;

        if (_currentStamina > 0)
        {
            isConsumingStamina = true;
            _recoveryTimer = 0f;
        }
    }

    public void StopConsumingStamina()
    {
        isConsumingStamina = false;
    }

    private void ModifyStamina(float value, float rate)
    {
        _currentStamina += value * Time.deltaTime / rate;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _blowerController.Stats.maxStamina.Value);
        _blowerController.Hud.UpdateStaminaBar(_currentStamina, _blowerController.Stats.maxStamina.Value);
    }

    public void EnableLeafBlower()
    {
        if (_blowerController.canUseLeafBlower) return;
        _blowerController.canUseLeafBlower = true;
    } 
    public void DisableLeafBlower()
    {
        if (!_blowerController.canUseLeafBlower) return;
        _blowerController.canUseLeafBlower = false;
    } 
    public bool HasStamina() => _currentStamina > 0;

}
