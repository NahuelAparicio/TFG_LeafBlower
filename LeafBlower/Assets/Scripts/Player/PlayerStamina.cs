using UnityEngine;

public class PlayerStamina : StaminaHandler
{
    private PlayerController _player;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        base.Start();
        _player.Hud.ResetStaminaBar();
    }

    public override void ConsumeValueStamina(float value)
    {
        base.ConsumeValueStamina(value);
        _player.Hud.UpdateStaminaBar(CurrentStamina, MaxStamina);
    }

    protected override void ConsumeStamina()
    {
        if (CurrentStamina > 0)
        {
            if(_player.Movement.isHovering)
            {
                ModifyStamina(-staminaConsumeHovering, consumeRate);
            }
            else if(_player.Inputs.isSprinting)
            {
                ModifyStamina(-staminaConsumeSprinting, consumeRate);
            }
        }

        if (CurrentStamina <= 0)
        {
            _player.Inputs.isSprinting = false;
            _player.Movement.isHovering = false;
            isConsumingStamina = false;
            _recoveryTimer = 0f;
        }
    }
    protected override void ModifyStamina(float value, float rate)
    {
        base.ModifyStamina(value, rate);
        _player.Hud.UpdateStaminaBar(CurrentStamina, MaxStamina);
    }
}
