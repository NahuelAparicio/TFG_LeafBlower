using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    [Header("Stamina Stadistics:")]
    public float staminaConsumedOverTime = 0.1f;
    public float consumeRate = 0.1f;

    public float staminaRecoverOverTime = 0.1f;
    public float recoveryRate = 0.1f;

    public float timeToStartRecovering = 1f;

    private BlowerController _blower;
    private float _currentStamina;
    private float _recoveryTimer = 0f;

    public bool isConsumingStamina = false;

    private void Start()
    {
        _blower = GetComponent<BlowerController>();
        _currentStamina = _blower.Stats.maxStamina.Value;
        _blower.Hud.ResetStaminaBar();
    }
    private void Update()
    {
        if (_blower.canUseLeafBlower && isConsumingStamina)
        {
            ConsumeStamina();
        }
        else
        {
            HandleStaminaRecover();
        }

        if (!_blower.canUseLeafBlower && (_currentStamina / _blower.Stats.maxStamina.Value) >= 0.15f)
        {
            EnableLeafBlower();
        }
    }

    private void HandleStaminaRecover()
    {
        if (!isConsumingStamina && _currentStamina < _blower.Stats.maxStamina.Value)
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
        _blower.Hud.UpdateStaminaBar(_currentStamina, _blower.Stats.maxStamina.Value);
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
            _blower.aspirarVFX.SetActive(false);
            _blower.blowVFX.SetActive(false);
            _blower.Player.Sounds.StopEngineSound();
            _blower.isHovering = false;
            isConsumingStamina = false;
            _recoveryTimer = 0f;
        }
    }
    public void StartConsumingStamina()
    {
        if (isConsumingStamina || !_blower.canUseLeafBlower) return;

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
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _blower.Stats.maxStamina.Value);
        _blower.Hud.UpdateStaminaBar(_currentStamina, _blower.Stats.maxStamina.Value);
    }

    public void EnableLeafBlower()
    {
        if (_blower.canUseLeafBlower) return;
        _blower.canUseLeafBlower = true;
    } 
    public void DisableLeafBlower()
    {
        if (!_blower.canUseLeafBlower) return;
        _blower.canUseLeafBlower = false;
    } 
    public bool HasStamina() => _currentStamina > 0;

}
