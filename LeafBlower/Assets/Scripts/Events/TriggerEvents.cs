using System;

public class TriggerEvents 
{
    public event Action onTriggerButton;
    public event Action onTriggerBasket;
    public event Action onTriggerFootball;

    public void TriggerButton() => onTriggerButton?.Invoke();
    public void TriggerBasket() => onTriggerBasket?.Invoke();
    public void TriggerFootball() => onTriggerFootball?.Invoke();
}
