using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    [SerializeField] private Stat _maxStamina;
    private float _currentStamina;
    protected float _recoveryTimer = 0f;

    [Header("Stamina Stadistics:")]
    public float staminaConsumedOverTime = 0.1f;
    public float consumeRate = 0.1f;

    public float staminaRecoverOverTime = 0.1f;
    public float recoveryRate = 0.1f;

    public float timeToStartRecovering = 1f;

    public bool isConsumingStamina = false;

    
    
    public float CurrentStamina => _currentStamina;
    public float MaxStamina => _maxStamina.Value;

    protected virtual void Start()
    {
        _currentStamina = _maxStamina.Value;
    }

    protected virtual void Update()
    {
        if (isConsumingStamina)
        {
            ConsumeStamina();
        }
        else
        {
            HandleStaminaRecover();
        }

        //if ((_currentStamina / MaxStamina) >= 0.15f)
        //{
        //    EnableLeafBlower();
        //}
    }

    protected virtual void HandleStaminaRecover()
    {
        if (!isConsumingStamina && _currentStamina < MaxStamina)
        {
            _recoveryTimer += Time.deltaTime;

            if (_recoveryTimer >= timeToStartRecovering)
            {
                ModifyStamina(staminaRecoverOverTime, recoveryRate);
            }
        }
    }
    public virtual void ConsumeValueStamina(float value)
    {
        _currentStamina -= value;
        _recoveryTimer = 0f;

    }
    protected virtual void ConsumeStamina()
    {
        if (_currentStamina > 0)
        {
            ModifyStamina(-staminaConsumedOverTime, consumeRate);
        }

        if (_currentStamina <= 0)
        {
            isConsumingStamina = false;
            _recoveryTimer = 0f;
        }
    }
    public virtual void StartConsumingStamina()
    {
        if (isConsumingStamina) return;

        if (_currentStamina > 0)
        {
            isConsumingStamina = true;
            _recoveryTimer = 0f;
        }
    }

    public virtual void StopConsumingStamina()
    {
        isConsumingStamina = false;
    }

    protected virtual void ModifyStamina(float value, float rate)
    {
        _currentStamina += value * (Time.deltaTime / rate);
        _currentStamina = Mathf.Clamp(_currentStamina, 0, MaxStamina);
    }

    //public virtual void EnableLeafBlower()
    //{
    //    if (_blower.canUseLeafBlower) return;
    //    _blower.canUseLeafBlower = true;
    //} 
    //public virtual void DisableLeafBlower()
    //{
    //    if (!_blower.canUseLeafBlower) return;
    //    _blower.canUseLeafBlower = false;
    //} 
    public bool HasStamina() => _currentStamina > 0;

}
