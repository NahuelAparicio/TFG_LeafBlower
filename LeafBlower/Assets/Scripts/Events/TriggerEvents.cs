using System;

public class TriggerEvents 
{
    public event Action onTriggerButton;
    public event Action<string> onTriggerBasket;
    public event Action<string> onTriggerFootball;

    public void TriggerButton() => onTriggerButton?.Invoke();
    public void TriggerBasket(string id) => onTriggerBasket?.Invoke(id);
    public void TriggerFootball(string id) => onTriggerFootball?.Invoke(id);
}
