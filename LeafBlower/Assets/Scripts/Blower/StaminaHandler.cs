using System.Collections;
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
    public bool HasStamina() => _currentStamina > 0;
    public bool HasMaxStamina() => _currentStamina >= _blowerController.Stats.maxStamina.Value;

    public bool isConsumingStamina = false;
    public bool isRecoveringStamina = false;

    private void Awake()
    {

    }
    private void Start()
    {
        _blowerController = GetComponent<BlowerController>();
        _currentStamina = _blowerController.Stats.maxStamina.Value;
        _blowerController.Hud.ResetStaminaBar();
    }

    private void Update()
    {
        if(!_blowerController.canUseLeafBlower)
        {
            if(Mathf.Clamp01(_currentStamina / _blowerController.Stats.maxStamina.Value) > 0.15f)
            {
                EnableLeafBlower();
            }
        }
    }

    public void ConsumeStaminaOverTime()
    {
        if(HasStamina() && !isConsumingStamina)
        {
            StopRecoveringStamina();
            isConsumingStamina = true;
            StartCoroutine(ConsumeStamina());
        }
    }

    public void StopConsumingStamina()
    {
        isConsumingStamina = false;
        isRecoveringStamina = false;
        StopCoroutine(ConsumeStamina());
    }

    private void StopRecoveringStamina()
    {
        isRecoveringStamina = false;
        StopCoroutine(RecoverStamina());
    }

    public void RecoverStaminaOverTime()
    {
        if(!isRecoveringStamina)
        {
            isRecoveringStamina = true;
            StartCoroutine(RecoverStamina());
        }
    }

    public void EnableLeafBlower() => _blowerController.canUseLeafBlower = true;
    public void DisableLeafBlower() => _blowerController.canUseLeafBlower = false;

    private IEnumerator ConsumeStamina()
    {
        while(HasStamina() && isConsumingStamina)
        {
            _currentStamina -= staminaConsumedOverTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, _blowerController.Stats.maxStamina.Value);
            _blowerController.Hud.UpdateStaminaBar(_currentStamina, _blowerController.Stats.maxStamina.Value);
            yield return new WaitForSeconds(consumeRate);
        }

        isConsumingStamina = false;

        if (_currentStamina <= 0)
        {
            DisableLeafBlower();
            _blowerController.isHovering = false;
        }
    }

    private IEnumerator RecoverStamina()
    {
        _blowerController.ResetStaminaCurrentTime();
        while (!isConsumingStamina && !HasMaxStamina())
        {
            _currentStamina += staminaRecoverOverTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, _blowerController.Stats.maxStamina.Value);
            _blowerController.Hud.UpdateStaminaBar(_currentStamina, _blowerController.Stats.maxStamina.Value);
            yield return new WaitForSeconds(recoveryRate);
        }

        if (HasMaxStamina())
        {
            _currentStamina = _blowerController.Stats.maxStamina.Value;
        }
        isRecoveringStamina = false;
    }
}
